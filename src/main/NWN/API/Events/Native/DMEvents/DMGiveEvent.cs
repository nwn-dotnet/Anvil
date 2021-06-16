namespace NWN.API.Events
{
  public abstract class DMGiveEvent : IEvent
  {
    public NwGameObject Target { get; internal init; }

    public int Amount { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }

  public sealed class OnDMGiveXP : DMGiveEvent {}

  public sealed class OnDMGiveLevel : DMGiveEvent {}

  public sealed class OnDMGiveGold : DMGiveEvent {}
}
