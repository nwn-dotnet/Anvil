using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a DM requests a dump/output of local variables.
  /// </summary>
  public sealed class OnDMDumpLocals : DMEvent
  {
    /// <summary>
    /// Gets the object to dump locals for.
    /// </summary>
    public NwObject Target { get; internal init; } = null!;

    /// <summary>
    /// Gets the dump mode to use.
    /// </summary>
    public DumpLocalsType Type { get; internal init; }
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
