using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NLog;
using NWN.API;
using NWN.Core;
using NWN.Core.NWNX;
using NWN.Plugins;
using NWN.Services;
using NWNX;

namespace NWN
{
  /// <summary>
  /// Handles bootstrap and interop between %NWN, %NWN.Core and the managed %API. The entry point of the implementing module should point to this class.<br/>
  /// Until <see cref="Init(IntPtr, int, IBindingInstaller, ITypeLoader)"/> is called, all APIs are unavailable for usage.
  /// </summary>
  public class NManager : IGameManager
  {
    private const uint ObjectInvalid = 0x7F000000;

    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private static NManager instance;

    private readonly Stack<uint> scriptContexts = new Stack<uint>();
    private readonly Dictionary<ulong, ActionDelegate> closures = new Dictionary<ulong, ActionDelegate>();

    // Core Services
    private readonly IBindingInstaller bindingInstaller;
    private readonly ITypeLoader typeLoader;
    private readonly LoggerManager loggerManager;

    private ServiceManager serviceManager;

    private uint objectSelf;
    private ulong nextEventId;

    // Native callbacks
    private ICoreRunScriptHandler runScriptHandler;
    private ICoreLoopHandler loopHandler;

    uint IGameManager.ObjectSelf
    {
      get => objectSelf;
    }

    /// <summary>
    /// Initialises the managed library, loading all defined services.
    /// </summary>
    /// <param name="arg">The NativeHandles pointer, provided by the NWNX bootstrap entry point.</param>
    /// <param name="argLength">The size of the NativeHandles bootstrap structure, provided by the NWNX entry point.</param>
    /// <param name="bindingInstaller">An optional custom binding installer to use instead of the default <see cref="ServiceInstaller"/>.</param>
    /// <param name="typeLoader">An optional type loader to use instead of the default <see cref="PluginLoader"/>.</param>
    /// <returns>The init result code to return back to NWNX.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Init(IntPtr arg, int argLength, IBindingInstaller bindingInstaller = default, ITypeLoader typeLoader = default)
    {
      typeLoader ??= new PluginLoader();
      bindingInstaller ??= new ServiceInstaller();

      instance = new NManager(bindingInstaller, typeLoader);
      return NWNCore.Init(arg, argLength, instance);
    }

    public static T GetService<T>() where T : class
    {
      return instance?.serviceManager.GetService<T>();
    }

    /// <summary>
    /// Initiates a complete reload of the managed stack.<br/>
    /// This will reload all plugins.
    /// </summary>
    public static async void Reload()
    {
      if (!EnvironmentConfig.ReloadEnabled)
      {
        Log.Error("Managed reload is not enabled (NWM_RELOAD_ENABLED=true)");
        return;
      }

      await NwTask.NextFrame();

      Log.Info("Reloading NWN.Managed.");

      instance.Dispose();

      GC.Collect();
      GC.WaitForPendingFinalizers();

      instance.Init();
    }

    private NManager(IBindingInstaller bindingInstaller, ITypeLoader typeLoader)
    {
      this.bindingInstaller = bindingInstaller;
      this.typeLoader = typeLoader;
      this.loggerManager = new LoggerManager();
    }

    void IGameManager.OnSignal(string signal)
    {
      switch (signal)
      {
        case "ON_MODULE_LOAD_FINISH":
          Init();
          break;
        case "ON_DESTROY_SERVER":
          Shutdown();
          break;
        default:
          Log.Debug($"Unhandled Signal: \"{signal}\"");
          break;
      }
    }

    private void Init()
    {
      loggerManager.Init();
      Log.Info($"Loading NWN.Managed - {AssemblyConstants.ManagedAssemblyName.Version}");
      CheckPluginDependencies();

      typeLoader.Init();
      serviceManager = new ServiceManager(typeLoader, bindingInstaller);
      serviceManager.Init();

      runScriptHandler = GetService<ICoreRunScriptHandler>();
      loopHandler = GetService<ICoreLoopHandler>();
    }

    private void Dispose()
    {
      serviceManager?.Dispose();
      serviceManager = null;

      runScriptHandler = null;
      loopHandler = null;

      typeLoader.Dispose();
    }

    private void CheckPluginDependencies()
    {
      Log.Info("Checking Plugin Dependencies");
      PluginUtils.AssertPluginExists<UtilPlugin>();
      PluginUtils.AssertPluginExists<ObjectPlugin>();
    }

    private void Shutdown()
    {
      Dispose();
      loggerManager.Dispose();
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
      objectSelf = oidSelf;
      scriptContexts.Push(oidSelf);

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
      objectSelf = scriptContexts.Count == 0 ? ObjectInvalid : scriptContexts.Peek();
      return retVal;
    }

    void IGameManager.OnClosure(ulong eid, uint oidSelf)
    {
      uint old = objectSelf;
      objectSelf = oidSelf;

      try
      {
        closures[eid].Invoke();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }

      closures.Remove(eid);
      objectSelf = old;
    }

    void IGameManager.ClosureAssignCommand(uint obj, ActionDelegate func)
    {
      if (VM.ClosureAssignCommand(obj, nextEventId) != 0)
      {
        closures.Add(nextEventId++, func);
      }
    }

    void IGameManager.ClosureDelayCommand(uint obj, float duration, ActionDelegate func)
    {
      if (VM.ClosureDelayCommand(obj, duration, nextEventId) != 0)
      {
        closures.Add(nextEventId++, func);
      }
    }

    void IGameManager.ClosureActionDoCommand(uint obj, ActionDelegate func)
    {
      if (VM.ClosureActionDoCommand(obj, nextEventId) != 0)
      {
        closures.Add(nextEventId++, func);
      }
    }
  }
}
