using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NLog;
using NWN.Core;
using NWN.Core.NWNX;
using NWN.Plugins;
using NWN.Services;
using NWNX;

namespace NWN
{
  /// <summary>
  /// Handles bootstrap and interop between %NWN, %NWN.Core and the managed %API. The entry point of the implementing module should point to this class.<br/>
  /// Until <see cref="Init"/> is called, all APIs are unavailable for usage.
  /// </summary>
  public class NManager : IGameManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const uint ObjectInvalid = 0x7F000000;

    uint IGameManager.ObjectSelf => this.ObjectSelf;
    internal uint ObjectSelf { get; private set; } = ObjectInvalid;

    public static NManager Instance { get; private set; }

    // Events
    public event Action OnInitComplete;

    // Native Management
    private readonly Stack<ScriptContext> scriptContexts = new Stack<ScriptContext>();
    private readonly Dictionary<ulong, Closure> closures = new Dictionary<ulong, Closure>();
    private ulong nextEventId = 0;

    // Core Services
    internal ServiceManager ServiceManager { get; private set; }

    private IRunScriptHandler runScriptHandler;
    private ILoopHandler loopHandler;

    // Bootstrap
    private readonly IBindingInstaller bindingInstaller;
    internal readonly ITypeLoader TypeLoader;

    /// <summary>
    /// Initialises the managed library, loading all defined services.
    /// </summary>
    /// <param name="arg">The NativeHandles pointer, provided by the NWNX bootstrap entry point.</param>
    /// <param name="argLength">The size of the NativeHandles bootstrap structure, provided by the NWNX entry point.</param>
    /// <param name="bindingInstaller">An optional custom binding installer to use instead of the default <see cref="ServiceBindingInstaller"/>.</param>
    /// <returns>The init result code to return back to NWNX.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Init(IntPtr arg, int argLength, IBindingInstaller bindingInstaller = default)
    {
      bindingInstaller ??= new ServiceBindingInstaller();
      Instance = new NManager(bindingInstaller, new PluginLoader());
      return NWNCore.Init(arg, argLength, Instance);
    }

    private NManager(IBindingInstaller bindingInstaller, PluginLoader typeLoader)
    {
      this.bindingInstaller = bindingInstaller;
      this.TypeLoader = typeLoader;
    }

    private static void CheckPluginDependencies()
    {
      Log.Info("Checking Plugin Dependencies");
      PluginUtils.AssertPluginExists<UtilPlugin>();
      PluginUtils.AssertPluginExists<ObjectPlugin>();
    }

    private void Start()
    {
      LogManager.Configuration.Variables["nwn_home"] = UtilPlugin.GetUserDirectory();

      ServiceManager serviceManager = new ServiceManager(TypeLoader, bindingInstaller);
      CheckPluginDependencies();
      serviceManager.InitServices();
      runScriptHandler = serviceManager.GetService<IRunScriptHandler>();
      loopHandler = serviceManager.GetService<ILoopHandler>();

      ServiceManager = serviceManager;
      OnInitComplete?.Invoke();
    }

    void IGameManager.OnSignal(string signal)
    {
      switch (signal)
      {
        case "ON_MODULE_LOAD_FINISH":
          Start();
          break;
        case "ON_DESTROY_SERVER":
          Shutdown();
          break;
        default:
          Log.Debug($"Unhandled Signal: \"{signal}\"");
          break;
      }
    }

    private void Shutdown()
    {
      ServiceManager?.Dispose();
      ServiceManager = null;
    }

    void IGameManager.OnMainLoop(ulong frame)
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

    int IGameManager.OnRunScript(string script, uint oidSelf)
    {
      int retVal = 0;
      ObjectSelf = oidSelf;
      scriptContexts.Push(new ScriptContext { OwnerObject = oidSelf, ScriptName = script });

      try
      {
        // Ignored Scripts
        if (script == EnvironmentConfig.ModStartScript || script == EnvironmentConfig.CoreShutdownScript)
        {
          return retVal;
        }

        retVal = runScriptHandler.OnRunScript(script, oidSelf);
      }
      catch (Exception e)
      {
        Log.Error(e);
      }

      scriptContexts.Pop();
      ObjectSelf = scriptContexts.Count == 0 ? ObjectInvalid : scriptContexts.Peek().OwnerObject;
      return retVal;
    }

    void IGameManager.OnClosure(ulong eid, uint oidSelf)
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

    void IGameManager.ClosureAssignCommand(uint obj, ActionDelegate func)
    {
      if (VM.ClosureAssignCommand(obj, nextEventId) != 0)
      {
        closures.Add(nextEventId++, new Closure { OwnerObject = obj, Run = func });
      }
    }

    void IGameManager.ClosureDelayCommand(uint obj, float duration, ActionDelegate func)
    {
      if (VM.ClosureDelayCommand(obj, duration, nextEventId) != 0)
      {
        closures.Add(nextEventId++, new Closure { OwnerObject = obj, Run = func });
      }
    }

    void IGameManager.ClosureActionDoCommand(uint obj, ActionDelegate func)
    {
      if (VM.ClosureActionDoCommand(obj, nextEventId) != 0)
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