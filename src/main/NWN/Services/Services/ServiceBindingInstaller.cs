using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using NWN.API;
using SimpleInjector;

namespace NWN.Services
{
  public class ServiceBindingInstaller : IBindingInstaller
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private Container container;

    private Dictionary<Type, List<Binding>> bindings = new Dictionary<Type, List<Binding>>();

    public virtual void ConfigureBindings(Container container, IReadOnlyCollection<Type> allTypes)
    {
      this.container = container;
      SearchForBindings(allTypes);
      RegisterBindings();
      bindings = null;

      container.Options.AllowOverridingRegistrations = true;
      RegisterOverrides();
      container.Options.AllowOverridingRegistrations = false;
    }

    private void SearchForBindings(IReadOnlyCollection<Type> allTypes)
    {
      Log.Info("Loading managed services");
      foreach (Type type in allTypes)
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

          container.Register(serviceType, binding.ImplementationType, binding.BindingInfo.BindingType.ToLifestyle());
          container.Collection.Append(serviceType, binding.ImplementationType, binding.BindingInfo.BindingType.ToLifestyle());
          Log.Debug($"Bind: {serviceType.FullName} -> {binding.ImplementationType.FullName}");
        }
        else
        {
          Log.Debug($"Bind: {serviceType} --");
          foreach (Binding binding in implementations)
          {
            container.Collection.Append(serviceType, binding.ImplementationType, binding.BindingInfo.BindingType.ToLifestyle());
            Log.Debug($"  ->  {binding.ImplementationType.FullName}");
          }
        }
      }
    }

    protected virtual void RegisterOverrides() {}
  }
}