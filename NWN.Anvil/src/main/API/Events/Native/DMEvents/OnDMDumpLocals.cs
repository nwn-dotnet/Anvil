using System;
using Anvil.API.Events;

namespace Anvil.API.Events
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

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMDumpLocals"/>
    public event Action<OnDMDumpLocals> OnDMDumpLocals
    {
      add => EventService.Subscribe<OnDMDumpLocals, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMDumpLocals, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMDumpLocals"/>
    public event Action<OnDMDumpLocals> OnDMDumpLocals
    {
      add => EventService.SubscribeAll<OnDMDumpLocals, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMDumpLocals, DMEventFactory>(value);
    }
  }
}
