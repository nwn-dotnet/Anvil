namespace NWN.API.Events
{
  public abstract class DMStandardEvent : IEvent
  {
    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }

  public sealed class OnDMAppear : DMStandardEvent {}

  public sealed class OnDMDisappear : DMStandardEvent {}

  public sealed class OnDMSetFaction : DMStandardEvent {}

  public sealed class OnDMTakeItem : DMStandardEvent {}

  public sealed class OnDMSetStat : DMStandardEvent {}

  public sealed class OnDMGetVariable : DMStandardEvent {}

  public sealed class OnDMSetVariable : DMStandardEvent {}

  public sealed class OnDMSetTime : DMStandardEvent {}

  public sealed class OnDMSetDate : DMStandardEvent {}

  public sealed class OnDMSetFactionReputation : DMStandardEvent {}

  public sealed class OnDMGetFactionReputation : DMStandardEvent {}

  public sealed class OnDMPlayerDMLogout : DMStandardEvent {}
}
