using NWN.API;

namespace NWN.Services
{
  public sealed class OnELCCustomCheck
  {
    public NwPlayer Player { get; internal init; }

    public bool IsFailed { get; set; }
  }
}
