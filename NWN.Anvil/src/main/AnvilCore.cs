using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Internal;
using Anvil.Services;
using LightInject;
using NLog;
using NWN.Core;
using NWN.Native.API;

namespace Anvil
{
  /// <summary>
  /// Handles bootstrap and interop between %NWN, %NWN.Core and the %Anvil %API. The entry point of the implementing module should point to this class.<br/>
  /// Until <see cref="Init(IntPtr, int, IContainerFactory)"/> is called, all APIs are unavailable for usage.
  /// </summary>
  public sealed class AnvilCore
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static AnvilCore instance;

    [Inject]
    private IReadOnlyList<ICoreService> CoreServices { get; init; }

    [Inject]
    private NwServer NwServer { get; init; }

    [Inject]
    private VirtualMachineFunctionHandler VirtualMachineFunctionHandler { get; init; }

    private readonly IContainerFactory containerFactory;

    private readonly ServiceContainer coreServiceContainer;
    private List<ILateDisposable> lateDisposableServices;
    private ServiceContainer serviceContainer;

    private AnvilCore(IContainerFactory containerFactory)
    {
      this.containerFactory = containerFactory;
      Log.Info("Using {ContainerFactory} to install service bindings", containerFactory.GetType().FullName);

      coreServiceContainer = containerFactory.BuildCoreContainer(this);
      coreServiceContainer.InjectProperties(this);
    }

    public static T GetService<T>()
    {
      return instance.serviceContainer.GetInstance<T>();
    }

    /// <summary>
    /// Entrypoint to start Anvil.
    /// </summary>
    /// <param name="arg">The NativeHandles pointer, provided by the NWNX bootstrap entry point.</param>
    /// <param name="argLength">The size of the NativeHandles bootstrap structure, provided by the NWNX entry point.</param>
    /// <param name="containerFactory">An optional container factory to use instead of the default <see cref="AnvilContainerFactory"/>.</param>
    /// <returns>The init result code to return back to NWNX.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Init(IntPtr arg, int argLength, IContainerFactory containerFactory = default)
    {
      containerFactory ??= new AnvilContainerFactory();
      instance = new AnvilCore(containerFactory);

      NWNCore.NativeEventHandles eventHandles = new NWNCore.NativeEventHandles
      {
        Signal = instance.OnNWNXSignal,
        RunScript = instance.VirtualMachineFunctionHandler.OnRunScript,
        Closure = instance.VirtualMachineFunctionHandler.OnClosure,
      };

      return NWNCore.Init(arg, argLength, instance.VirtualMachineFunctionHandler, eventHandles);
    }

    /// <summary>
    /// Initiates a complete reload of plugins and Anvil services.<br/>
    /// This will reload all plugins.
    /// </summary>
    public static void Reload()
    {
      if (!EnvironmentConfig.ReloadEnabled)
      {
        Log.Error("Hot Reload of plugins is not enabled (NWM_RELOAD_ENABLED=true)");
      }

      GetService<SchedulerService>()?.Schedule(() =>
      {
        Log.Info("Reloading Anvil");
        instance.Unload();
        instance.Load();
      }, TimeSpan.Zero);
    }

    private void CheckServerVersion()
    {
      AssemblyName assemblyName = Assemblies.Anvil.GetName();
      Version serverVersion = NwServer.ServerVersion;

      if (assemblyName.Version?.Major != serverVersion.Major || assemblyName.Version.Minor != serverVersion.Minor)
      {
        Log.Warn("The current version of {Name} targets version {TargetVersion}, but the server is running {ServerVersion}! You may encounter compatibility issues",
          assemblyName.Name,
          assemblyName.Version,
          serverVersion);
      }
    }

    private void Init()
    {
      foreach (ICoreService coreService in CoreServices)
      {
        Log.Info("Initialising core service: {CoreService}", coreService.GetType().FullName);
        coreService.Init();
      }

      try
      {
        PrelinkNative();
      }
      catch (Exception e)
      {
        Log.Fatal(e, "Failed to load {Name:l} {Version:l} (NWN.Core: {CoreVersion}, NWN.Native: {NativeVersion})",
          Assemblies.Anvil.GetName().Name,
          AssemblyInfo.VersionInfo.InformationalVersion,
          Assemblies.Core.GetName().Version,
          Assemblies.Native.GetName().Version);
        throw;
      }

      Log.Info("Loading {Name:l} {Version:l} (NWN.Core: {CoreVersion}, NWN.Native: {NativeVersion})",
        Assemblies.Anvil.GetName().Name,
        AssemblyInfo.VersionInfo.InformationalVersion,
        Assemblies.Core.GetName().Version,
        Assemblies.Native.GetName().Version);

      CheckServerVersion();
    }

    private void Load()
    {
      foreach (ICoreService coreService in CoreServices)
      {
        coreService.Load();
      }

      serviceContainer = containerFactory.BuildAnvilServiceContainer(coreServiceContainer);
      foreach (IInitializable service in serviceContainer.GetAllInstances<IInitializable>().OrderBy(service => service.GetType().GetServicePriority()))
      {
        service.Init();
      }
    }

    private void OnNWNXSignal(string signal)
    {
      switch (signal)
      {
        case "ON_NWNX_LOADED":
          Init();
          break;
        case "ON_MODULE_LOAD_FINISH":
          Load();
          break;
        case "ON_DESTROY_SERVER":
          Log.Info("Server is shutting down...");
          Unload();
          break;
        case "ON_DESTROY_SERVER_AFTER":
          Shutdown();
          break;
      }
    }

    private void PrelinkNative()
    {
      if (!EnvironmentConfig.NativePrelinkEnabled)
      {
        Log.Warn("Marshaller prelinking is disabled (ANVIL_PRELINK_ENABLED=false). You may encounter random crashes or issues");
        return;
      }

      Log.Info("Prelinking native methods");

      try
      {
        Marshal.PrelinkAll(typeof(NWNXLibPINVOKE));
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

    private void Shutdown()
    {
      if (lateDisposableServices == null)
      {
        return;
      }

      foreach (ILateDisposable lateDisposable in lateDisposableServices)
      {
        lateDisposable.LateDispose();
      }

      foreach (ICoreService coreService in CoreServices)
      {
        coreService.Shutdown();
      }

      lateDisposableServices = null;
    }

    private void Unload()
    {
      if (serviceContainer == null)
      {
        return;
      }

      Log.Info("Unloading services...");
      lateDisposableServices = serviceContainer.GetAllInstances<ILateDisposable>().ToList();

      serviceContainer.Dispose();
      serviceContainer = null;

      foreach (ICoreService coreService in CoreServices)
      {
        coreService.Unload();
      }
    }
  }
}
