using System;
using System.Threading;
using System.Threading.Tasks;
using NWN.Services;

namespace NWN.API
{
  public static partial class NwTask
  {
    private static async Task RunAndAwait(Func<bool> completionSource)
    {
      SynchronizationContext.SetSynchronizationContext(Scheduler.SyncContext);
      await AwaitCompletionSource(completionSource).ConfigureAwait(false);
    }

    private static async Task AwaitCompletionSource(Func<bool> completionSource)
    {
      await completionSource;
    }

    [ServiceBinding(typeof(IUpdateable))]
    [BindingOrder(BindingOrder.API)]
    public class Scheduler : IUpdateable
    {
      internal static SyncContext SyncContext { get; private set; }

      public Scheduler()
      {
        SyncContext = new SyncContext(Thread.CurrentThread.ManagedThreadId);
        SynchronizationContext.SetSynchronizationContext(SyncContext);
      }

      public void Update()
      {
        if (SynchronizationContext.Current is SyncContext syncContext)
        {
          syncContext.Update();
        }
      }

      public static void RunInSyncContext(Action continuation)
      {
        SyncContext.Post(_ => continuation(), null);
      }
    }
  }
}
