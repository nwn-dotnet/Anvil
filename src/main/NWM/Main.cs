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
  public class Main : IGameManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const string ShutdownScript = "__nwm_stop";
    private const uint ObjectInvalid = 0x7F000000;
    public uint ObjectSelf { get; private set; } = ObjectInvalid;

    public static Main Instance { get; private set; }

    // Events
    public event Action OnInitComplete;

    // Native Management
    private readonly Stack<ScriptContext> scriptContexts = new Stack<ScriptContext>();
    private readonly Dictionary<ulong, Closure> closures = new Dictionary<ulong, Closure>();
    private ulong nextEventId = 0;

    // Core Services
    public ServiceManager ServiceManager { get; private set; }

    private IRunScriptHandler runScriptHandler;
    private ILoopHandler loopHandler;

    // Bootstrap
    private readonly IBindingInstaller bindingInstaller;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Bootstrap(IntPtr arg, int argLength) => Bootstrap(arg, argLength, new ServiceBindingInstaller());

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Bootstrap(IntPtr arg, int argLength, IBindingInstaller bindingInstaller)
    {
      Instance = new Main(bindingInstaller);
      return Internal.Bootstrap(arg, argLength, Instance);
    }

    public Main(IBindingInstaller bindingInstaller)
    {
      this.bindingInstaller = bindingInstaller;

      Log.Info("--------Neverwinter Managed--------");
      AppendAssemblyToPath();
    }

    // Needed to allow native libs to be loaded.
    private void AppendAssemblyToPath()
    {
      string envPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
      string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

      Environment.SetEnvironmentVariable("PATH", $"{envPath}; {assemblyDir}");
    }

    private void Init()
    {
      ServiceManager serviceManager = new ServiceManager(bindingInstaller);
      CheckPluginDependencies();
      ServiceManager.InitServices();
      runScriptHandler = serviceManager.GetService<IRunScriptHandler>();
      loopHandler = serviceManager.GetService<ILoopHandler>();

      ServiceManager = serviceManager;
    }

    private static void CheckPluginDependencies()
    {
      Log.Info("Checking Plugin Dependencies");
      PluginUtils.AssertPluginExists<UtilPlugin>();
      PluginUtils.AssertPluginExists<ObjectPlugin>();
    }

    private void Shutdown()
    {
      ServiceManager?.Dispose();
      ServiceManager = null;
    }

    public void OnMainLoop(ulong frame)
    {
      try
      {
        loopHandler?.OnLoop();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    public int OnRunScript(string script, uint oidSelf)
    {
      int retVal = 0;
      ObjectSelf = oidSelf;
      scriptContexts.Push(new ScriptContext { OwnerObject = oidSelf, ScriptName = script });

      try
      {
        if (ServiceManager == null)
        {
          Init();
        }
        else if (script == ShutdownScript)
        {
          Shutdown();
        }

        retVal = runScriptHandler.OnRunScript(script, oidSelf);
      }
      catch (Exception e)
      {
        Log.Error(e);

        // We want the server to crash if init fails.
        if (ServiceManager == null)
        {
          throw;
        }
      }

      scriptContexts.Pop();
      ObjectSelf = scriptContexts.Count == 0 ? ObjectInvalid : scriptContexts.Peek().OwnerObject;
      return retVal;
    }

    public void OnClosure(ulong eid, uint oidSelf)
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

    public void ClosureAssignCommand(uint obj, ActionDelegate func)
    {
      if (Internal.NativeFunctions.ClosureAssignCommand(obj, nextEventId) != 0)
      {
        closures.Add(nextEventId++, new Closure { OwnerObject = obj, Run = func });
      }
    }

    public void ClosureDelayCommand(uint obj, float duration, ActionDelegate func)
    {
      if (Internal.NativeFunctions.ClosureDelayCommand(obj, duration, nextEventId) != 0)
      {
        closures.Add(nextEventId++, new Closure { OwnerObject = obj, Run = func });
      }
    }

    public void ClosureActionDoCommand(uint obj, ActionDelegate func)
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