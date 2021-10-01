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

    private readonly PluginManager pluginManager;
    private readonly IContainerFactory containerFactory;

    private ServiceContainer serviceContainer;
    private List<ILateDisposable> lateDisposables;

    internal ServiceManager(PluginManager pluginManager, IContainerFactory containerFactory)
    {
      Log.Info("Using {ContainerFactory} to install service bindings", containerFactory.GetType().FullName);

      this.pluginManager = pluginManager;
      this.containerFactory = containerFactory;

      serviceContainer = containerFactory.Setup(pluginManager);
    }

    public T GetService<T>() where T : class
    {
      return serviceContainer.GetInstance<T>();
    }

    internal void Init()
    {
      RegisterCoreService(pluginManager);
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
