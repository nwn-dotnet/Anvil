using NWN.API.Constants;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for door objects.
  /// </summary>
  public static class DoorEvents
  {
    [NativeEvent(EventScriptType.DoorOnOpen)]
    public sealed class OnOpen : NativeEvent<NwDoor, OnOpen>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnClose)]
    public sealed class OnClose : NativeEvent<NwDoor, OnClose>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnDamage)]
    public sealed class OnDamaged : NativeEvent<NwDoor, OnDamaged>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnDeath)]
    public sealed class OnDeath : NativeEvent<NwDoor, OnDeath>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnDisarm)]
    public sealed class OnDisarm : NativeEvent<NwDoor, OnDisarm>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnHeartbeat)]
    public sealed class OnHeartbeat : NativeEvent<NwDoor, OnHeartbeat>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnLock)]
    public sealed class OnLock : NativeEvent<NwDoor, OnLock>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : NativeEvent<NwDoor, OnPhysicalAttacked>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnSpellCastAt)]
    public sealed class OnSpellCastAt : NativeEvent<NwDoor, OnSpellCastAt>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnTrapTriggered)]
    public sealed class OnTrapTriggered : NativeEvent<NwDoor, OnTrapTriggered>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnUnlock)]
    public sealed class OnUnlock : NativeEvent<NwDoor, OnUnlock>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnUserDefined)]
    public sealed class OnUserDefined : NativeEvent<NwDoor, OnUserDefined>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnClicked)]
    public sealed class OnAreaTransitionClick : NativeEvent<NwDoor, OnAreaTransitionClick>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnDialogue)]
    public sealed class OnDialogue : NativeEvent<NwDoor, OnDialogue>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }

    [NativeEvent(EventScriptType.DoorOnFailToOpen)]
    public sealed class OnFailToOpen : NativeEvent<NwDoor, OnFailToOpen>
    {
      public NwDoor Door { get; private set; }

      protected override void PrepareEvent(NwDoor objSelf)
      {
        Door = objSelf;
      }
    }
  }
}
