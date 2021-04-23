using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using NWN.Services;

namespace NWN.API
{
  public static partial class NwTask
  {
    private static readonly BlockingCollection<ScheduledItem> ScheduledItems = new BlockingCollection<ScheduledItem>();

    private static int managedThreadId;

    private static async Task RunAndAwait(Func<bool> completionSource)
    {
      if (completionSource())
      {
        await Task.CompletedTask;
      }

      ScheduledItem scheduledItem = new ScheduledItem(completionSource);
      ScheduledItems.Add(scheduledItem);

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
        foreach (ScheduledItem item in ScheduledItems.GetConsumingEnumerable())
        {
          if (!item.CompletionSource())
          {
            ScheduledItems.Add(item);
            continue;
          }

          item.TaskCompletionSource.SetResult(true);
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
