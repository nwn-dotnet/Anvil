using System.Collections.Generic;
using System.Linq;
using LightInject;
using NLog;
using NWN.Plugins;

namespace NWN.Services
{
  [BindingOrder(BindingOrder.Core)]
  public class ServiceManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly ServiceContainer serviceContainer;

    public List<object> RegisteredServices { get; private set; }

    internal ServiceManager(ITypeLoader typeLoader, IContainerBuilder containerBuilder)
    {
      Log.Info($"Using \"{containerBuilder.GetType().FullName}\" to install service bindings.");

      serviceContainer = containerBuilder.Setup(typeLoader);

      containerBuilder.RegisterCoreService(typeLoader);
      containerBuilder.RegisterCoreService(this);

      containerBuilder.BuildContainer();
    }

    internal void Init()
    {
      RegisteredServices = serviceContainer.GetAllInstances<object>().ToList();
      NotifyInitComplete();
    }

    ~ServiceManager()
    {
      Dispose();
    }

    internal T GetService<T>() where T : class
    {
      return serviceContainer.GetInstance<T>();
    }

    private void NotifyInitComplete()
    {
      foreach (IInitializable initializable in serviceContainer.GetAllInstances<IInitializable>())
      {
        initializable.Init();
      }
    }

    internal void Dispose()
    {
      serviceContainer?.Dispose();
    }
  }
}
