using Anvil.API;

namespace Anvil.Services
{
  public sealed class OnELCCustomCheck
  {
    public bool IsFailed { get; set; }
    public NwPlayer Player { get; internal init; }
  }
}
