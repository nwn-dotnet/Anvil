using System.Runtime.CompilerServices;

namespace Anvil.API
{
  /// <summary>
  /// Explicit interface for an awaiter. An awaiter is a system that will run an async operation and schedule a continuation when it completes.
  /// </summary>
  public interface IAwaiter : INotifyCompletion
  {
    bool IsCompleted { get; }

    void GetResult();
  }

  /// <summary>
  /// Explicit interface for an awaiter. An awaiter is a system that will run an async operation and schedule a continuation when it completes.
  /// </summary>
  /// <typeparam name="TResult">The awaiter return value type.</typeparam>
  public interface IAwaiter<out TResult> : INotifyCompletion
  {
    bool IsCompleted { get; }

    TResult GetResult();
  }
}
