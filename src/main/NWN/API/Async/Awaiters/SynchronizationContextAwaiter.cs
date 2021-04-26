using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NWN.API
{
  public sealed class SynchronizationContextAwaiter : INotifyCompletion
  {
    private static readonly SendOrPostCallback postCallback = state => ((Action)state)?.Invoke();

    private readonly SynchronizationContext context;

    public SynchronizationContextAwaiter(SynchronizationContext context)
    {
      this.context = context;
    }

    public bool IsCompleted
    {
      get => context == SynchronizationContext.Current;
    }

    public void OnCompleted(Action continuation)
    {
      context.Post(postCallback, continuation);
    }

    public void GetResult() {}
  }
}
