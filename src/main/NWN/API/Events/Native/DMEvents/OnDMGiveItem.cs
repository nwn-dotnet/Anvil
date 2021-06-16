namespace NWN.API.Events
{
  public sealed class OnDMGiveItem : IEvent
  {
    public NwGameObject Target { get; internal init; }

    public NwItem Item { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }
}
