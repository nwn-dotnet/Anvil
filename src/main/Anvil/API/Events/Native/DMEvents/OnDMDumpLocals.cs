using System;
using NWN.API.Events;

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

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMDumpLocals"/>
    public event Action<OnDMDumpLocals> OnDMDumpLocals
    {
      add => EventService.Subscribe<OnDMDumpLocals, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMDumpLocals, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMDumpLocals"/>
    public event Action<OnDMDumpLocals> OnDMDumpLocals
    {
      add => EventService.SubscribeAll<OnDMDumpLocals, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMDumpLocals, DMEventFactory>(value);
    }
  }
}
