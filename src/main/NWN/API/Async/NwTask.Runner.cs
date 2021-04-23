using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NWN.Services;

namespace NWN.API
{
  public static partial class NwTask
  {
    private static readonly HashSet<ScheduledItem> ScheduledItems = new HashSet<ScheduledItem>();
    private static readonly object SchedulerLock = new object();

    private static int managedThreadId;

    private static async Task RunAndAwait(Func<bool> completionSource)
    {
      if (completionSource())
      {
        await Task.CompletedTask;
      }

      ScheduledItem scheduledItem = new ScheduledItem(completionSource);
      lock (SchedulerLock)
      {
        ScheduledItems.Add(scheduledItem);
      }

      await scheduledItem.TaskCompletionSource.Task.ConfigureAwait(false);
    }

    [ServiceBinding(typeof(IUpdateable))]
    [BindingOrder(BindingOrder.API)]
    internal class TaskRunner : IUpdateable
    {
      public TaskRunner()
      {
        managedThreadId = Thread.CurrentThread.ManagedThreadId;
      }

      void IUpdateable.Update()
      {
        lock (SchedulerLock)
        {
          ScheduledItems.RemoveWhere(item =>
          {
            if (!item.CompletionSource())
            {
              return false;
            }

            item.TaskCompletionSource.SetResult(true);
            return true;
          });
        }
      }
    }

    private class ScheduledItem
    {
      public readonly Func<bool> CompletionSource;
      public readonly TaskCompletionSource<bool> TaskCompletionSource = new TaskCompletionSource<bool>();

      public ScheduledItem(Func<bool> completionSource)
      {
        CompletionSource = completionSource;
      }
    }
  }
}
