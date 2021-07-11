using System;
using Anvil.API;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;

namespace NWN.API.Events
{
  /// <summary>
  /// Events for effects created with <see cref="Effect.AreaOfEffect"/>.
  /// </summary>
  public static class AreaOfEffectEvents
  {
    /// <summary>
    /// Called at a regular interval (approx. 6 seconds).
    /// </summary>
    [GameEvent(EventScriptType.AreaOfEffectOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();

      NwObject IEvent.Context
      {
        get => Effect;
      }
    }

    [GameEvent(EventScriptType.AreaOfEffectOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();

      NwObject IEvent.Context
      {
        get => Effect;
      }

      public static void Signal(NwAreaOfEffect areaOfEffect, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(areaOfEffect, nwEvent);
      }
    }

    /// <summary>
    /// Called when an object enters the area of effect.
    /// </summary>
    [GameEvent(EventScriptType.AreaOfEffectOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();

      public NwGameObject Entering { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context
      {
        get => Effect;
      }
    }

    /// <summary>
    /// Called when an object exits the area of effect.
    /// </summary>
    [GameEvent(EventScriptType.AreaOfEffectOnObjectExit)]
    public sealed class OnExit : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();

      public NwGameObject Exiting { get; } = NWScript.GetExitingObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context
      {
        get => Effect;
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwAreaOfEffect
  {
    /// <inheritdoc cref="NWN.API.Events.AreaOfEffectEvents.OnEnter"/>
    public event Action<AreaOfEffectEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnEnter, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnEnter, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.AreaOfEffectEvents.OnExit"/>
    public event Action<AreaOfEffectEvents.OnExit> OnExit
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnExit, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnExit, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.AreaOfEffectEvents.OnHeartbeat"/>
    public event Action<AreaOfEffectEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnHeartbeat, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.AreaOfEffectEvents.OnUserDefined"/>
    public event Action<AreaOfEffectEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
