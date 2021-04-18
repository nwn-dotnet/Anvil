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
      string serviceName = GetServiceName(instance.GetType());
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

      string serviceName = GetServiceName(bindTo);
      PerContainerLifetime lifeTime = new PerContainerLifetime();

      serviceContainer.Register(typeof(object), bindTo, serviceName, lifeTime);

      foreach (ServiceBindingAttribute bindingInfo in newBindings)
      {
        serviceContainer.Register(bindingInfo.BindFrom, bindTo, serviceName, lifeTime);
        Log.Debug($"Bind: {bindingInfo.BindFrom.FullName} -> {bindTo.FullName}");
      }

      Log.Info($"Registered service: {bindTo.FullName}");
    }

    private string GetServiceName(Type implementation)
    {
      BindingOrderAttribute attribute = implementation.GetCustomAttribute<BindingOrderAttribute>();
      BindingOrder bindingOrder = attribute?.Order ?? BindingOrder.Default;

      return ((short)bindingOrder).ToString("D5") + implementation.FullName;
    }

    /// <summary>
    /// Override in a child class to specify additional bindings/overrides.<br/>
    /// See https://www.lightinject.net/ for documentation.
    /// </summary>
    protected virtual void RegisterOverrides() {}
  }
}
