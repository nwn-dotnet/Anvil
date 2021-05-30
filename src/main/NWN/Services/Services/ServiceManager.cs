using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LightInject;
using NLog;
using NWN.Plugins;

namespace NWN.Services
{
  [ServiceBindingOptions(BindingOrder.Core)]
  public sealed class ServiceManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    [UsedImplicitly]
    private readonly ITypeLoader typeLoader;

    private readonly IContainerBuilder containerBuilder;
    private readonly ServiceContainer serviceContainer;

    public List<object> RegisteredServices { get; private set; }

    internal ServiceManager(ITypeLoader typeLoader, IContainerBuilder containerBuilder)
    {
      Log.Info($"Using \"{containerBuilder.GetType().FullName}\" to install service bindings.");

      this.typeLoader = typeLoader;
      this.containerBuilder = containerBuilder;

      serviceContainer = containerBuilder.Setup(typeLoader);
    }

    internal void RegisterCoreService<T>(T instance)
    {
      containerBuilder.RegisterCoreService(instance);
    }

    internal void Init()
    {
      containerBuilder.BuildContainer();
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
