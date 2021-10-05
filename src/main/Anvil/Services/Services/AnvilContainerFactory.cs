using System;
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

    protected PluginManager PluginManager;
    protected ServiceContainer ServiceContainer;

    public ServiceContainer Setup(PluginManager pluginManager)
    {
      PluginManager = pluginManager;
      ServiceContainer = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = true, EnableVariance = false });
      SetupInjectPropertySelector();

      ServiceContainer.RegisterInstance((IServiceContainer)ServiceContainer);
      return ServiceContainer;
    }

    public void RegisterCoreService<T>(T instance)
    {
      Type instanceType = instance.GetType();
      ServiceBindingOptionsAttribute options = instanceType.GetCustomAttribute<ServiceBindingOptionsAttribute>();

      string serviceName = GetServiceName(instanceType, options);
      ServiceContainer.RegisterInstance(instance, serviceName);
    }

    public void BuildContainer()
    {
      Log.Info("Loading services...");
      foreach (Type type in PluginManager.LoadedTypes)
      {
        TryRegisterType(type);
      }

      RegisterOverrides();
    }

    /// <summary>
    /// Override in a child class to specify additional bindings/overrides.<br/>
    /// See https://www.lightinject.net/ for documentation.
    /// </summary>
    protected virtual void RegisterOverrides() {}

    private void TryRegisterType(Type type)
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
      if (IsServiceRequirementsMet(options))
      {
        RegisterBindings(type, bindings, options);
      }
    }

    private void RegisterBindings(Type bindTo, ServiceBindingAttribute[] bindings, ServiceBindingOptionsAttribute options)
    {
      string serviceName = GetServiceName(bindTo, options);

      PerContainerLifetime lifeTime = new PerContainerLifetime();
      RegisterExplicitBindings(bindTo, bindings, serviceName, lifeTime);

      if (options is not { Lazy: true })
      {
        RegisterImplicitBindings(bindTo, serviceName, lifeTime);
      }

      Log.Info("Registered service {Service}", bindTo.FullName);
    }

    private bool IsServiceRequirementsMet(ServiceBindingOptionsAttribute options)
    {
      if (options == null || options.PluginDependencies == null && options.MissingPluginDependencies == null)
      {
        return true;
      }

      if (options.PluginDependencies != null && options.PluginDependencies.Any(dependency => !PluginManager.IsPluginLoaded(dependency)))
      {
        return false;
      }

      if (options.MissingPluginDependencies != null && options.MissingPluginDependencies.Any(dependency => PluginManager.IsPluginLoaded(dependency)))
      {
        return false;
      }

      return true;
    }

    private void RegisterImplicitBindings(Type bindTo, string serviceName, ILifetime lifeTime)
    {
      ServiceContainer.Register(typeof(object), bindTo, serviceName, lifeTime);

      if (bindTo.IsAssignableTo(typeof(IInitializable)))
      {
        ServiceContainer.Register(typeof(IInitializable), bindTo, serviceName, lifeTime);
      }

      if (bindTo.IsAssignableTo(typeof(ILateDisposable)))
      {
        ServiceContainer.Register(typeof(ILateDisposable), bindTo, serviceName, lifeTime);
      }
    }

    private void RegisterExplicitBindings(Type bindTo, ServiceBindingAttribute[] newBindings, string serviceName, ILifetime lifeTime)
    {
      foreach (ServiceBindingAttribute bindingInfo in newBindings)
      {
        ServiceContainer.Register(bindingInfo.BindFrom, bindTo, serviceName, lifeTime);
        Log.Debug("Bind {BindFrom} -> {BindTo}", bindingInfo.BindFrom.FullName, bindTo.FullName);
      }
    }

    private void SetupInjectPropertySelector()
    {
      InjectPropertySelector propertySelector = new InjectPropertySelector(InjectPropertyTypes.InstanceOnly);
      ServiceContainer.PropertyDependencySelector = new InjectPropertyDependencySelector(propertySelector);
    }

    private string GetServiceName(Type implementation, ServiceBindingOptionsAttribute options)
    {
      short bindingOrder = options?.Order ?? (short)BindingOrder.Default;
      return bindingOrder.ToString("D5") + implementation.FullName;
    }
  }
}
