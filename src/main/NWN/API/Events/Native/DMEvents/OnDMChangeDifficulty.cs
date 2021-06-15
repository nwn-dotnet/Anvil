using NWN.API.Constants;

namespace NWN.API.Events
{
  public sealed class OnDMChangeDifficulty : IEvent
  {
    public GameDifficulty NewDifficulty { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context => DungeonMaster?.LoginCreature;
  }
}
