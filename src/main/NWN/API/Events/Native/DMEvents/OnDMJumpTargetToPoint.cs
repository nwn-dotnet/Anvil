using System.Numerics;

namespace NWN.API.Events
{
  public class OnDMJumpTargetToPoint : IEvent
  {
    public NwArea NewArea { get; init; }

    public Vector3 NewPosition { get; init; }

    public NwGameObject[] Targets { get; init; }

    public NwPlayer DungeonMaster { get; init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }
}
