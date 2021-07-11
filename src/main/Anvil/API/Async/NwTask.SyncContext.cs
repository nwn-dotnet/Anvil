using System;
using System.Collections.Generic;
using System.Threading;
using NLog;

namespace Anvil.API
{
  public static partial class NwTask
  {
    public sealed class SyncContext : SynchronizationContext, IAwaitable
    {
      private static readonly Logger Log = LogManager.GetCurrentClassLogger();

      private readonly List<QueuedTask> queuedTasks = new List<QueuedTask>();
      private readonly List<QueuedTask> currentWork = new List<QueuedTask>();

      internal void Update()
      {
        lock (queuedTasks)
        {
          currentWork.AddRange(queuedTasks);
          queuedTasks.Clear();
        }

        foreach (QueuedTask task in currentWork)
        {
          task.Invoke();
        }

        currentWork.Clear();
      }

      public override void Post(SendOrPostCallback callback, object state)
      {
        lock (queuedTasks)
        {
          queuedTasks.Add(new QueuedTask(callback, state));
        }
      }

      public override void Send(SendOrPostCallback callback, object state)
      {
        lock (queuedTasks)
        {
          queuedTasks.Add(new QueuedTask(callback, state));
        }
      }

      public IAwaiter GetAwaiter()
      {
        return new SynchronizationContextAwaiter(this);
      }

      private readonly struct QueuedTask
      {
        private readonly SendOrPostCallback callback;
        private readonly object state;

        public QueuedTask(SendOrPostCallback callback, object state)
        {
          this.callback = callback;
          this.state = state;
        }

        public void Invoke()
        {
          try
          {
            callback(state);
          }
          catch (Exception e)
          {
            Log.Error(e);
          }
        }
      }
    }
  }
}
