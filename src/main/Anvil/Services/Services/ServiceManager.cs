using System.Collections.Generic;
using System.Linq;
using Anvil.Plugins;
using LightInject;
using NLog;

namespace Anvil.Services
{
  [ServiceBindingOptions(BindingOrder.Core)]
  internal sealed class ServiceManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly ITypeLoader typeLoader;
    private readonly IContainerFactory containerFactory;

    private ServiceContainer serviceContainer;
    private List<ILateDisposable> lateDisposables;

    internal ServiceManager(ITypeLoader typeLoader, IContainerFactory containerFactory)
    {
      Log.Info($"Using \"{containerFactory.GetType().FullName}\" to install service bindings.");

      this.typeLoader = typeLoader;
      this.containerFactory = containerFactory;

      serviceContainer = containerFactory.Setup(typeLoader);
    }

    ~ServiceManager()
    {
      ShutdownServices();
      ShutdownLateServices();
    }

    public T GetService<T>() where T : class
    {
      return serviceContainer.GetInstance<T>();
    }

    internal void Init()
    {
      RegisterCoreService(typeLoader);
      RegisterCoreService(this);

      containerFactory.BuildContainer();
      NotifyInitComplete();
    }

    internal void ShutdownServices()
    {
      if (serviceContainer == null)
      {
        return;
      }

      Log.Info("Unloading services...");
      lateDisposables = serviceContainer.GetAllInstances<ILateDisposable>().ToList();
      serviceContainer.Dispose();

      serviceContainer = null;
    }

    internal void ShutdownLateServices()
    {
      foreach (ILateDisposable lateDisposable in lateDisposables)
      {
        lateDisposable.LateDispose();
      }

      lateDisposables = null;
    }

    private void RegisterCoreService<T>(T instance)
    {
      containerFactory.RegisterCoreService(instance);
    }

    private void NotifyInitComplete()
    {
      foreach (IInitializable initializable in serviceContainer.GetAllInstances<IInitializable>())
      {
        initializable.Init();
      }
    }
  }
}
