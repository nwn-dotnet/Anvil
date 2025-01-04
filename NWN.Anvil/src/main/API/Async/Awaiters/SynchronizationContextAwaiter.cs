using System.Threading;

namespace Anvil.API
{
  /// <summary>
  /// Awaits for the current async context to return to the specified context.
  /// </summary>
  internal readonly struct SynchronizationContextAwaiter(SynchronizationContext context) : IAwaiter
  {
    private static readonly SendOrPostCallback PostCallback = state => ((System.Action?)state)?.Invoke();

    public bool IsCompleted => context == SynchronizationContext.Current;

    public void GetResult() {}

    public void OnCompleted(System.Action continuation)
    {
      context.Post(PostCallback, continuation);
    }
  }
}
