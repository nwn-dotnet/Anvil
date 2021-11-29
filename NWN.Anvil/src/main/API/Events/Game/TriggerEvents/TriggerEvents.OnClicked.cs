using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for drawn, world-placed triggers.
  /// </summary>
  public static partial class TriggerEvents
  {
    [GameEvent(EventScriptType.TriggerOnClicked)]
    public sealed class OnClicked : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that clicked this <see cref="NwTrigger"/>.
      /// </summary>
      public NwCreature ClickedBy { get; } = NWScript.GetClickingObject().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the <see cref="NwTrigger"/> that was clicked.
      /// </summary>
      public NwTrigger Trigger { get; } = NWScript.OBJECT_SELF.ToNwObject<NwTrigger>();

      NwObject IEvent.Context
      {
        get => Trigger;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwTrigger
  {
    /// <inheritdoc cref="TriggerEvents.OnClicked"/>
    public event Action<TriggerEvents.OnClicked> OnClicked
    {
      add => EventService.Subscribe<TriggerEvents.OnClicked, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnClicked, GameEventFactory>(this, value);
    }
  }
}
