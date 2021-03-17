using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for door objects.
  /// </summary>
  public static class DoorEvents
  {
    [NativeEvent(EventScriptType.DoorOnOpen)]
    public sealed class OnOpen : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnOpen()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnClose)]
    public sealed class OnClose : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnClose()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnDamage)]
    public sealed class OnDamaged : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnDamaged()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnDeath)]
    public sealed class OnDeath : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnDeath()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnDisarm)]
    public sealed class OnDisarm : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnDisarm()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnHeartbeat()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnLock)]
    public sealed class OnLock : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnLock()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnPhysicalAttacked()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the door targeted by this spell.
      /// </summary>
      public NwDoor Door { get; }

      /// <summary>
      /// Gets the caster of this spell (creature, placeable, door). Returns null from an area of effect.
      /// </summary>
      public NwGameObject Caster { get; }

      /// <summary>
      /// Gets the spell that was cast.
      /// </summary>
      public Spell Spell { get; }

      /// <summary>
      /// Gets a value indicating whether this spell is considered harmful.
      /// </summary>
      public bool Harmful { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnSpellCastAt()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
        Caster = NWScript.GetLastSpellCaster().ToNwObject<NwGameObject>();
        Spell = (Spell)NWScript.GetLastSpell();
        Harmful = NWScript.GetLastSpellHarmful().ToBool();
      }
    }

    [NativeEvent(EventScriptType.DoorOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnTrapTriggered()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnUnlock)]
    public sealed class OnUnlock : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnUnlock()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnUserDefined)]
    public sealed class OnUserDefined : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnUserDefined()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnClicked)]
    public sealed class OnAreaTransitionClick : IEvent
    {
      public NwDoor Door { get; }

      public NwPlayer ClickedBy { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnAreaTransitionClick()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
        ClickedBy = NWScript.GetClickingObject().ToNwObject<NwPlayer>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnDialogue)]
    public sealed class OnDialogue : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnDialogue()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }

    [NativeEvent(EventScriptType.DoorOnFailToOpen)]
    public sealed class OnFailToOpen : IEvent
    {
      public NwDoor Door { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Door;

      public OnFailToOpen()
      {
        Door = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();
      }
    }
  }
}
