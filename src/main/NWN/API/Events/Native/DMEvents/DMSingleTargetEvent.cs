namespace NWN.API.Events
{
  public abstract class DMSingleTargetEvent : IEvent
  {
    public NwGameObject Target { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }

  public sealed class OnDMGoTo : DMSingleTargetEvent {}

  public sealed class OnDMPossess : DMSingleTargetEvent {}

  public sealed class OnDMPossessFullPower : DMSingleTargetEvent {}

  public sealed class OnDMToggleLock : DMSingleTargetEvent {}

  public sealed class OnDMDisableTrap : DMSingleTargetEvent {}
}
