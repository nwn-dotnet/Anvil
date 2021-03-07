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

    private static bool isInScriptContext;
    private static Thread mainThread;

    private static Task RunAndAwait(Func<bool> completionSource)
    {
      if (completionSource())
      {
        return Task.CompletedTask;
      }

      ScheduledItem scheduledItem = new ScheduledItem(completionSource);
      lock (SchedulerLock)
      {
        ScheduledItems.Add(scheduledItem);
      }

      return scheduledItem.TaskCompletionSource.Task;
    }

    [ServiceBinding(typeof(IUpdateable))]
    [BindingOrder(BindingOrder.API)]
    internal class TaskRunner : IUpdateable
    {
      public TaskRunner(DispatchServiceManager dispatchServiceManager)
      {
        mainThread = Thread.CurrentThread;
        dispatchServiceManager.OnScriptContextBegin += () => isInScriptContext = true;
        dispatchServiceManager.OnScriptContextEnd += () => isInScriptContext = false;
      }

      void IUpdateable.Update()
      {
        isInScriptContext = true;

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

        isInScriptContext = false;
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
