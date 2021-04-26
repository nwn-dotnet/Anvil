using System.Threading;

namespace NWN.API
{
  public static class AwaiterExtensions
  {
    public static SynchronizationContextAwaiter GetAwaiter(this SynchronizationContext context)
    {
      return new SynchronizationContextAwaiter(context);
    }
  }
}
