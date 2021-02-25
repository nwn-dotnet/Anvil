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

    protected ITypeLoader TypeLoader;
    protected ServiceContainer ServiceContainer;

    public ServiceContainer Setup(ITypeLoader typeLoader)
    {
      this.TypeLoader = typeLoader;
      this.ServiceContainer = new ServiceContainer(new ContainerOptions {EnablePropertyInjection = false});

      return ServiceContainer;
    }

    public void RegisterCoreService<T>(T instance)
    {
      ServiceContainer.RegisterInstance(instance);
    }

    public void BuildContainer()
    {
      SearchForBindings();
      RegisterOverrides();
    }

    private void SearchForBindings()
    {
      Log.Info("Loading managed services...");
      foreach (Type type in TypeLoader.LoadedTypes)
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
      ServiceContainer.Register(typeof(object), bindTo, lifeTime);

      if (bindTo.IsAssignableTo(typeof(IDisposable)))
      {
        ServiceContainer.Register(typeof(IDisposable), bindTo, lifeTime);
      }

      foreach (ServiceBindingAttribute bindingInfo in newBindings)
      {
        ServiceContainer.Register(bindingInfo.BindFrom, bindTo, lifeTime);
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
