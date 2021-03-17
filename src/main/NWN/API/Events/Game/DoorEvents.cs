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
    [GameEvent(EventScriptType.DoorOnOpen)]
    public sealed class OnOpen : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnClose)]
    public sealed class OnClose : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnDamage)]
    public sealed class OnDamaged : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnDeath)]
    public sealed class OnDeath : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnDisarm)]
    public sealed class OnDisarm : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnLock)]
    public sealed class OnLock : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnMeleeAttacked)]
    public sealed class OnPhysicalAttacked : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnSpellCastAt)]
    public sealed class OnSpellCastAt : IEvent
    {
      /// <summary>
      /// Gets the door targeted by this spell.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      /// <summary>
      /// Gets the caster of this spell (creature, placeable, door). Returns null from an area of effect.
      /// </summary>
      public NwGameObject Caster { get; } = NWScript.GetLastSpellCaster().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the spell that was cast.
      /// </summary>
      public Spell Spell { get; } = (Spell)NWScript.GetLastSpell();

      /// <summary>
      /// Gets a value indicating whether this spell is considered harmful.
      /// </summary>
      public bool Harmful { get; } = NWScript.GetLastSpellHarmful().ToBool();

      NwObject IEvent.Context => Door;

      public static void Signal(NwObject caster, NwDoor target, Spell spell, bool harmful = true)
      {
        Event nwEvent = NWScript.EventSpellCastAt(caster, (int)spell, harmful.ToInt());
        NWScript.SignalEvent(target, nwEvent);
      }
    }

    [GameEvent(EventScriptType.DoorOnTrapTriggered)]
    public sealed class OnTrapTriggered : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnUnlock)]
    public sealed class OnUnlock : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnUserDefined)]
    public sealed class OnUserDefined : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnClicked)]
    public sealed class OnAreaTransitionClick : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      public NwPlayer ClickedBy { get; } = NWScript.GetClickingObject().ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnDialogue)]
    public sealed class OnDialogue : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }

    [GameEvent(EventScriptType.DoorOnFailToOpen)]
    public sealed class OnFailToOpen : IEvent
    {
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>();

      NwObject IEvent.Context => Door;
    }
  }
}
