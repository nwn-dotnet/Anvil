using NWN.API.Constants;

namespace NWN.API.Events
{
  public static class AreaOfEffectEvents
  {
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

    [ScriptEvent(EventScriptType.AreaOfEffectOnObjectEnter)]
    public sealed class OnEnter : Event<NwAreaOfEffect, OnEnter>
    {
      public NwAreaOfEffect Effect { get; private set; }

      protected override void PrepareEvent(NwAreaOfEffect objSelf)
      {
        Effect = objSelf;
      }
    }

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