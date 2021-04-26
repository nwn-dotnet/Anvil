using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using NWN.Services;

namespace NWN.API
{
  public static partial class NwTask
  {
    private static int managedThreadId;
    private static TaskRunner taskRunner;

    public static SyncContext MainThreadScriptContext { get; private set; }

    [ServiceBinding(typeof(IUpdateable))]
    [BindingOrder(BindingOrder.API)]
    internal class TaskRunner : IUpdateable
    {
      private readonly List<ScheduledItem> currentWork = new List<ScheduledItem>();
      private readonly List<ScheduledItem> scheduledItems = new List<ScheduledItem>();

      public TaskRunner()
      {
        managedThreadId = Thread.CurrentThread.ManagedThreadId;
        MainThreadScriptContext = new SyncContext();
        taskRunner = this;
      }

      public Task Schedule(Func<bool> completionSource, CancellationToken? cancellationToken = null)
      {
        ScheduledItem scheduledItem = new ScheduledItem(completionSource, cancellationToken);
        lock (scheduledItems)
        {
          scheduledItems.Add(scheduledItem);
        }

        return scheduledItem.Task;
      }

      void IUpdateable.Update()
      {
        MainThreadScriptContext.Update();

        lock (scheduledItems)
        {
          currentWork.AddRange(scheduledItems);
        }

        foreach (ScheduledItem item in currentWork)
        {
          if (item.IsComplete())
          {
            lock (scheduledItems)
            {
              scheduledItems.Remove(item);
            }
          }
        }

        currentWork.Clear();
      }
    }

    private class ScheduledItem
    {
      private static readonly Logger Log = LogManager.GetCurrentClassLogger();

      private readonly Func<bool> completionSource;
      private readonly TaskCompletionSource taskCompletionSource;
      private readonly CancellationToken? cancellationToken;

      public Task Task { get; }

      public ScheduledItem(Func<bool> completionSource, CancellationToken? cancellationToken)
      {
        this.completionSource = completionSource;
        this.taskCompletionSource = new TaskCompletionSource();
        this.cancellationToken = cancellationToken;

        Task = taskCompletionSource.Task;
      }

      public bool IsComplete()
      {
        Task task = taskCompletionSource.Task;
        if (task.IsCompleted)
        {
          return true;
        }

        try
        {
          if (completionSource())
          {
            taskCompletionSource.SetResult();
          }
          else if (cancellationToken.HasValue && cancellationToken.Value.IsCancellationRequested)
          {
            taskCompletionSource.SetCanceled();
          }
        }
        catch (Exception e)
        {
          Log.Error(e);
          taskCompletionSource.SetException(e);
          return true;
        }

        return task.IsCompleted;
      }
    }
  }
}
