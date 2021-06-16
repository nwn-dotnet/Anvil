namespace NWN.API.Events
{
  public sealed class OnDMPlayerDMLogin : IEvent
  {
    public string Password { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }
}
