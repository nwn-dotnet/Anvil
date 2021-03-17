using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
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

      NwObject IEvent.Context => Effect;
    }

    [GameEvent(EventScriptType.AreaOfEffectOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();

      NwObject IEvent.Context => Effect;
    }

    /// <summary>
    /// Called when an object enters the area of effect.
    /// </summary>
    [GameEvent(EventScriptType.AreaOfEffectOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();

      NwObject IEvent.Context => Effect;
    }

    /// <summary>
    /// Called when an object exits the area of effect.
    /// </summary>
    [GameEvent(EventScriptType.AreaOfEffectOnObjectExit)]
    public sealed class OnExit : IEvent
    {
      public NwAreaOfEffect Effect { get; } = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();

      NwObject IEvent.Context => Effect;
    }
  }
}
