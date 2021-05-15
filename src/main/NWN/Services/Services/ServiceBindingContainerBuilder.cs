using System;
using System.Reflection;
using LightInject;
using NLog;
using NWN.API;
using NWN.Plugins;

namespace NWN.Services
{
  /// <summary>
  /// Wires up and prepares service classes for dependency injection.
  /// </summary>
  public class ServiceBindingContainerBuilder : IContainerBuilder
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    protected ITypeLoader typeLoader;
    protected ServiceContainer serviceContainer;

    public ServiceContainer Setup(ITypeLoader typeLoader)
    {
      this.typeLoader = typeLoader;
      this.serviceContainer = new ServiceContainer(new ContainerOptions {EnablePropertyInjection = false, EnableVariance = false});

      return serviceContainer;
    }

    public void RegisterCoreService<T>(T instance)
    {
      Type instanceType = instance.GetType();
      ServiceBindingOptionsAttribute options = instanceType.GetCustomAttribute<ServiceBindingOptionsAttribute>();

      string serviceName = GetServiceName(instanceType, options);
      serviceContainer.RegisterInstance(instance, serviceName);
    }

    public void BuildContainer()
    {
      SearchForBindings();
      RegisterOverrides();
    }

    private void SearchForBindings()
    {
      Log.Info("Loading services...");

      foreach (Type type in typeLoader.LoadedTypes)
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

      if (options is not { Lazy: true })
      {
        serviceContainer.Register(typeof(object), bindTo, serviceName, lifeTime);
      }

      foreach (ServiceBindingAttribute bindingInfo in newBindings)
      {
        serviceContainer.Register(bindingInfo.BindFrom, bindTo, serviceName, lifeTime);
        Log.Debug($"Bind: {bindingInfo.BindFrom.FullName} -> {bindTo.FullName}");
      }

      Log.Info($"Registered service: {bindTo.FullName}");
    }

    private string GetServiceName(Type implementation, ServiceBindingOptionsAttribute options)
    {
      short bindingOrder = options?.Order ?? (short)BindingOrder.Default;
      return bindingOrder.ToString("D5") + implementation.FullName;
    }

    /// <summary>
    /// Override in a child class to specify additional bindings/overrides.<br/>
    /// See https://www.lightinject.net/ for documentation.
    /// </summary>
    protected virtual void RegisterOverrides() {}
  }
}
