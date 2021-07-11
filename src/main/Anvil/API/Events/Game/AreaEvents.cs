using System;
using Anvil.API;
using NWN.API.Events;
using NWN.Core;

namespace NWN.API.Events
{
  /// <summary>
  /// Events for <see cref="NwArea"/>.
  /// </summary>
  public static class AreaEvents
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

      NwObject IEvent.Context
      {
        get => Area;
      }
    }

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

    /// <summary>
    /// Called at a regular interval (approx. 6 seconds).
    /// </summary>
    [GameEvent(EventScriptType.AreaOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>();

      NwObject IEvent.Context
      {
        get => Area;
      }
    }

    [GameEvent(EventScriptType.AreaOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>();

      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context
      {
        get => Area;
      }

      public static void Signal(NwArea area, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(area, nwEvent);
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwArea
  {
    /// <inheritdoc cref="NWN.API.Events.AreaEvents.OnEnter"/>
    public event Action<AreaEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<AreaEvents.OnEnter, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaEvents.OnEnter, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.AreaEvents.OnExit"/>
    public event Action<AreaEvents.OnExit> OnExit
    {
      add => EventService.Subscribe<AreaEvents.OnExit, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaEvents.OnExit, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.AreaEvents.OnHeartbeat"/>
    public event Action<AreaEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<AreaEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaEvents.OnHeartbeat, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.AreaEvents.OnUserDefined"/>
    public event Action<AreaEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<AreaEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
