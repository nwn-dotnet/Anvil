using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  public static partial class AreaEvents
  {
    /// <summary>
    /// Called when a new <see cref="NwGameObject"/> has entered the <see cref="NwArea"/>.
    /// </summary>
    [GameEvent(EventScriptType.AreaOnEnter)]
    public sealed class OnEnter : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwArea"/> that was entered.
      /// </summary>
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that entered the <see cref="NwArea"/>.
      /// </summary>
      public NwGameObject EnteringObject { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => Area;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwArea
  {
    /// <inheritdoc cref="AreaEvents.OnEnter"/>
    public event Action<AreaEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<AreaEvents.OnEnter, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaEvents.OnEnter, GameEventFactory>(this, value);
    }
  }
}
