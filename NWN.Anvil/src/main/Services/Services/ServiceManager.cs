using System.Collections.Generic;
using System.Linq;
using Anvil.Internal;
using Anvil.Plugins;
using LightInject;
using NLog;

namespace Anvil.Services
{
  [ServiceBindingOptions(InternalBindingPriority.Core)]
  public sealed class ServiceManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private readonly IContainerFactory containerFactory;
    private readonly CoreInteropHandler interopHandler;

    private readonly PluginManager pluginManager;
    private List<ILateDisposable> lateDisposables;

    private ServiceContainer serviceContainer;

    internal ServiceManager(PluginManager pluginManager, CoreInteropHandler interopHandler, IContainerFactory containerFactory)
    {
      this.pluginManager = pluginManager;
      this.interopHandler = interopHandler;
      this.containerFactory = containerFactory;

      Log.Info("Using {ContainerFactory} to install service bindings", containerFactory.GetType().FullName);
    }

    public T GetService<T>() where T : class
    {
      return serviceContainer.GetInstance<T>();
    }

    internal void Init(params object[] coreServices)
    {
      serviceContainer = containerFactory.CreateContainer(pluginManager, coreServices);
      interopHandler.Init(GetService<ICoreRunScriptHandler>(), GetService<ICoreLoopHandler>());
      InitServices();
    }

    internal void ShutdownLateServices()
    {
      if (lateDisposables == null)
      {
        return;
      }

      foreach (ILateDisposable lateDisposable in lateDisposables)
      {
        lateDisposable.LateDispose();
      }

      lateDisposables = null;
    }

    internal void ShutdownServices()
    {
      if (serviceContainer == null)
      {
        return;
      }

      Log.Info("Unloading services...");
      lateDisposables = serviceContainer.GetAllInstances<ILateDisposable>().ToList();

      interopHandler.Dispose();
      serviceContainer.Dispose();
      serviceContainer = null;
    }

    private void InitServices()
    {
      foreach (IInitializable initializable in serviceContainer.GetAllInstances<IInitializable>())
      {
        initializable.Init();
      }
    }
  }
}
