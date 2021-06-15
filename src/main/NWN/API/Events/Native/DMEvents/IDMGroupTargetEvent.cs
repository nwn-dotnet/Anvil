namespace NWN.API.Events
{
  public interface IDMGroupTargetEvent
  {
    public NwGameObject[] Targets { get; init; }

    public NwPlayer DungeonMaster { get; init; }

    public bool Skip { get; set; }
  }
}
