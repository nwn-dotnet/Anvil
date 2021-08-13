using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Internal;
using Anvil.Plugins;
using Anvil.Services;
using NLog;
using NWN.Core;

namespace Anvil
{
  /// <summary>
  /// Handles bootstrap and interop between %NWN, %NWN.Core and the %Anvil %API. The entry point of the implementing module should point to this class.<br/>
  /// Until <see cref="Init(IntPtr, int, IContainerFactory, ITypeLoader)"/> is called, all APIs are unavailable for usage.
  /// </summary>
  public sealed class AnvilCore : ICoreSignalHandler
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static AnvilCore instance;

    // Core Services
    private CoreInteropHandler interopHandler;
    private IContainerFactory containerFactory;
    private ITypeLoader typeLoader;
    private LoggerManager loggerManager;
    private ServiceManager serviceManager;

    /// <summary>
    /// Entrypoint to start Anvil.
    /// </summary>
    /// <param name="arg">The NativeHandles pointer, provided by the NWNX bootstrap entry point.</param>
    /// <param name="argLength">The size of the NativeHandles bootstrap structure, provided by the NWNX entry point.</param>
    /// <param name="containerFactory">An optional custom binding installer to use instead of the default <see cref="AnvilContainerFactory"/>.</param>
    /// <param name="typeLoader">An optional type loader to use instead of the default <see cref="PluginLoader"/>.</param>
    /// <returns>The init result code to return back to NWNX.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Init(IntPtr arg, int argLength, IContainerFactory containerFactory = default, ITypeLoader typeLoader = default)
    {
      typeLoader ??= new PluginLoader();
      containerFactory ??= new AnvilContainerFactory();

      instance = new AnvilCore();
      instance.interopHandler = new CoreInteropHandler(instance);
      instance.containerFactory = containerFactory;
      instance.typeLoader = typeLoader;
      instance.loggerManager = new LoggerManager();

      return NWNCore.Init(arg, argLength, instance.interopHandler, instance.interopHandler);
    }

    /// <summary>
    /// Initiates a complete reload of plugins and Anvil services.<br/>
    /// This will reload all plugins.
    /// </summary>
    public static async void Reload()
    {
      if (!EnvironmentConfig.ReloadEnabled)
      {
        Log.Error("Hot Reload of plugins is not enabled (NWM_RELOAD_ENABLED=true)");
        return;
      }

      await NwTask.NextFrame();

      Log.Info("Reloading Anvil");

      instance.Shutdown(true);

      GC.Collect();
      GC.WaitForPendingFinalizers();

      instance.typeLoader.Init();
      instance.Start();
    }

    private AnvilCore() {}

    void ICoreSignalHandler.OnStart()
    {
      Init();
      Start();
    }

    void ICoreSignalHandler.OnShutdown()
    {
      Shutdown();
    }

    private void Init()
    {
      loggerManager.Init();
      PrelinkNative();
      loggerManager.InitVariables();

      AssemblyName assemblyName = Assemblies.Anvil.GetName();

      Log.Info("Loading {Name} {Version} (NWN.Core: {CoreVersion}, NWN.Native: {NativeVersion})",
        assemblyName.Name,
        Assemblies.Anvil.GetName().Version,
        Assemblies.Core.GetName().Version,
        Assemblies.Native.GetName().Version);

      Log.Info(".NET runtime is {NetVersion}, running on {OS}, installed at {NetInstallDir}",
        RuntimeInformation.FrameworkDescription,
        RuntimeInformation.OSDescription,
        RuntimeEnvironment.GetRuntimeDirectory());

      Log.Info("Server is running Neverwinter Nights {Version}", NwServer.Instance.ServerVersion);

      CheckServerVersion();
      typeLoader.Init();
    }

    private void Start()
    {
      serviceManager = new ServiceManager(typeLoader, containerFactory);

      serviceManager.RegisterCoreService(typeLoader);
      serviceManager.RegisterCoreService(serviceManager);

      serviceManager.Init();
      interopHandler.Init(serviceManager.GetService<ICoreRunScriptHandler>(), serviceManager.GetService<ICoreLoopHandler>());
    }

    private void Shutdown(bool keepLoggerAlive = false)
    {
      serviceManager?.Dispose();
      serviceManager = null;
      typeLoader.Dispose();

      if (!keepLoggerAlive)
      {
        loggerManager.Dispose();
      }
    }

    private void CheckServerVersion()
    {
      AssemblyName assemblyName = Assemblies.Anvil.GetName();
      Version serverVersion = NwServer.Instance.ServerVersion;

      if (assemblyName.Version?.Major != serverVersion.Major || assemblyName.Version.Minor != serverVersion.Minor)
      {
        Log.Warn("The current version of {Name} targets version {TargetVersion}, but the server is running {ServerVersion}! You may encounter compatibility issues",
          assemblyName.Name,
          assemblyName.Version,
          serverVersion);
      }
    }

    private void PrelinkNative()
    {
      Log.Info("Prelinking native methods");

      try
      {
        Marshal.PrelinkAll(typeof(NWN.Native.API.NWNXLibPINVOKE));
        Log.Info("Prelinking complete");
      }
      catch (TypeInitializationException)
      {
        Log.Fatal("The NWNX_SWIG_DotNET plugin could not be found. Has it been enabled? (NWNX_SWIG_DOTNET_SKIP=n)");
        throw;
      }
      catch (Exception)
      {
        Log.Fatal("The NWNX_SWIG_DotNET plugin could not be loaded");
        throw;
      }
    }
  }
}
