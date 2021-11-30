using System;
using System.Collections.Generic;
using Anvil.API;
using NLog;
using Action = System.Action;

namespace Anvil.Services
{
  //! ## Examples
  //! @include SchedulerServiceExample.cs

  /// <summary>
  /// A service for scheduling tasks to run with a timed delay and/or repeat with a regular interval.
  /// </summary>
  [ServiceBinding(typeof(IUpdateable))]
  [ServiceBinding(typeof(SchedulerService))]
  public class SchedulerService : IUpdateable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// The next server loop after the current.
    /// </summary>
    public static readonly TimeSpan NextUpdate = TimeSpan.Zero;

    private readonly IComparer<ScheduledItem> comparer = new ScheduledItem.SortedByExecutionTime();

    // Dependencies
    private readonly LoopTimeService loopTimeService;

    private readonly List<ScheduledItem> scheduledItems = new List<ScheduledItem>(1024);

    public SchedulerService(LoopTimeService loopTimeService)
    {
      this.loopTimeService = loopTimeService;
    }

    /// <summary>
    /// Schedules the specified action to be invoked after the given delay.
    /// </summary>
    /// <param name="task">The task/action to run.</param>
    /// <param name="delay">The delay until the task is run.</param>
    /// <returns>A disposable object representing the scheduled task. Calling Dispose() will prevent the schedule from running.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the delay is less than 0.</exception>
    /// <exception cref="ArgumentNullException">Thrown if no action is specified.</exception>
    public IDisposable Schedule(Action task, TimeSpan delay)
    {
      if (delay < TimeSpan.Zero)
      {
        throw new ArgumentOutOfRangeException(nameof(delay), $"{nameof(delay)} cannot be < zero.");
      }

      if (task == null)
      {
        throw new ArgumentNullException(nameof(task));
      }

      Log.Debug("Scheduled Future Task {TaskName}", task.Method.GetFullName());
      ScheduledItem item = new ScheduledItem(task, loopTimeService.Time + delay.TotalSeconds);
      scheduledItems.InsertOrdered(item, comparer);
      return item;
    }

    /// <summary>
    /// Schedules the specified task to be invoked on a specified schedule, after an optional delay.
    /// </summary>
    /// <param name="task">The task/action to be run.</param>
    /// <param name="schedule">The delay between invocations.</param>
    /// <param name="delay">An additional delay for the first time run.</param>
    /// <returns>A disposable object representing the scheduled task. Calling Dispose() will cancel the schedule and any future invocations.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the schedule is less than 0.</exception>
    /// <exception cref="ArgumentNullException">Thrown if no action is specified.</exception>
    public IDisposable ScheduleRepeating(Action task, TimeSpan schedule, TimeSpan delay = default)
    {
      if (schedule <= TimeSpan.Zero)
      {
        throw new ArgumentOutOfRangeException(nameof(delay), $"{nameof(delay)} cannot be <= zero.");
      }

      if (task == null)
      {
        throw new ArgumentNullException(nameof(task));
      }

      Log.Debug("Scheduled Repeating Task: {TaskName}", task.Method.GetFullName());
      ScheduledItem item = new ScheduledItem(task, loopTimeService.Time + delay.TotalSeconds + schedule.TotalSeconds, schedule.TotalSeconds);
      scheduledItems.InsertOrdered(item, comparer);
      return item;
    }

    void IUpdateable.Update()
    {
      int i;
      for (i = 0; i < scheduledItems.Count; i++)
      {
        ScheduledItem item = scheduledItems[i];
        if (loopTimeService.Time < item.ExecutionTime)
        {
          break;
        }

        if (item.Disposed)
        {
          continue;
        }

        try
        {
          item.Execute();
        }
        catch (Exception e)
        {
          Log.Error(e);
        }

        if (!item.Repeating || item.Disposed)
        {
          continue;
        }

        item.Reschedule(loopTimeService.Time + item.Schedule);
        scheduledItems.RemoveAt(i);
        scheduledItems.InsertOrdered(item, comparer);
        i--;
      }

      if (i > 0)
      {
        scheduledItems.RemoveRange(0, i);
      }
    }

    internal void Unschedule(ScheduledItem scheduledItem)
    {
      scheduledItems.Remove(scheduledItem);
    }
  }
}
