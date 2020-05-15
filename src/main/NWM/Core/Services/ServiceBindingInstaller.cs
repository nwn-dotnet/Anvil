using System;
using System.Linq;
using System.Reflection;
using NLog;
using SimpleInjector;

namespace NWM.Core
{
  public class ServiceBindingInstaller : IBindingInstaller
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private Container container;

    public virtual void ConfigureBindings(Container container)
    {
      this.container = container;
      RegisterAssemblies();

      container.Options.AllowOverridingRegistrations = true;
      RegisterOverrides();
      container.Options.AllowOverridingRegistrations = false;
    }

    private void RegisterAssemblies()
    {
      Log.Info("Loading managed services");
      Assembly nwmAssembly = Assembly.GetExecutingAssembly();
      string nwmAssemblyName = nwmAssembly.FullName;

      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      foreach (Assembly assembly in assemblies)
      {
        if (assembly == nwmAssembly || assembly.GetReferencedAssemblies().Any(name => name.ToString() == nwmAssemblyName))
        {
          Log.Debug($"Registering assembly: {assembly.FullName}");
          RegisterAssemblyServices(assembly);
        }
      }
    }

    private void RegisterAssemblyServices(Assembly assembly)
    {
      foreach (Type type in assembly.GetTypes())
      {
        ServiceAttribute service = type.GetCustomAttribute<ServiceAttribute>();
        if (service == null)
        {
          continue;
        }

        RegisterService(type, service);
        Log.Info($"Registered service: {type.Name}");
      }
    }

    protected virtual void RegisterService(Type toType, ServiceAttribute service)
    {
      Lifestyle lifestyle = service.BindingType.ToLifestyle();

      if (service.BindSelf)
      {
        Log.Debug($"Bind: {toType.FullName} -> {toType.FullName}");
        container.Register(toType, toType, lifestyle);
      }

      if (!service.IsCollection)
      {
        foreach (Type fromType in service.BindFrom)
        {
          Log.Debug($"Bind: {fromType.FullName} -> {toType.FullName}");
          container.Register(fromType, toType, lifestyle);
        }
      }
      else
      {
        foreach (Type fromType in service.BindFrom)
        {
          Log.Debug($"Append Bind: {fromType.FullName} -> {toType.FullName}");
          container.Collection.Append(fromType, toType, lifestyle);
        }
      }
    }

    protected virtual void RegisterOverrides() {}
  }
}