using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NWN.Services;

namespace NWN.API
{
  public static partial class NwTask
  {
    private static readonly List<ScheduledItem> ScheduledItems = new List<ScheduledItem>();
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
        ScheduledItem[] items;
        lock (SchedulerLock)
        {
          items = ScheduledItems.ToArray();
          Console.WriteLine(items.Length);
        }

        foreach (ScheduledItem item in items)
        {
          if (item.IsComplete())
          {
            lock (SchedulerLock)
            {
              ScheduledItems.Remove(item);
            }
          }
        }
      }
    }

    private class ScheduledItem
    {
      public readonly Func<bool> CompletionSource;
      public readonly TaskCompletionSource TaskCompletionSource = new TaskCompletionSource();

      public ScheduledItem(Func<bool> completionSource)
      {
        CompletionSource = completionSource;
      }

      public bool IsComplete()
      {
        if (CompletionSource())
        {
          TaskCompletionSource.SetResult();
        }

        return TaskCompletionSource.Task.IsCompleted;
      }
    }
  }
}
