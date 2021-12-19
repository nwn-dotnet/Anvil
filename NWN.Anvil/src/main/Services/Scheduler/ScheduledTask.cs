using System;
using Action = System.Action;

namespace Anvil.Services
{
  public sealed partial class ScheduledTask : IDisposable
  {
    /// <summary>
    /// Gets if this is a repeating scheduled task.
    /// </summary>
    public readonly bool Repeating;

    /// <summary>
    /// Gets the interval for repeating this task if <see cref="Repeating"/> is set to true.
    /// </summary>
    public readonly TimeSpan Schedule;

    private readonly Action task;

    internal ScheduledTask(Action task, TimeSpan executionTime)
    {
      this.task = task;
      ExecutionTime = executionTime;
      Repeating = false;
    }

    internal ScheduledTask(Action task, TimeSpan executionTime, TimeSpan schedule)
    {
      this.task = task;
      ExecutionTime = executionTime;
      Schedule = schedule;
      Repeating = true;
    }

    /// <summary>
    /// Gets the number of times this task has successfully executed.
    /// </summary>
    public int ExecutionCount { get; internal set; }

    /// <summary>
    /// Gets the <see cref="API.Time.TimeSinceStartup"/> that this task was last executed/will next be executed at.
    /// </summary>
    public TimeSpan ExecutionTime { get; internal set; }

    /// <summary>
    /// Gets the number of times this task has failed to execute because of an exception.
    /// </summary>
    public int FailedExecutionCount { get; internal set; }

    /// <summary>
    /// Gets if this task has been cancelled for execution.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Cancels the execution of this task.
    /// </summary>
    public void Cancel()
    {
      IsCancelled = true;
    }

    /// <summary>
    /// <inheritdoc cref="Cancel"/>
    /// </summary>
    public void Dispose()
    {
      Cancel();
    }

    internal void Execute()
    {
      task();
    }
  }
}
