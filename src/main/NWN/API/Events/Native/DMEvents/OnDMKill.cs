namespace NWN.API.Events
{
  public sealed class OnDMKill : IEvent, IDMGroupTargetEvent
  {
    public NwGameObject[] Targets { get; init; }

    public NwPlayer DungeonMaster { get; init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context => DungeonMaster?.LoginCreature;
  }
}
