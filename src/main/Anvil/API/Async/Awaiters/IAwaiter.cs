using System.Runtime.CompilerServices;

namespace Anvil.API
{
  public interface IAwaiter : INotifyCompletion
  {
    public bool IsCompleted { get; }

    public void GetResult() {}
  }
}
