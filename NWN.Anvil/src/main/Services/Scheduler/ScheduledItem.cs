using System;
using System.Collections.Generic;

namespace Anvil.Services
{
  internal sealed class ScheduledItem : IDisposable
  {
    public readonly bool Repeating;
    public readonly double Schedule;
    private readonly Action task;

    public ScheduledItem(Action task, double executionTime)
    {
      this.task = task;
      ExecutionTime = executionTime;
      Repeating = false;
    }

    public ScheduledItem(Action task, double executionTime, double schedule)
    {
      this.task = task;
      ExecutionTime = executionTime;
      Schedule = schedule;
      Repeating = true;
    }

    public bool Disposed { get; private set; }

    public double ExecutionTime { get; private set; }

    public void Dispose()
    {
      Disposed = true;
    }

    public void Execute()
    {
      task();
    }

    public void Reschedule(double newTime)
    {
      ExecutionTime = newTime;
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
