using Anvil.API;

namespace Anvil.Services
{
  public sealed class OnELCCustomCheck
  {
    public NwPlayer Player { get; internal init; }

    public bool IsFailed { get; set; }
  }
}
