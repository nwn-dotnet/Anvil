using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Anvil.Services;
using NLog;

namespace Anvil.API
{
  [ServiceBinding(typeof(MainThreadSynchronizationContext))]
  [ServiceBinding(typeof(IUpdateable))]
  [ServiceBindingOptions(BindingOrder.API)]
  public sealed class MainThreadSynchronizationContext : SynchronizationContext, IUpdateable, IAwaitable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly List<QueuedTask> queuedTasks = new List<QueuedTask>();
    private readonly List<QueuedTask> currentWork = new List<QueuedTask>();

    void IUpdateable.Update()
    {
      lock (queuedTasks)
      {
        currentWork.AddRange(queuedTasks);
        queuedTasks.Clear();
      }

      try
      {
        foreach (QueuedTask task in currentWork)
        {
          task.Invoke();
        }
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
      finally
      {
        currentWork.Clear();
      }
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
