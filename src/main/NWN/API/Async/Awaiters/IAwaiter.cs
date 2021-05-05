using System.Runtime.CompilerServices;

namespace NWN.API
{
  public interface IAwaiter : INotifyCompletion
  {
    public bool IsCompleted { get; }

    public void GetResult() {}
  }
}
