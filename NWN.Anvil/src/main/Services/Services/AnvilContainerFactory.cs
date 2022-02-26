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
  /// <summary>
  /// Wires up and prepares service classes for dependency injection and initialization.
  /// </summary>
  public class AnvilContainerFactory : IContainerFactory
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly ContainerOptions containerOptions = new ContainerOptions
    {
      EnablePropertyInjection = true,
      EnableVariance = false,
      DefaultServiceSelector = SelectHighestPriorityService,
      LogFactory = CreateLogHandler,
    };

    public ServiceContainer BuildCoreContainer(AnvilCore anvilCore)
    {
      ServiceContainer serviceContainer = CreateContainer();
      serviceContainer.RegisterInstance(anvilCore);

      RegisterCoreService<NwServer>(serviceContainer);
      RegisterCoreService<LoggerManager>(serviceContainer);
      RegisterCoreService<UnhandledExceptionLogger>(serviceContainer);
      RegisterCoreService<ModuleLoadTracker>(serviceContainer);
      RegisterCoreService<VirtualMachineFunctionHandler>(serviceContainer);
      RegisterCoreService<HookService>(serviceContainer);
      RegisterCoreService<VirtualMachine>(serviceContainer);
      RegisterCoreService<PluginManager>(serviceContainer);

      return serviceContainer;
    }

    public ServiceContainer BuildAnvilServiceContainer(ServiceContainer coreContainer)
    {
      ServiceContainer serviceContainer = CreateContainer();
      serviceContainer.RegisterInstance((IServiceContainer)serviceContainer);

      // Expose all core services as concrete types.
      foreach (ICoreService coreService in coreContainer.GetAllInstances<ICoreService>())
      {
        serviceContainer.RegisterInstance(coreService.GetType(), coreService);
      }

      PluginManager pluginManager = coreContainer.GetInstance<PluginManager>();

      Log.Info("Loading services...");
      foreach (Type type in pluginManager.LoadedTypes)
      {
        TryRegisterType(pluginManager, serviceContainer, type);
      }

      RegisterOverrides(pluginManager, serviceContainer);
      return serviceContainer;
    }

    private ServiceContainer CreateContainer()
    {
      ServiceContainer serviceContainer = new ServiceContainer(containerOptions);
      SetupInjectPropertySelector(serviceContainer);

      return serviceContainer;
    }

    /// <summary>
    /// Override in a child class to specify additional bindings/overrides.<br/>
    /// See https://www.lightinject.net/ for documentation.
    /// </summary>
    // ReSharper disable UnusedParameter.Global
    protected virtual void RegisterOverrides(PluginManager pluginManager, ServiceContainer serviceContainer) {}
    // ReSharper restore UnusedParameter.Global

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

    private static string GetServiceName(Type implementation)
    {
      int bindingPriority = implementation.GetServicePriority() - (int)InternalBindingPriority.Highest;
      return bindingPriority.ToString("D6") + implementation.FullName;
    }

    private static bool IsServiceRequirementsMet(PluginManager pluginManager, ServiceBindingOptionsAttribute options)
    {
      return options?.PluginDependencies == null || options.PluginDependencies.All(pluginManager.IsPluginLoaded);
    }

    private static void RegisterBindings(ServiceContainer serviceContainer, Type bindToType, IEnumerable<Type> bindFromTypes, ServiceBindingOptionsAttribute options)
    {
      string serviceName = GetServiceName(bindToType);

      PerContainerLifetime lifeTime = new PerContainerLifetime();
      RegisterExplicitBindings(serviceContainer, bindToType, bindFromTypes, serviceName, lifeTime);

      if (options is not { Lazy: true })
      {
        RegisterImplicitBindings(serviceContainer, bindToType, serviceName, lifeTime);
      }

      Log.Info("Registered service {Service}", bindToType.FullName);
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

    private static void RegisterCoreService<T>(ServiceContainer serviceContainer) where T : ICoreService
    {
      Type bindToType = typeof(T);
      if (bindToType.IsAbstract || bindToType.ContainsGenericParameters)
      {
        return;
      }

      ServiceBindingOptionsAttribute options = bindToType.GetCustomAttribute<ServiceBindingOptionsAttribute>();
      RegisterBindings(serviceContainer, bindToType, new[] { bindToType, typeof(ICoreService) }, options);
    }

    private static void TryRegisterType(PluginManager pluginManager, ServiceContainer serviceContainer, Type bindToType)
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
        RegisterBindings(serviceContainer, bindToType, bindings.Select(attribute => attribute.BindFrom), options);
      }
    }
  }
}
