using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using LightInject;

namespace Anvil.Services
{
  [ServiceBindingOptions(InternalBindingPriority.Highest)]
  internal sealed class ContainerMessageService : ICoreService
  {
    private readonly List<IUpdateable> updateTargets = new List<IUpdateable>();
    private readonly HashSet<ILateDisposable> lateDisposeTargets = new HashSet<ILateDisposable>();

    public ContainerMessageService(IServiceManager serviceManager)
    {
      serviceManager.OnContainerCreate += OnContainerCreate;
      serviceManager.OnContainerDispose += OnContainerDispose;
    }

    internal void RunServerLoop()
    {
      foreach (IUpdateable updateTarget in updateTargets)
      {
        updateTarget.Update();
      }
    }

    private void OnContainerCreate(IServiceContainer container)
    {
      foreach (IInitializable service in container.GetAllInstances<IInitializable>().OrderBy(service => service.GetType().GetServicePriority()))
      {
        service.Init();
      }

      updateTargets.AddRange(container.GetAllInstances<IUpdateable>().OrderBy(service => service.GetType().GetServicePriority()));
    }

    private void OnContainerDispose(IServiceContainer container)
    {
      foreach (IUpdateable updateTarget in container.GetAllInstances<IUpdateable>())
      {
        updateTargets.Remove(updateTarget);
      }

      foreach (ILateDisposable lateDisposeTarget in container.GetAllInstances<ILateDisposable>())
      {
        lateDisposeTargets.Add(lateDisposeTarget);
      }
    }

    void ICoreService.Init() {}

    void ICoreService.Load() {}

    void ICoreService.Shutdown()
    {
      foreach (ILateDisposable lateDisposable in lateDisposeTargets)
      {
        lateDisposable.LateDispose();
      }

      lateDisposeTargets.Clear();
    }

    void ICoreService.Start() {}

    void ICoreService.Unload() {}
  }
}
