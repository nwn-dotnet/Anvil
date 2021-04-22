using System;
using System.Runtime.CompilerServices;

namespace NWN.API
{
  public readonly struct CompletionSourceAwaiter : INotifyCompletion
  {
    private readonly Func<bool> completionSource;

    public bool IsCompleted
    {
      get => completionSource();
    }

    public CompletionSourceAwaiter(Func<bool> completionSource)
    {
      this.completionSource = completionSource;
    }

    public void OnCompleted(Action continuation)
    {
      if (continuation != null)
      {
        NwTask.Scheduler.RunInSyncContext(continuation);
      }
    }

    public bool GetResult()
    {
      return completionSource();
    }
  }
}
