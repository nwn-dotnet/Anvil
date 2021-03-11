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
    [NativeEvent(EventScriptType.AreaOfEffectOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwAreaOfEffect Effect { get; }

      public OnHeartbeat()
      {
        Effect = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();
      }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Effect;
    }

    [NativeEvent(EventScriptType.AreaOfEffectOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwAreaOfEffect Effect { get; }

      public OnUserDefined()
      {
        Effect = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();
      }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Effect;
    }

    /// <summary>
    /// Called when an object enters the area of effect.
    /// </summary>
    [NativeEvent(EventScriptType.AreaOfEffectOnObjectEnter)]
    public sealed class OnEnter : IEvent
    {
      public NwAreaOfEffect Effect { get; }

      public OnEnter()
      {
        Effect = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();
      }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Effect;
    }

    /// <summary>
    /// Called when an object exits the area of effect.
    /// </summary>
    [NativeEvent(EventScriptType.AreaOfEffectOnObjectExit)]
    public sealed class OnExit : IEvent
    {
      public NwAreaOfEffect Effect { get; }

      public OnExit()
      {
        Effect = NWScript.OBJECT_SELF.ToNwObject<NwAreaOfEffect>();
      }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Effect;
    }
  }
}
