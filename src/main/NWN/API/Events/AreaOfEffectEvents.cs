using NWN.API.Constants;

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
    [ScriptEvent(EventScriptType.AreaOfEffectOnHeartbeat)]
    public sealed class OnHeartbeat : Event<NwAreaOfEffect, OnHeartbeat>
    {
      public NwAreaOfEffect Effect { get; private set; }

      protected override void PrepareEvent(NwAreaOfEffect objSelf)
      {
        Effect = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.AreaOfEffectOnUserDefinedEvent)]
    public sealed class OnUserDefined : Event<NwAreaOfEffect, OnUserDefined>
    {
      public NwAreaOfEffect Effect { get; private set; }

      protected override void PrepareEvent(NwAreaOfEffect objSelf)
      {
        Effect = objSelf;
      }
    }

    /// <summary>
    /// Called when an object enters the area of effect.
    /// </summary>
    [ScriptEvent(EventScriptType.AreaOfEffectOnObjectEnter)]
    public sealed class OnEnter : Event<NwAreaOfEffect, OnEnter>
    {
      public NwAreaOfEffect Effect { get; private set; }

      protected override void PrepareEvent(NwAreaOfEffect objSelf)
      {
        Effect = objSelf;
      }
    }

    /// <summary>
    /// Called when an object exits the area of effect.
    /// </summary>
    [ScriptEvent(EventScriptType.AreaOfEffectOnObjectExit)]
    public sealed class OnExit : Event<NwAreaOfEffect, OnExit>
    {
      public NwAreaOfEffect Effect { get; private set; }

      protected override void PrepareEvent(NwAreaOfEffect objSelf)
      {
        Effect = objSelf;
      }
    }
  }
}
