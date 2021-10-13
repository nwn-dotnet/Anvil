using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Anvil.API;
using Anvil.Plugins;
using LightInject;
using NLog;

namespace Anvil.Services
{
  /// <summary>
  /// Wires up and prepares service classes for dependency injection and initialization.
  /// </summary>
  public class AnvilContainerFactory : IContainerFactory
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public ServiceContainer CreateContainer(PluginManager pluginManager, IEnumerable<object> coreServices)
    {
      ServiceContainer serviceContainer = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = true, EnableVariance = false });

      SetupInjectPropertySelector(serviceContainer);
      serviceContainer.RegisterInstance((IServiceContainer)serviceContainer);

      foreach (object coreService in coreServices)
      {
        RegisterCoreService(serviceContainer, coreService);
      }

      BuildContainer(pluginManager, serviceContainer);
      return serviceContainer;
    }

    private static void RegisterCoreService(ServiceContainer serviceContainer, object instance)
    {
      Type instanceType = instance.GetType();
      ServiceBindingOptionsAttribute options = instanceType.GetCustomAttribute<ServiceBindingOptionsAttribute>();

      string serviceName = GetServiceName(instanceType, options);
      serviceContainer.RegisterInstance(instanceType, instance, serviceName);
    }

    private void BuildContainer(PluginManager pluginManager, ServiceContainer serviceContainer)
    {
      Log.Info("Loading services...");
      foreach (Type type in pluginManager.LoadedTypes)
      {
        TryRegisterType(pluginManager, serviceContainer, type);
      }

      RegisterOverrides(pluginManager, serviceContainer);
    }

    /// <summary>
    /// Override in a child class to specify additional bindings/overrides.<br/>
    /// See https://www.lightinject.net/ for documentation.
    /// </summary>
    // ReSharper disable UnusedParameter.Global
    protected virtual void RegisterOverrides(PluginManager pluginManager, ServiceContainer serviceContainer) {}
    // ReSharper restore UnusedParameter.Global

    private static void TryRegisterType(PluginManager pluginManager, ServiceContainer serviceContainer, Type type)
    {
      if (!type.IsClass || type.IsAbstract || type.ContainsGenericParameters)
      {
        return;
      }

      ServiceBindingAttribute[] bindings = type.GetCustomAttributes<ServiceBindingAttribute>();
      if (bindings.Length == 0)
      {
        return;
      }

      ServiceBindingOptionsAttribute options = type.GetCustomAttribute<ServiceBindingOptionsAttribute>();
      if (IsServiceRequirementsMet(pluginManager, options))
      {
        RegisterBindings(serviceContainer, type, bindings, options);
      }
    }

    private static void RegisterBindings(ServiceContainer serviceContainer, Type bindTo, ServiceBindingAttribute[] bindings, ServiceBindingOptionsAttribute options)
    {
      string serviceName = GetServiceName(bindTo, options);

      PerContainerLifetime lifeTime = new PerContainerLifetime();
      RegisterExplicitBindings(serviceContainer, bindTo, bindings, serviceName, lifeTime);

      if (options is not { Lazy: true })
      {
        RegisterImplicitBindings(serviceContainer, bindTo, serviceName, lifeTime);
      }

      Log.Info("Registered service {Service}", bindTo.FullName);
    }

    private static bool IsServiceRequirementsMet(PluginManager pluginManager, ServiceBindingOptionsAttribute options)
    {
      if (options == null || options.PluginDependencies == null && options.MissingPluginDependencies == null)
      {
        return true;
      }

      if (options.PluginDependencies != null && options.PluginDependencies.Any(dependency => !pluginManager.IsPluginLoaded(dependency)))
      {
        return false;
      }

      if (options.MissingPluginDependencies != null && options.MissingPluginDependencies.Any(pluginManager.IsPluginLoaded))
      {
        return false;
      }

      return true;
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

    private static void RegisterExplicitBindings(ServiceContainer serviceContainer, Type bindTo, ServiceBindingAttribute[] newBindings, string serviceName, ILifetime lifeTime)
    {
      foreach (ServiceBindingAttribute bindingInfo in newBindings)
      {
        serviceContainer.Register(bindingInfo.BindFrom, bindTo, serviceName, lifeTime);
        Log.Debug("Bind {BindFrom} -> {BindTo}", bindingInfo.BindFrom.FullName, bindTo.FullName);
      }
    }

    private static void SetupInjectPropertySelector(ServiceContainer serviceContainer)
    {
      InjectPropertySelector propertySelector = new InjectPropertySelector(InjectPropertyTypes.InstanceOnly);
      serviceContainer.PropertyDependencySelector = new InjectPropertyDependencySelector(propertySelector);
    }

    private static string GetServiceName(Type implementation, ServiceBindingOptionsAttribute options)
    {
      short bindingOrder = options?.Order ?? (short)BindingOrder.Default;
      return bindingOrder.ToString("D5") + implementation.FullName;
    }
  }
}
