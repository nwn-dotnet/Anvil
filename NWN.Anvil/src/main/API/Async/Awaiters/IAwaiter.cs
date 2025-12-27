using System.Runtime.CompilerServices;

namespace Anvil.API
{
  /// <summary>
  /// Explicit interface for an awaiter. An awaiter is a system that will run an async operation and schedule a continuation when it completes.
  /// </summary>
  public interface IAwaiter : INotifyCompletion
  {
    /// <summary>
    /// Gets whether the asynchronous operation has completed.
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// Ends the await operation, rethrowing any captured exception.
    /// </summary>
    void GetResult();
  }

  /// <summary>
  /// Explicit interface for an awaiter. An awaiter is a system that will run an async operation and schedule a continuation when it completes.
  /// </summary>
  /// <typeparam name="TResult">The awaiter return value type.</typeparam>
  public interface IAwaiter<out TResult> : INotifyCompletion
  {
    /// <summary>
    /// Gets whether the asynchronous operation has completed.
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// Ends the await operation and returns the computed result.
    /// </summary>
    /// <returns>The result value produced by the asynchronous operation.</returns>
    TResult GetResult();
  }
}
