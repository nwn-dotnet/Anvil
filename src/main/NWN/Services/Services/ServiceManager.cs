using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using NWN.Plugins;
using SimpleInjector;

namespace NWN.Services
{
  internal class ServiceManager : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private readonly Container container;

    internal ServiceManager(ITypeLoader typeLoader, IBindingInstaller bindingInstaller)
    {
      container = new Container();
      Log.Info($"Using \"{bindingInstaller.GetType().FullName}\" to install service bindings.");
      bindingInstaller.ConfigureBindings(container, typeLoader.LoadedTypes);
    }

    ~ServiceManager()
    {
      Dispose();
    }

    public IEnumerable<object> GetRegisteredServices()
    {
      return container.GetCurrentRegistrations()
        .Where(producer => producer.Lifestyle == Lifestyle.Singleton)
        .Select(producer => producer.GetInstance())
        .Distinct();
    }

    public T GetService<T>() where T : class
    {
      return container.GetInstance<T>();
    }

    internal void InitServices()
    {
      container.Verify();
    }

    public void Dispose()
    {
      container?.Dispose();
    }
  }
}