using System;
using System.Threading.Tasks;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Door, ObjectType.Door)]
  public sealed class NwDoor : NwStationary
  {
    internal readonly CNWSDoor Door;

    internal NwDoor(uint objectId, CNWSDoor door) : base(objectId, door)
    {
      this.Door = door;
    }

    public static implicit operator CNWSDoor(NwDoor door)
    {
      return door?.Door;
    }

    public event Action<DoorEvents.OnOpen> OnOpen
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnClose> OnClose
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnDamaged> OnDamaged
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnDeath> OnDeath
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnDisarm> OnDisarm
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnHeartbeat> OnHeartbeat
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnLock> OnLock
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnPhysicalAttacked> OnPhysicalAttacked
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnSpellCastAt> OnSpellCastAt
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnTrapTriggered> OnTrapTriggered
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnUnlock> OnUnlock
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnUserDefined> OnUserDefined
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnAreaTransitionClick> OnAreaTransitionClick
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnDialogue> OnDialogue
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<DoorEvents.OnFailToOpen> OnFailToOpen
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public override Location Location
    {
      set
      {
        Door.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z);
        Rotation = value.Rotation;
      }
    }

    /// <summary>
    /// Opens this door.
    /// </summary>
    public async Task Open()
    {
      await WaitForObjectContext();
      NWScript.ActionOpenDoor(this);
    }

    /// <summary>
    /// Closes this door.
    /// </summary>
    public async Task Close()
    {
      await WaitForObjectContext();
      NWScript.ActionCloseDoor(this);
    }

    /// <summary>
    /// Determines whether the specified action can be performed on this door.
    /// </summary>
    /// <param name="action">The action to check.</param>
    /// <returns>true if the specified action can be performed, otherwise false.</returns>
    public bool IsDoorActionPossible(DoorAction action)
      => NWScript.GetIsDoorActionPossible(this, (int)action).ToBool();
  }
}
