using NWN.API.Constants;

namespace NWN.API.Events
{
  public sealed class OnDMGiveAlignment : IEvent
  {
    public Alignment Alignment { get; internal init; }

    public int Amount { get; internal init; }

    public NwObject Target { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }
}
