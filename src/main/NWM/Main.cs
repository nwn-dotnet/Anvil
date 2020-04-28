using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using NLog;
using NWM.Core;
using NWN;
using NWNX;

namespace NWM
{
  public static class Main
  {
    public const uint ObjectInvalid = 0x7F000000;
    public static uint ObjectSelf { get; private set; } = ObjectInvalid;

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static readonly Stack<ScriptContext> scriptContexts = new Stack<ScriptContext>();
    private static readonly Dictionary<ulong, Closure> closures = new Dictionary<ulong, Closure>();
    private static ulong nextEventId = 0;

    private static ServiceManager serviceManager;
    private static DispatchServiceManager handlerDispatcher;
    private static LoopService loopService;

    private static bool initialized;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Bootstrap(IntPtr arg, int argLength)
    {
      Log.Info("--------Neverwinter Managed--------");

      Internal.AllHandlers handlers;
      handlers.MainLoop = OnMainLoop;
      handlers.RunScript = OnRunScript;
      handlers.Closure = OnClosure;

      int retVal = Internal.Bootstrap(arg, argLength, handlers);

      if (retVal == 0)
      {
        serviceManager = new ServiceManager();
        AppendAssemblyToPath();
      }

      return retVal;
    }

    // Needed to allow native libs to be loaded.
    private static void AppendAssemblyToPath()
    {
      string envPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
      string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

      Environment.SetEnvironmentVariable("PATH", $"{envPath}; {assemblyDir}");
    }

    private static void OnMainLoop(ulong frame)
    {
      try
      {
        loopService?.Update();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private static int OnRunScript(string script, uint oidSelf)
    {
      int retVal = 0;
      ObjectSelf = oidSelf;
      scriptContexts.Push(new ScriptContext { OwnerObject = oidSelf, ScriptName = script });

      try
      {
        if (!initialized)
        {
          Init();
          initialized = true;
        }

        retVal = handlerDispatcher.OnRunScript(script, oidSelf);
      }
      catch (Exception e)
      {
        Log.Error(e);

        // We want the server to crash if init fails.
        if (!initialized)
        {
          throw;
        }
      }

      scriptContexts.Pop();
      ObjectSelf = scriptContexts.Count == 0 ? ObjectInvalid : scriptContexts.Peek().OwnerObject;
      return retVal;
    }

    private static void OnClosure(ulong eid, uint oidSelf)
    {
      uint old = ObjectSelf;
      ObjectSelf = oidSelf;

      try
      {
        closures[eid].Run();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }

      closures.Remove(eid);
      ObjectSelf = old;
    }

    private static void Init()
    {
      CheckPluginDependencies();
      serviceManager.Verify();
      handlerDispatcher = serviceManager.GetService<DispatchServiceManager>();
      loopService = serviceManager.GetService<LoopService>();
      serviceManager.GetService<AttributeDispatchService>().Init(serviceManager.GetRegisteredServices());
    }

    private static void CheckPluginDependencies()
    {
      Log.Info("Checking Plugin Dependencies");
      PluginUtils.AssertPluginExists<UtilPlugin>();
      PluginUtils.AssertPluginExists<ObjectPlugin>();
    }

    internal static void ClosureAssignCommand(uint obj, ActionDelegate func)
    {
      if (Internal.NativeFunctions.ClosureAssignCommand(obj, nextEventId) != 0)
      {
        closures.Add(nextEventId++, new Closure { OwnerObject = obj, Run = func });
      }
    }

    internal static void ClosureDelayCommand(uint obj, float duration, ActionDelegate func)
    {
      if (Internal.NativeFunctions.ClosureDelayCommand(obj, duration, nextEventId) != 0)
      {
        closures.Add(nextEventId++, new Closure { OwnerObject = obj, Run = func });
      }
    }

    internal static void ClosureActionDoCommand(uint obj, ActionDelegate func)
    {
      if (Internal.NativeFunctions.ClosureActionDoCommand(obj, nextEventId) != 0)
      {
        closures.Add(nextEventId++, new Closure { OwnerObject = obj, Run = func });
      }
    }

    private struct ScriptContext
    {
      public uint OwnerObject;
      public string ScriptName;
    }

    private struct Closure
    {
      public uint OwnerObject;
      public ActionDelegate Run;
    }
  }
}