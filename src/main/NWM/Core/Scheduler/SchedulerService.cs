using System;
using System.Collections.Generic;
using NLog;
using NWM.API;

namespace NWM.Core
{
  [Service(typeof(IUpdateable), IsCollection = true)]
  public class SchedulerService : IUpdateable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    public static readonly TimeSpan NextUpdate = TimeSpan.Zero;

    // Dependencies
    private readonly LoopTimeService loopTimeService;

    private readonly List<ScheduledItem> scheduledItems = new List<ScheduledItem>(1024);
    private readonly IComparer<ScheduledItem> comparer = new ScheduledItem.SortedByExecutionTime();

    public SchedulerService(LoopTimeService loopTimeService)
    {
      this.loopTimeService = loopTimeService;
    }

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

      Log.Debug($"Scheduled Future Task: {task.Method.GetFullName()}");
      ScheduledItem item = new ScheduledItem(this, task, loopTimeService.Time + delay.TotalSeconds);
      scheduledItems.InsertOrdered(item, comparer);
      return item;
    }

    public IDisposable ScheduleRepeating(string name, Action task, TimeSpan schedule, TimeSpan delay = default)
    {
      if (schedule <= TimeSpan.Zero)
      {
        throw new ArgumentOutOfRangeException(nameof(delay), $"{nameof(delay)} cannot be <= zero.");
      }
      if (task == null)
      {
        throw new ArgumentNullException(nameof(task));
      }

      Log.Debug($"Scheduled Repeating Task: {name}");
      ScheduledItem item = new ScheduledItem(this, task, loopTimeService.Time + delay.TotalSeconds + schedule.TotalSeconds, schedule.TotalSeconds);
      scheduledItems.InsertOrdered(item, comparer);
      return item;
    }

    internal void Unschedule(ScheduledItem scheduledItem)
    {
      scheduledItems.Remove(scheduledItem);
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

        item.Execute();
        if (!item.Repeating)
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
  }
}