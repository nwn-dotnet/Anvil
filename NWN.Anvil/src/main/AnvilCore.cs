using System;
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
  /// Until <see cref="Init(IntPtr, int, IServiceManager)"/> is called, all APIs are unavailable for usage.
  /// </summary>
  public sealed partial class AnvilCore
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static AnvilCore instance = null!;

    private readonly IServiceManager serviceManager;

    private AnvilCore(IServiceManager serviceManager)
    {
      Log.Info("Using {ServiceManager} to manage Anvil services", serviceManager.GetType().FullName);
      this.serviceManager = serviceManager;
      this.serviceManager.CoreServiceContainer.InjectProperties(this);
    }

    /// <summary>
    /// Gets the specified anvil service instance.
    /// </summary>
    /// <typeparam name="T">The service type to get.</typeparam>
    /// <returns>The associated anvil service instance.</returns>
    public static T? GetService<T>()
    {
      return instance.serviceManager.AnvilServiceContainer.GetInstance<T>();
    }

    /// <summary>
    /// Entrypoint to start Anvil.
    /// </summary>
    /// <param name="arg">The NativeHandles pointer, provided by the NWNX bootstrap entry point.</param>
    /// <param name="argLength">The size of the NativeHandles bootstrap structure, provided by the NWNX entry point.</param>
    /// <param name="serviceManager">A custom service manager to use instead of the default <see cref="AnvilServiceManager"/>. For advanced users only.</param>
    /// <returns>The init result code to return back to NWNX.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe int Init(IntPtr arg, int argLength, IServiceManager? serviceManager = default)
    {
      serviceManager ??= new AnvilServiceManager();
      instance = new AnvilCore(serviceManager);

      NWNCore.NativeEventHandlesUnmanaged eventHandles = new NWNCore.NativeEventHandlesUnmanaged
      {
        Signal = &OnNWNXSignal,
        RunScript = &OnRunScript,
        Closure = &OnClosure,
        MainLoop = &OnLoop,
        AssertFail = &OnAssertFail,
        CrashHandler = &OnServerCrash,
      };

      return NWNCore.Init(arg, argLength, instance, eventHandles);
    }

    /// <summary>
    /// Initiates a complete reload of plugins and Anvil services.<br/>
    /// This will reload all plugins.
    /// </summary>
    public static void Reload()
    {
      if (!EnvironmentConfig.ReloadEnabled)
      {
        Log.Error("Hot Reload of plugins is not enabled (ANVIL_RELOAD_ENABLED=true)");
      }

      GetService<SchedulerService>()?.Schedule(() =>
      {
        Log.Info("Reloading Anvil");
        instance.Unload();
        instance.LoadAndStart();
      }, TimeSpan.Zero);
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

    private void Init()
    {
      runtimeInfo = new RuntimeInfo
      {
        AssemblyName = Assemblies.Anvil.GetName().Name,
        AssemblyVersion = AssemblyInfo.VersionInfo.InformationalVersion,
        CoreVersion = Assemblies.Core.GetName().Version?.ToString(),
        NativeVersion = Assemblies.Native.GetName().Version?.ToString(),
      };

      serviceManager.Init();

      try
      {
        PrelinkNative();
      }
      catch (Exception e)
      {
        Log.Fatal(e, $"Failed to load {runtimeInfo.AssemblyName} {runtimeInfo.AssemblyVersion} (NWN.Core: {runtimeInfo.CoreVersion}, NWN.Native: {runtimeInfo.NativeVersion})");
        throw;
      }

      runtimeInfo.ServerVersion = NwServer.Instance.ServerVersion.ToString();

      Log.Info($"Loading {runtimeInfo.AssemblyName} {runtimeInfo.AssemblyVersion} (NWN.Core: {runtimeInfo.CoreVersion}, NWN.Native: {runtimeInfo.NativeVersion})");
      CheckServerVersion();
    }

    private void LoadAndStart()
    {
      serviceManager.Load();
      serviceManager.Start();
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
      serviceManager.Shutdown();
    }

    private void Unload()
    {
      serviceManager.Unload();
    }
  }
}
