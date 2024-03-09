using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Anvil.API;
using Anvil.Internal;
using Anvil.Plugins;
using LightInject;
using NLog;

namespace Anvil.Services
{
  internal sealed class AnvilServiceManager : IServiceManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private List<ICoreService> coreServices = null!;
    private List<ILateDisposable>? lateDisposableServices;

    private readonly AnvilContainerFactory anvilContainerFactory;
    private PluginManager pluginManager = null!;

    private bool shuttingDown;
    private bool anvilContainerAvailable;

    public ServiceContainer AnvilServiceContainer { get; private set; }

    public ServiceContainer CoreServiceContainer { get; }

    public AnvilServiceManager()
    {
      anvilContainerFactory = new AnvilContainerFactory(this);
      CoreServiceContainer = anvilContainerFactory.CreateContainer();
      AnvilServiceContainer = anvilContainerFactory.CreateContainer(CoreServiceContainer);

      InstallCoreContainer();
    }

    public void InjectProperties(object? instance)
    {
      if (anvilContainerAvailable)
      {
        AnvilServiceContainer.InjectProperties(instance);
      }
      else
      {
        CoreServiceContainer.InjectProperties(instance);
      }
    }

    IServiceContainer IServiceManager.CreateIsolatedPluginContainer(IEnumerable<Type> pluginTypes)
    {
      ServiceContainer pluginContainer = anvilContainerFactory.CreateContainer(AnvilServiceContainer);
      foreach (Type type in pluginTypes)
      {
        TryRegisterAnvilService(pluginContainer, type);
      }

      pluginContainer.ConstructAllServices();
      return pluginContainer;
    }

    void IServiceManager.Init()
    {
      pluginManager = CoreServiceContainer.GetInstance<PluginManager>();
      coreServices = CoreServiceContainer.GetAllInstances<ICoreService>().ToList();

      foreach (ICoreService coreService in coreServices)
      {
        Log.Info("Initialising core service {CoreService}", coreService.GetType().FullName);
        coreService.Init();
      }
    }

    void IServiceManager.Load()
    {
      Log.Info("Loading core services...");
      foreach (ICoreService coreService in coreServices)
      {
        Log.Info("Loading core service {CoreService}", coreService.GetType().FullName);
        coreService.Load();
      }

      Log.Info("Loading anvil services...");
      InstallAnvilServiceContainer();
      anvilContainerAvailable = true;
    }

    void IServiceManager.Shutdown()
    {
      // Prevent multiple invocations
      if (shuttingDown)
      {
        return;
      }

      shuttingDown = true;

      if (lateDisposableServices != null)
      {
        foreach (ILateDisposable lateDisposable in lateDisposableServices)
        {
          lateDisposable.LateDispose();
        }
      }

      for (int i = coreServices.Count - 1; i >= 0; i--)
      {
        coreServices[i].Shutdown();
      }

      lateDisposableServices = null;
    }

    void IServiceManager.Start()
    {
      foreach (ICoreService coreService in coreServices)
      {
        coreService.Start();
      }

      AnvilServiceContainer.ConstructAllServices();
    }

    void IServiceManager.Unload()
    {
      UnloadAnvilServices();
      UnloadCoreServices();
    }

    private static bool IsServiceRequirementsMet(PluginManager pluginManager, ServiceBindingOptionsAttribute? options)
    {
      return options?.PluginDependencies == null || options.PluginDependencies.All(plugin => pluginManager.GetPlugin(plugin)?.IsLoaded == true);
    }

    private void InstallAnvilServiceContainer()
    {
      foreach (Type type in Assemblies.AllTypes)
      {
        TryRegisterAnvilService(AnvilServiceContainer, type);
      }

      foreach (Plugin plugin in pluginManager.Plugins)
      {
        if (plugin.PluginTypes != null && !plugin.PluginInfo.Isolated)
        {
          foreach (Type type in plugin.PluginTypes)
          {
            TryRegisterAnvilService(AnvilServiceContainer, type);
          }
        }
      }
    }

    private void InstallCoreContainer()
    {
      CoreServiceContainer.RegisterCoreService<NwServer>();
      CoreServiceContainer.RegisterCoreService<LoggerManager>();
      CoreServiceContainer.RegisterCoreService<UnhandledExceptionLogger>();
      CoreServiceContainer.RegisterCoreService<UnobservedTaskExceptionLogger>();
      CoreServiceContainer.RegisterCoreService<InjectionService>();
      CoreServiceContainer.RegisterCoreService<ModuleLoadTracker>();
      CoreServiceContainer.RegisterCoreService<HookService>();
      CoreServiceContainer.RegisterCoreService<VirtualMachine>();
      CoreServiceContainer.RegisterCoreService<PluginManager>();
      CoreServiceContainer.RegisterCoreService<EncodingService>();
      CoreServiceContainer.RegisterCoreService<ResourceManager>();
    }

    private void TryRegisterAnvilService(IServiceContainer container, Type bindToType)
    {
      if (bindToType.IsAnvilService(out ServiceBindingAttribute[]? bindings, out ServiceBindingOptionsAttribute? options) && IsServiceRequirementsMet(pluginManager, options))
      {
        container.RegisterAnvilService(bindToType, bindings.Select(attribute => attribute.BindFrom), options);
      }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UnloadAnvilServices()
    {
      Log.Info("Unloading anvil services...");

      // This must always happen in a separate scope/method, otherwise in Debug and some Release configurations, AnvilServiceContainer will hold a strong reference and prevent plugin unload.
      lateDisposableServices = AnvilServiceContainer.GetAllInstances<ILateDisposable>().ToList();
      AnvilServiceContainer.Dispose();
      AnvilServiceContainer = anvilContainerFactory.CreateContainer();
    }

    private void UnloadCoreServices()
    {
      Log.Info("Unloading core services...");
      for (int i = coreServices.Count - 1; i >= 0; i--)
      {
        ICoreService coreService = coreServices[i];

        Log.Info("Unloading core service {CoreService}", coreService.GetType().FullName);
        coreService.Unload();
      }
    }
  }
}
