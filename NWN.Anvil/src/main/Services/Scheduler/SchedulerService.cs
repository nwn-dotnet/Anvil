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
  public sealed class SchedulerService : IUpdateable, IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// The next server loop after the current.
    /// </summary>
    public static readonly TimeSpan NextUpdate = TimeSpan.Zero;

    private readonly IComparer<ScheduledTask> comparer = new ScheduledTask.SortedByExecutionTime();
    private readonly List<ScheduledTask> scheduledTasks = new List<ScheduledTask>(1024);

    /// <summary>
    /// Schedules the specified action to be invoked after the given delay.
    /// </summary>
    /// <param name="action">The task/action to run.</param>
    /// <param name="delay">The delay until the task is run.</param>
    /// <returns>A disposable object representing the scheduled task. Calling Dispose() will prevent the schedule from running.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the delay is less than 0.</exception>
    /// <exception cref="ArgumentNullException">Thrown if no action is specified.</exception>
    public ScheduledTask Schedule(Action action, TimeSpan delay)
    {
      if (delay < TimeSpan.Zero)
      {
        throw new ArgumentOutOfRangeException(nameof(delay), $"{nameof(delay)} cannot be < zero.");
      }

      if (action == null)
      {
        throw new ArgumentNullException(nameof(action));
      }

      Log.Debug("Scheduled Future Task {TaskName}", action.Method.GetFullName());
      ScheduledTask task = new ScheduledTask(action, Time.TimeSinceStartup + delay);
      scheduledTasks.InsertOrdered(task, comparer);

      return task;
    }

    /// <summary>
    /// Schedules the specified task to be invoked on a specified schedule, after an optional delay.
    /// </summary>
    /// <param name="action">The task/action to be run.</param>
    /// <param name="schedule">The delay between invocations.</param>
    /// <param name="delay">An additional delay for the first time run.</param>
    /// <returns>A disposable object representing the scheduled task. Calling Dispose() will cancel the schedule and any future invocations.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the schedule is less than 0.</exception>
    /// <exception cref="ArgumentNullException">Thrown if no action is specified.</exception>
    public ScheduledTask ScheduleRepeating(Action action, TimeSpan schedule, TimeSpan delay = default)
    {
      if (schedule <= TimeSpan.Zero)
      {
        throw new ArgumentOutOfRangeException(nameof(delay), $"{nameof(delay)} cannot be <= zero.");
      }

      if (action == null)
      {
        throw new ArgumentNullException(nameof(action));
      }

      Log.Debug("Scheduled Repeating Task: {TaskName}", action.Method.GetFullName());
      ScheduledTask task = new ScheduledTask(action, Time.TimeSinceStartup + delay + schedule, schedule);
      scheduledTasks.InsertOrdered(task, comparer);

      return task;
    }

    void IDisposable.Dispose()
    {
      foreach (ScheduledTask task in scheduledTasks)
      {
        task.Dispose();
      }

      scheduledTasks.Clear();
      scheduledTasks.TrimExcess();
    }

    void IUpdateable.Update()
    {
      int i;
      for (i = 0; i < scheduledTasks.Count; i++)
      {
        ScheduledTask task = scheduledTasks[i];
        if (Time.TimeSinceStartup < task.ExecutionTime)
        {
          break;
        }

        if (task.IsCancelled)
        {
          continue;
        }

        try
        {
          task.Execute();
          task.ExecutionCount++;
        }
        catch (Exception e)
        {
          task.FailedExecutionCount++;
          Log.Error(e);
        }

        if (!task.Repeating || task.IsCancelled)
        {
          continue;
        }

        task.ExecutionTime = Time.TimeSinceStartup + task.Schedule;

        scheduledTasks.RemoveAt(i);
        scheduledTasks.InsertOrdered(task, comparer);
        i--;
      }

      if (i > 0 && i <= scheduledTasks.Count)
      {
        scheduledTasks.RemoveRange(0, i);
      }
    }

    internal void Unschedule(ScheduledTask scheduledTask)
    {
      scheduledTasks.Remove(scheduledTask);
    }
  }
}
