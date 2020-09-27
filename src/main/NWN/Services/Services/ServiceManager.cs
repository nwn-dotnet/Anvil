using System.Collections.Generic;
using System.Linq;
using NLog;
using NWN.Plugins;
using SimpleInjector;

namespace NWN.Services
{
  public class ServiceManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private readonly Container coreContainer;
    private readonly Container serviceContainer;

    public IReadOnlyList<object> RegisteredServices { get; private set; }

    internal ServiceManager(ITypeLoader typeLoader, IBindingInstaller bindingInstaller)
    {
      Log.Info($"Using \"{bindingInstaller.GetType().FullName}\" to install service bindings.");

      coreContainer = new Container();
      serviceContainer = new Container();

      InstallerInfo installerInfo = new InstallerInfo
      {
        CoreContainer = coreContainer,
        ServiceContainer = serviceContainer,
        ServiceManager = this,
        TypeLoader = typeLoader
      };

      bindingInstaller.ConfigureBindings(installerInfo);
    }

    internal void Init()
    {
      InitServices();
      NotifyInitComplete();
    }

    ~ServiceManager()
    {
      Dispose();
    }

    internal T GetService<T>() where T : class
    {
      if (!serviceContainer.IsLocked)
      {
        return coreContainer.GetInstance<T>();
      }

      return serviceContainer.GetInstance<T>();
    }

    private void InitServices()
    {
      coreContainer.Verify();

      foreach (InstanceProducer instanceProducer in coreContainer.GetCurrentRegistrations())
      {
        serviceContainer.RegisterInstance(instanceProducer.ServiceType, instanceProducer.GetInstance());
      }

      serviceContainer.Verify();
      RegisteredServices = GetRegisteredServices(serviceContainer).ToList().AsReadOnly();
    }

    private void NotifyInitComplete()
    {
      foreach (IInitializable initializable in serviceContainer.GetAllInstances<IInitializable>())
      {
        initializable.Init();
      }
    }

    private IEnumerable<object> GetRegisteredServices(Container container)
    {
      return container.GetCurrentRegistrations()
        .Where(producer => producer.Lifestyle == Lifestyle.Singleton)
        .Select(producer => producer.GetInstance())
        .Distinct();
    }

    internal void Dispose()
    {
      serviceContainer?.Dispose();
      coreContainer?.Dispose();
    }
  }
}
