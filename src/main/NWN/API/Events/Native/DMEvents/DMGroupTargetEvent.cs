namespace NWN.API.Events
{
  public abstract class DMGroupTargetEvent : IEvent
  {
    public NwGameObject[] Targets { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }

  public sealed class OnDMHeal : DMGroupTargetEvent {}

  public sealed class OnDMKill : DMGroupTargetEvent {}

  public sealed class OnDMForceRest : DMGroupTargetEvent {}

  public sealed class OnDMToggleInvulnerable : DMGroupTargetEvent {}

  public sealed class OnDMLimbo : DMGroupTargetEvent {}

  public sealed class OnDMToggleAI : DMGroupTargetEvent {}

  public sealed class OnDMToggleImmortal : DMGroupTargetEvent {}
}
