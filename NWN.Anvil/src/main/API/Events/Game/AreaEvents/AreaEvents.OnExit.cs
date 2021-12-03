using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  public static partial class AreaEvents
  {
    /// <summary>
    /// Called when an <see cref="NwGameObject"/> leaves the <see cref="NwArea"/>.
    /// </summary>
    [GameEvent(EventScriptType.AreaOnExit)]
    public sealed class OnExit : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwArea"/> that was left.
      /// </summary>
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that left the <see cref="NwArea"/>.
      /// </summary>
      public NwGameObject ExitingObject { get; } = NWScript.GetExitingObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context
      {
        get => Area;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwArea
  {
    /// <inheritdoc cref="AreaEvents.OnExit"/>
    public event Action<AreaEvents.OnExit> OnExit
    {
      add => EventService.Subscribe<AreaEvents.OnExit, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaEvents.OnExit, GameEventFactory>(this, value);
    }
  }
}
