using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a DM adjusts a target's alignment.
  /// </summary>
  public sealed class OnDMGiveAlignment : DMEvent
  {
    /// <summary>
    /// Gets the alignment axis being adjusted.
    /// </summary>
    public Alignment Alignment { get; internal init; }

    /// <summary>
    /// Gets the amount to adjust along the alignment axis.
    /// </summary>
    public int Amount { get; internal init; }

    /// <summary>
    /// Gets the object whose alignment is being adjusted.
    /// </summary>
    public NwObject Target { get; internal init; } = null!;
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMGiveAlignment"/>
    public event Action<OnDMGiveAlignment> OnDMGiveAlignment
    {
      add => EventService.Subscribe<OnDMGiveAlignment, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMGiveAlignment, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMGiveAlignment"/>
    public event Action<OnDMGiveAlignment> OnDMGiveAlignment
    {
      add => EventService.SubscribeAll<OnDMGiveAlignment, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMGiveAlignment, DMEventFactory>(value);
    }
  }
}
