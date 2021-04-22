using System;

namespace NWN.API
{
  public static class AwaiterExtensions
  {
    public static CompletionSourceAwaiter GetAwaiter(this Func<bool> completionSource)
    {
      return new CompletionSourceAwaiter(completionSource);
    }
  }
}
