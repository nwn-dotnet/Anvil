using System;
using System.Collections.Generic;

namespace NWN.Services
{
  internal class ScheduledItem : IDisposable
  {
    private readonly Action task;

    public double ExecutionTime { get; private set; }

    public bool Disposed { get; private set; }

    public readonly bool Repeating;
    public readonly double Schedule;

    public ScheduledItem(Action task, double executionTime)
    {
      this.task = task;
      this.ExecutionTime = executionTime;
      Repeating = false;
    }

    public ScheduledItem(Action task, double executionTime, double schedule)
    {
      this.task = task;
      this.ExecutionTime = executionTime;
      this.Schedule = schedule;
      Repeating = true;
    }

    public void Reschedule(double newTime)
    {
      this.ExecutionTime = newTime;
    }

    public void Execute()
    {
      task();
    }

    public void Dispose()
    {
      Disposed = true;
    }

    public sealed class SortedByExecutionTime : IComparer<ScheduledItem>
    {
      public int Compare(ScheduledItem x, ScheduledItem y)
      {
        if (ReferenceEquals(x, y))
        {
          return 0;
        }

        if (ReferenceEquals(null, y))
        {
          return 1;
        }

        if (ReferenceEquals(null, x))
        {
          return -1;
        }

        return x.ExecutionTime.CompareTo(y.ExecutionTime);
      }
    }
  }
}
