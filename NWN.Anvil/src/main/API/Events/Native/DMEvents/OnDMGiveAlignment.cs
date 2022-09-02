using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public sealed class OnDMGiveAlignment : DMEvent
  {
    public Alignment Alignment { get; internal init; }

    public int Amount { get; internal init; }

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
