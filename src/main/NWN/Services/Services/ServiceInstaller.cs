using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;
using NWN.API;
using SimpleInjector;

namespace NWN.Services
{
  /// <summary>
  /// Wires up and prepares service classes for dependency injection.
  /// </summary>
  public class ServiceInstaller : IBindingInstaller
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private Dictionary<Type, List<Binding>> bindings = new Dictionary<Type, List<Binding>>();
    private InstallerInfo installerInfo;

    public void ConfigureBindings(InstallerInfo installerInfo)
    {
      this.installerInfo = installerInfo;
      installerInfo.ServiceContainer.ResolveUnregisteredType += (sender, args) => OnResolveUnregisteredType(installerInfo.ServiceContainer, args);

      SearchForBindings();
      RegisterCoreServices();
      RegisterBindings();

      bindings.Clear();

      installerInfo.ServiceContainer.Options.AllowOverridingRegistrations = true;
      RegisterOverrides();
      installerInfo.ServiceContainer.Options.AllowOverridingRegistrations = false;
    }

    private void OnResolveUnregisteredType(Container container, UnregisteredTypeEventArgs e)
    {
      if (!e.UnregisteredServiceType.IsGenericType)
      {
        return;
      }

      Type genericTypeDef = e.UnregisteredServiceType.GetGenericTypeDefinition();
      if (IsSupportedCollectionType(genericTypeDef))
      {
        Type elementType = e.UnregisteredServiceType.GetGenericArguments().First();
        Type newCollection = typeof(List<>).MakeGenericType(elementType);

        object instance = Activator.CreateInstance(newCollection);
        e.Register(Lifestyle.Singleton.CreateRegistration(newCollection, instance, container));
      }
    }

    private bool IsSupportedCollectionType(Type genericTypeDef)
    {
      return genericTypeDef != null &&
        genericTypeDef == typeof(IEnumerable<>) ||
        genericTypeDef == typeof(ICollection<>) ||
        genericTypeDef == typeof(IList<>) ||
        genericTypeDef == typeof(IReadOnlyCollection<>) ||
        genericTypeDef == typeof(IReadOnlyList<>);
    }

    private void RegisterCoreServices()
    {
      installerInfo.CoreContainer.RegisterInstance(installerInfo.TypeLoader);
      installerInfo.CoreContainer.RegisterInstance(installerInfo.ServiceManager);
    }

    private void SearchForBindings()
    {
      Log.Info("Loading managed services");
      foreach (Type type in installerInfo.TypeLoader.LoadedTypes)
      {
        if (PopulateBindings(type, type.GetCustomAttributes<ServiceBindingAttribute>()))
        {
          Log.Info($"Registered service: {type.FullName}");
        }
      }
    }

    private bool PopulateBindings(Type bindFrom, IEnumerable<ServiceBindingAttribute> newBindings)
    {
      bool addedBinding = false;
      foreach (ServiceBindingAttribute bindingInfo in newBindings)
      {
        Binding binding = new Binding(bindFrom, bindingInfo);
        bindings.AddElement(bindingInfo.BindFrom, binding);
        addedBinding = true;
      }

      return addedBinding;
    }

    private void RegisterBindings()
    {
      foreach ((Type serviceType, List<Binding> implementations) in bindings)
      {
        if (implementations.Count == 1)
        {
          Binding binding = implementations[0];
          RegisterType(binding.BindingInfo.BindingContext, serviceType, binding.ImplementationType, binding.BindingInfo.BindingType.ToLifestyle());
          Log.Debug($"Bind: {serviceType.FullName} -> {binding.ImplementationType.FullName}");
        }
        else
        {
          Log.Debug($"Bind: {serviceType} --");
          foreach (Binding binding in implementations)
          {
            AppendType(binding.BindingInfo.BindingContext, serviceType, binding.ImplementationType, binding.BindingInfo.BindingType.ToLifestyle());
            Log.Debug($"  ->  {binding.ImplementationType.FullName}");
          }
        }
      }
    }

    private void RegisterType(BindingContext bindingContext, Type serviceType, Type implementationType, Lifestyle lifestyle)
    {
      switch (bindingContext)
      {
        case BindingContext.Service:
          installerInfo.ServiceContainer.Collection.Append(serviceType, implementationType, lifestyle);
          installerInfo.ServiceContainer.Register(serviceType, implementationType, lifestyle);
          break;
        case BindingContext.API:
          installerInfo.CoreContainer.Register(serviceType, implementationType, lifestyle);
          break;
      }
    }

    private void AppendType(BindingContext bindingContext, Type serviceType, Type implementationType, Lifestyle lifestyle)
    {
      switch (bindingContext)
      {
        case BindingContext.Service:
          installerInfo.ServiceContainer.Collection.Append(serviceType, implementationType, lifestyle);
          break;
        case BindingContext.API:
          installerInfo.CoreContainer.Collection.Append(serviceType, implementationType, lifestyle);
          break;
      }
    }

    protected virtual void RegisterOverrides() {}

    private class Binding
    {
      public readonly Type ImplementationType;
      public readonly ServiceBindingAttribute BindingInfo;

      public Binding(Type implementationType, ServiceBindingAttribute bindingInfo)
      {
        this.ImplementationType = implementationType;
        this.BindingInfo = bindingInfo;
      }
    }
  }
}
