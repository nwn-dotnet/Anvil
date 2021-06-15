namespace NWN.API.Events
{
  public class OnDMPossessFullPower : IEvent, IDMSingleTargetEvent
  {
    public NwGameObject Target { get; init; }

    public NwPlayer DungeonMaster { get; init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }
}
