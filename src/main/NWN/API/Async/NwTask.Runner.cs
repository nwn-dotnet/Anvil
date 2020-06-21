using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWN.Services;

namespace NWN.API
{
  public static partial class NwTask
  {
    private static readonly HashSet<ScheduledItem> scheduledItems = new HashSet<ScheduledItem>();

    private static Task RunAndAwait(Func<bool> completionSource)
    {
      if (completionSource())
      {
        return Task.CompletedTask;
      }

      ScheduledItem scheduledItem = new ScheduledItem(completionSource);
      scheduledItems.Add(scheduledItem);

      return scheduledItem.TaskCompletionSource.Task;
    }

    [ServiceBinding(typeof(IUpdateable))]
    public class TaskRunner : IUpdateable
    {
      void IUpdateable.Update()
      {
        scheduledItems.RemoveWhere(item =>
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