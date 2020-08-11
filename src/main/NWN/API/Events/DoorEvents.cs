using NWN.API.Constants;

namespace NWN.API.Events
{
  // TODO Populate event data.
  /// <summary>
  /// Events for door objects.
  /// </summary>
  public static class DoorEvents
  {
    [ScriptEvent(EventScriptType.DoorOnOpen)]
    public sealed class OnOpen : Event<NwDoor, OnOpen>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnClose)]
    public sealed class OnClose : Event<NwDoor, OnClose>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnDamage)]
    public sealed class OnDamaged : Event<NwDoor, OnDamaged>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnDeath)]
    public sealed class OnDeath : Event<NwDoor, OnDeath>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnDisarm)]
    public sealed class OnDisarm : Event<NwDoor, OnDisarm>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnHeartbeat)]
    public sealed class OnHeartbeat : Event<NwDoor, OnHeartbeat>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnLock)]
    public sealed class OnLock : Event<NwDoor, OnLock>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : Event<NwDoor, OnPhysicalAttacked>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnSpellCastAt)]
    public sealed class OnSpellCastAt : Event<NwDoor, OnSpellCastAt>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnTrapTriggered)]
    public sealed class OnTrapTriggered : Event<NwDoor, OnTrapTriggered>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnUnlock)]
    public sealed class OnUnlock : Event<NwDoor, OnUnlock>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnUserDefined)]
    public sealed class OnUserDefined : Event<NwDoor, OnUserDefined>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnClicked)]
    public sealed class OnAreaTransitionClick : Event<NwDoor, OnAreaTransitionClick>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnDialogue)]
    public sealed class OnDialogue : Event<NwDoor, OnDialogue>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.DoorOnFailToOpen)]
    public sealed class OnFailToOpen : Event<NwDoor, OnFailToOpen>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }
  }
}