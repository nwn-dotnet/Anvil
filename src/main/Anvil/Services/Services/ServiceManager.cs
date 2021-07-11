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

    private readonly IContainerFactory containerFactory;
    private readonly ServiceContainer serviceContainer;

    internal ServiceManager(ITypeLoader typeLoader, IContainerFactory containerFactory)
    {
      Log.Info($"Using \"{containerFactory.GetType().FullName}\" to install service bindings.");

      this.typeLoader = typeLoader;
      this.containerFactory = containerFactory;

      serviceContainer = containerFactory.Setup(typeLoader);
    }

    internal void RegisterCoreService<T>(T instance)
    {
      containerFactory.RegisterCoreService(instance);
    }

    internal void Init()
    {
      containerFactory.BuildContainer();
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
