using System;
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

    protected ITypeLoader TypeLoader;
    protected ServiceContainer ServiceContainer;

    public ServiceContainer Setup(ITypeLoader typeLoader)
    {
      TypeLoader = typeLoader;
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
      SearchForBindings();
      RegisterOverrides();
    }

    private void SetupInjectPropertySelector()
    {
      InjectPropertySelector propertySelector = new InjectPropertySelector(InjectPropertyTypes.InstanceOnly);
      ServiceContainer.PropertyDependencySelector = new InjectPropertyDependencySelector(propertySelector);
    }

    private void SearchForBindings()
    {
      Log.Info("Loading services...");

      foreach (Type type in TypeLoader.LoadedTypes)
      {
        if (!type.IsClass || type.IsAbstract || type.ContainsGenericParameters)
        {
          continue;
        }

        RegisterBindings(type, type.GetCustomAttributes<ServiceBindingAttribute>());
      }
    }

    private void RegisterBindings(Type bindTo, ServiceBindingAttribute[] newBindings)
    {
      if (newBindings.Length == 0)
      {
        return;
      }

      ServiceBindingOptionsAttribute options = bindTo.GetCustomAttribute<ServiceBindingOptionsAttribute>();
      string serviceName = GetServiceName(bindTo, options);

      PerContainerLifetime lifeTime = new PerContainerLifetime();
      RegisterExplicitBindings(bindTo, newBindings, serviceName, lifeTime);

      if (options is not { Lazy: true })
      {
        RegisterImplicitBindings(bindTo, serviceName, lifeTime);
      }

      Log.Info($"Registered service: {bindTo.FullName}");
    }

    private string GetServiceName(Type implementation, ServiceBindingOptionsAttribute options)
    {
      short bindingOrder = options?.Order ?? (short)BindingOrder.Default;
      return bindingOrder.ToString("D5") + implementation.FullName;
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
        Log.Debug($"Bind: {bindingInfo.BindFrom.FullName} -> {bindTo.FullName}");
      }
    }

    /// <summary>
    /// Override in a child class to specify additional bindings/overrides.<br/>
    /// See https://www.lightinject.net/ for documentation.
    /// </summary>
    protected virtual void RegisterOverrides() {}
  }
}
