using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using LightInject;

namespace Anvil.Services
{
  internal sealed class ContainerMessageService : ICoreService
  {
    private readonly List<IUpdateable> updateTargets = new List<IUpdateable>();
    private readonly HashSet<ILateDisposable> lateDisposeTargets = new HashSet<ILateDisposable>();

    private readonly List<IUpdateable> pendingAddTargets = new List<IUpdateable>();
    private readonly List<IUpdateable> pendingRemoveTargets = new List<IUpdateable>();

    public ContainerMessageService(IServiceManager serviceManager)
    {
      serviceManager.OnContainerCreate += OnContainerCreate;
      serviceManager.OnContainerDispose += OnContainerDispose;
    }

    internal void RunServerLoop()
    {
      for (int i = 0; i < updateTargets.Count; i++)
      {
        updateTargets[i].Update();
      }

      if (pendingAddTargets.Count > 0 || pendingRemoveTargets.Count > 0)
      {
        UpdateLoopList();
      }
    }

    private void UpdateLoopList()
    {
      foreach (IUpdateable updateable in pendingRemoveTargets)
      {
        updateTargets.Remove(updateable);
      }

      foreach (IUpdateable updateable in pendingAddTargets)
      {
        updateTargets.Add(updateable);
      }

      pendingAddTargets.Clear();
      pendingRemoveTargets.Clear();
    }

    private void OnContainerCreate(IServiceContainer container)
    {
      foreach (IInitializable service in container.GetAllInstances<IInitializable>().OrderBy(service => service.GetType().GetServicePriority()))
      {
        service.Init();
      }

      pendingAddTargets.AddRange(container.GetAllInstances<IUpdateable>().OrderBy(service => service.GetType().GetServicePriority()));
    }

    private void OnContainerDispose(IServiceContainer container)
    {
      pendingRemoveTargets.AddRange(container.GetAllInstances<IUpdateable>());

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
