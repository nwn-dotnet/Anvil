using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Anvil.API;
using Anvil.Plugins;
using LightInject;
using NLog;
using LogLevel = LightInject.LogLevel;

namespace Anvil.Services
{
  internal sealed class AnvilServiceManager : IServiceManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly ContainerOptions containerOptions = new ContainerOptions
    {
      EnablePropertyInjection = true,
      EnableCurrentScope = false,
      EnableVariance = false,
      DefaultServiceSelector = SelectHighestPriorityService,
      LogFactory = CreateLogHandler,
    };

    public ServiceContainer CoreServiceContainer { get; }

    public ServiceContainer AnvilServiceContainer { get; private set; }

    private PluginManager pluginManager;
    private List<ICoreService> coreServices;
    private List<ILateDisposable> lateDisposableServices;

    public AnvilServiceManager()
    {
      CoreServiceContainer = CreateContainer();
      AnvilServiceContainer = CreateContainer();

      InstallCoreContainer();
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
    }

    void IServiceManager.Start()
    {
      foreach (ICoreService coreService in coreServices)
      {
        coreService.Start();
      }

      foreach (IInitializable service in AnvilServiceContainer.GetAllInstances<IInitializable>().OrderBy(service => service.GetType().GetServicePriority()))
      {
        service.Init();
      }
    }

    void IServiceManager.Unload()
    {
      Log.Info("Unloading anvil services...");
      lateDisposableServices = AnvilServiceContainer.GetAllInstances<ILateDisposable>().ToList();

      AnvilServiceContainer.Dispose();
      AnvilServiceContainer = null;

      Log.Info("Unloading core services...");
      for (int i = coreServices.Count - 1; i >= 0; i--)
      {
        ICoreService coreService = coreServices[i];

        Log.Info("Unloading core service {CoreService}", coreService.GetType().FullName);
        coreService.Unload();
      }

      AnvilServiceContainer = CreateContainer();
    }

    void IServiceManager.Shutdown()
    {
      if (lateDisposableServices == null)
      {
        return;
      }

      foreach (ILateDisposable lateDisposable in lateDisposableServices)
      {
        lateDisposable.LateDispose();
      }

      for (int i = coreServices.Count - 1; i >= 0; i--)
      {
        coreServices[i].Shutdown();
      }

      lateDisposableServices = null;
    }

    private ServiceContainer CreateContainer()
    {
      ServiceContainer serviceContainer = new ServiceContainer(containerOptions);
      SetupInjectPropertySelector(serviceContainer);

      serviceContainer.RegisterInstance(typeof(IServiceManager), this);
      return serviceContainer;
    }

    private void InstallAnvilServiceContainer()
    {
      // Expose all core services as concrete types.
      foreach (ICoreService coreService in CoreServiceContainer.GetAllInstances<ICoreService>())
      {
        AnvilServiceContainer.RegisterInstance(coreService.GetType(), coreService);
      }

      foreach (Type type in pluginManager.LoadedTypes)
      {
        TryRegisterAnvilService(type);
      }
    }

    private void InstallCoreContainer()
    {
      RegisterCoreService<NwServer>();
      RegisterCoreService<LoggerManager>();
      RegisterCoreService<UnhandledExceptionLogger>();
      RegisterCoreService<InjectionService>();
      RegisterCoreService<ModuleLoadTracker>();
      RegisterCoreService<VirtualMachineFunctionHandler>();
      RegisterCoreService<HookService>();
      RegisterCoreService<VirtualMachine>();
      RegisterCoreService<PluginManager>();
    }

    private static bool IsServiceRequirementsMet(PluginManager pluginManager, ServiceBindingOptionsAttribute options)
    {
      return options?.PluginDependencies == null || options.PluginDependencies.All(pluginManager.IsPluginLoaded);
    }

    private static void RegisterBindings(ServiceContainer serviceContainer, Type bindToType, IEnumerable<Type> bindFromTypes, ServiceBindingOptionsAttribute options)
    {
      string serviceName = bindToType.GetInternalServiceName();

      PerContainerLifetime lifeTime = new PerContainerLifetime();
      RegisterExplicitBindings(serviceContainer, bindToType, bindFromTypes, serviceName, lifeTime);

      if (options is not { Lazy: true })
      {
        RegisterImplicitBindings(serviceContainer, bindToType, serviceName, lifeTime);
      }

      Log.Info("Registered service {Service}", bindToType.FullName);
    }

    private void RegisterCoreService<T>() where T : ICoreService
    {
      Type bindToType = typeof(T);
      if (bindToType.IsAbstract || bindToType.ContainsGenericParameters)
      {
        return;
      }

      ServiceBindingOptionsAttribute options = bindToType.GetCustomAttribute<ServiceBindingOptionsAttribute>();
      RegisterBindings(CoreServiceContainer, bindToType, new[] { bindToType, typeof(ICoreService) }, options);
    }

    private static void RegisterExplicitBindings(ServiceContainer serviceContainer, Type bindToType, IEnumerable<Type> bindFromTypes, string serviceName, ILifetime lifeTime)
    {
      foreach (Type binding in bindFromTypes)
      {
        serviceContainer.Register(binding, bindToType, serviceName, lifeTime);
        Log.Debug("Bind {BindFrom} -> {BindTo}", binding.FullName, bindToType.FullName);
      }
    }

    private static void RegisterImplicitBindings(ServiceContainer serviceContainer, Type bindTo, string serviceName, ILifetime lifeTime)
    {
      serviceContainer.Register(typeof(object), bindTo, serviceName, lifeTime);

      if (bindTo.IsAssignableTo(typeof(IInitializable)))
      {
        serviceContainer.Register(typeof(IInitializable), bindTo, serviceName, lifeTime);
      }

      if (bindTo.IsAssignableTo(typeof(ILateDisposable)))
      {
        serviceContainer.Register(typeof(ILateDisposable), bindTo, serviceName, lifeTime);
      }
    }

    private void TryRegisterAnvilService(Type bindToType)
    {
      if (!bindToType.IsClass || bindToType.IsAbstract || bindToType.ContainsGenericParameters)
      {
        return;
      }

      ServiceBindingAttribute[] bindings = bindToType.GetCustomAttributes<ServiceBindingAttribute>();
      if (bindings.Length == 0)
      {
        return;
      }

      ServiceBindingOptionsAttribute options = bindToType.GetCustomAttribute<ServiceBindingOptionsAttribute>();
      if (IsServiceRequirementsMet(pluginManager, options))
      {
        RegisterBindings(AnvilServiceContainer, bindToType, bindings.Select(attribute => attribute.BindFrom), options);
      }
    }

    private static string SelectHighestPriorityService(string[] services)
    {
      // Services are sorted in priority order.
      // So we just return the first service.
      return services[0];
    }

    private static void SetupInjectPropertySelector(ServiceContainer serviceContainer)
    {
      InjectPropertySelector propertySelector = new InjectPropertySelector(InjectPropertyTypes.InstanceOnly);
      serviceContainer.PropertyDependencySelector = new InjectPropertyDependencySelector(propertySelector);
    }

    private static Action<LogEntry> CreateLogHandler(Type type)
    {
      Logger logger = LogManager.GetLogger(type.FullName);
      return entry =>
      {
        switch (entry.Level)
        {
          case LogLevel.Info:
            logger.Debug(entry.Message);
            break;
          case LogLevel.Warning:
            logger.Warn(entry.Message);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      };
    }
  }
}
