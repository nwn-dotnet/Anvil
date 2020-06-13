using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using SimpleInjector;

namespace NWN.Services
{
  public class ServiceManager : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private readonly Container container;
    internal ServiceManager(IBindingInstaller bindingInstaller)
    {
      container = new Container();
      Log.Info($"Using \"{bindingInstaller.GetType().FullName}\" to install service bindings.");
      bindingInstaller.ConfigureBindings(container);
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