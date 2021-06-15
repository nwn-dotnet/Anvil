namespace NWN.API.Events
{
  public sealed class OnDMSpawnObject : IEvent
  {
    public NwGameObject SpawnedObject { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context => DungeonMaster?.LoginCreature;
  }
}
