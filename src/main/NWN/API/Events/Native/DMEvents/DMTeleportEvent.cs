using System.Numerics;

namespace NWN.API.Events
{
  public abstract class DMTeleportEvent : IEvent
  {
    public NwArea TargetArea { get; internal init; }

    public Vector3 TargetPosition { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }

  public sealed class OnDMJumpToPoint : DMTeleportEvent {}

  public sealed class OnDMJumpAllPlayersToPoint : DMTeleportEvent {}
}
