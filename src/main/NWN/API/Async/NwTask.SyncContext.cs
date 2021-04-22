using System.Collections.Concurrent;
using System.Threading;

namespace NWN.API
{
  public static partial class NwTask
  {
    internal sealed class SyncContext : SynchronizationContext
    {
      private readonly BlockingCollection<QueuedItem> queue = new BlockingCollection<QueuedItem>();

      private readonly int mainThreadId;

      public SyncContext(int mainThreadId)
      {
        this.mainThreadId = mainThreadId;
      }

      public override void Send(SendOrPostCallback callback, object state)
      {
        if (mainThreadId == Thread.CurrentThread.ManagedThreadId)
        {
          callback(state);
          return;
        }

        queue.Add(new QueuedItem(callback, state));
      }

      public override void Post(SendOrPostCallback callback, object state)
      {
        queue.Add(new QueuedItem(callback, state));
      }

      public override SynchronizationContext CreateCopy()
      {
        return new SyncContext(mainThreadId);
      }

      internal void Update()
      {
        foreach (QueuedItem item in queue.GetConsumingEnumerable())
        {
          item.Invoke();
        }
      }

      private readonly struct QueuedItem
      {
        private readonly SendOrPostCallback callback;
        private readonly object state;

        public QueuedItem(SendOrPostCallback callback, object state)
        {
          this.callback = callback;
          this.state = state;
        }

        public void Invoke()
        {
          callback(state);
        }
      }
    }
  }
}
