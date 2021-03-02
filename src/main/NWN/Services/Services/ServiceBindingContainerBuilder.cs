using System;
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
      this.serviceContainer = new ServiceContainer(new ContainerOptions {EnablePropertyInjection = false});

      return serviceContainer;
    }

    public void RegisterCoreService<T>(T instance)
    {
      serviceContainer.RegisterInstance(instance);
    }

    public void BuildContainer()
    {
      SearchForBindings();
      RegisterOverrides();
    }

    private void SearchForBindings()
    {
      Log.Info("Loading managed services...");
      foreach (Type type in typeLoader.LoadedTypes)
      {
        RegisterBindings(type, type.GetCustomAttributes<ServiceBindingAttribute>());
      }
    }

    private void RegisterBindings(Type bindTo, ServiceBindingAttribute[] newBindings)
    {
      if (newBindings.Length == 0)
      {
        return;
      }

      PerContainerLifetime lifeTime = new PerContainerLifetime();
      serviceContainer.Register(typeof(object), bindTo, lifeTime);

      if (bindTo.IsAssignableTo(typeof(IDisposable)))
      {
        serviceContainer.Register(typeof(IDisposable), bindTo, lifeTime);
      }

      foreach (ServiceBindingAttribute bindingInfo in newBindings)
      {
        serviceContainer.Register(bindingInfo.BindFrom, bindTo, lifeTime);
        Log.Debug($"Bind: {bindingInfo.BindFrom.FullName} -> {bindTo.FullName}");
      }

      Log.Info($"Registered service: {bindTo.FullName}");
    }

    /// <summary>
    /// Override in a child class to specify additional bindings/overrides.<br/>
    /// See https://www.lightinject.net/ for documentation.
    /// </summary>
    protected virtual void RegisterOverrides() {}
  }
}
