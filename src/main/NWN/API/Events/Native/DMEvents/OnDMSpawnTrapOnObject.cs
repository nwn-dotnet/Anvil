namespace NWN.API.Events
{
  public sealed class OnDMSpawnTrapOnObject : IEvent
  {
    public NwPlayer DungeonMaster { get; internal init; }

    public NwStationary Target { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context => DungeonMaster?.LoginCreature;
  }
}
