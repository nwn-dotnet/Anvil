using System.Collections.Generic;

namespace Anvil.Services
{
  public sealed partial class ScheduledTask
  {
    internal sealed class SortedByExecutionTime : IComparer<ScheduledTask>
    {
      public int Compare(ScheduledTask x, ScheduledTask y)
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
