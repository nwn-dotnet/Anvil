using System;
using System.Collections.Generic;
using System.Threading;
using Anvil.Services;
using NLog;

namespace Anvil.API
{
  /// <summary>
  /// This synchronization context is invoked by the NWN server main loop.<br/>
  /// Using <see cref="SynchronizationContextAwaiter"/>, it allows async code to return to a valid script context by using await with this class.<br/>
  /// Async tasks are queued into a list, before being flushed on the next loop update.
  /// </summary>
  [ServiceBinding(typeof(MainThreadSynchronizationContext))]
  [ServiceBinding(typeof(IUpdateable))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  internal sealed class MainThreadSynchronizationContext : SynchronizationContext, IUpdateable, IAwaitable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly List<QueuedTask> currentWork = new List<QueuedTask>();
    private readonly List<QueuedTask> queuedTasks = new List<QueuedTask>();

    public IAwaiter GetAwaiter()
    {
      return new SynchronizationContextAwaiter(this);
    }

    public override void Post(SendOrPostCallback callback, object? state)
    {
      lock (queuedTasks)
      {
        queuedTasks.Add(new QueuedTask(callback, state));
      }
    }

    public override void Send(SendOrPostCallback callback, object? state)
    {
      lock (queuedTasks)
      {
        queuedTasks.Add(new QueuedTask(callback, state));
      }
    }

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

    private readonly struct QueuedTask
    {
      private readonly SendOrPostCallback callback;
      private readonly object? state;

      public QueuedTask(SendOrPostCallback callback, object? state)
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
