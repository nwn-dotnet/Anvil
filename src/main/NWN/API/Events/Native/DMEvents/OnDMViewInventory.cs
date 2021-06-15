namespace NWN.API.Events
{
  public sealed class OnDMViewInventory : IEvent
  {
    public bool IsOpening { get; internal init; }

    public NwGameObject Target { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context => DungeonMaster?.LoginCreature;
  }
}
