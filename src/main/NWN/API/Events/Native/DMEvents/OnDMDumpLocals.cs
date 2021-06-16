namespace NWN.API.Events
{
  public sealed class OnDMDumpLocals : IEvent
  {
    public DumpLocalsType Type { get; internal init; }

    public NwObject Target { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }
}
