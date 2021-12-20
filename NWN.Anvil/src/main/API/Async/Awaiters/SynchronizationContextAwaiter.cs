using System.Threading;

namespace Anvil.API
{
  public readonly struct SynchronizationContextAwaiter : IAwaiter
  {
    private static readonly SendOrPostCallback PostCallback = state => ((System.Action)state)?.Invoke();

    private readonly SynchronizationContext context;

    public SynchronizationContextAwaiter(SynchronizationContext context)
    {
      this.context = context;
    }

    public bool IsCompleted => context == SynchronizationContext.Current;

    public void GetResult() {}

    public void OnCompleted(System.Action continuation)
    {
      context.Post(PostCallback, continuation);
    }
  }
}
