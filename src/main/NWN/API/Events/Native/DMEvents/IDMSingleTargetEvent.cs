namespace NWN.API.Events
{
  public interface IDMSingleTargetEvent
  {
    public NwGameObject Target { get; init; }

    public NwPlayer DungeonMaster { get; init; }

    public bool Skip { get; set; }
  }
}
