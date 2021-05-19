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

    internal NwDoor(CNWSDoor door) : base(door)
    {
      this.Door = door;
    }

    public static implicit operator CNWSDoor(NwDoor door)
    {
      return door?.Door;
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnOpen"/>
    public event Action<DoorEvents.OnOpen> OnOpen
    {
      add => EventService.Subscribe<DoorEvents.OnOpen, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnOpen, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnClose"/>
    public event Action<DoorEvents.OnClose> OnClose
    {
      add => EventService.Subscribe<DoorEvents.OnClose, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnClose, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnDamaged"/>
    public event Action<DoorEvents.OnDamaged> OnDamaged
    {
      add => EventService.Subscribe<DoorEvents.OnDamaged, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnDamaged, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnDeath"/>
    public event Action<DoorEvents.OnDeath> OnDeath
    {
      add => EventService.Subscribe<DoorEvents.OnDeath, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnDeath, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnDisarm"/>
    public event Action<DoorEvents.OnDisarm> OnDisarm
    {
      add => EventService.Subscribe<DoorEvents.OnDisarm, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnDisarm, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnHeartbeat"/>
    public event Action<DoorEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<DoorEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnHeartbeat, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnLock"/>
    public event Action<DoorEvents.OnLock> OnLock
    {
      add => EventService.Subscribe<DoorEvents.OnLock, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnLock, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnPhysicalAttacked"/>
    public event Action<DoorEvents.OnPhysicalAttacked> OnPhysicalAttacked
    {
      add => EventService.Subscribe<DoorEvents.OnPhysicalAttacked, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnPhysicalAttacked, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnSpellCastAt"/>
    public event Action<DoorEvents.OnSpellCastAt> OnSpellCastAt
    {
      add => EventService.Subscribe<DoorEvents.OnSpellCastAt, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnSpellCastAt, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnTrapTriggered"/>
    public event Action<DoorEvents.OnTrapTriggered> OnTrapTriggered
    {
      add => EventService.Subscribe<DoorEvents.OnTrapTriggered, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnTrapTriggered, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnUnlock"/>
    public event Action<DoorEvents.OnUnlock> OnUnlock
    {
      add => EventService.Subscribe<DoorEvents.OnUnlock, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnUnlock, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnUserDefined"/>
    public event Action<DoorEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<DoorEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnUserDefined, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnAreaTransitionClick"/>
    public event Action<DoorEvents.OnAreaTransitionClick> OnAreaTransitionClick
    {
      add => EventService.Subscribe<DoorEvents.OnAreaTransitionClick, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnAreaTransitionClick, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnDialogue"/>
    public event Action<DoorEvents.OnDialogue> OnDialogue
    {
      add => EventService.Subscribe<DoorEvents.OnDialogue, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnDialogue, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.DoorEvents.OnFailToOpen"/>
    public event Action<DoorEvents.OnFailToOpen> OnFailToOpen
    {
      add => EventService.Subscribe<DoorEvents.OnFailToOpen, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnFailToOpen, GameEventFactory>(this, value);
    }

    public override Location Location
    {
      set
      {
        if (value.Area != Area)
        {
          Door.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z, true.ToInt());

          // If the door is trapped it needs to be added to the area's trap list for it to be detectable by players.
          if (IsTrapped)
          {
            value.Area.Area.m_pTrapList.Add(this);
          }
        }
        else
        {
          Position = value.Position;
        }

        Rotation = value.Rotation;
      }
    }

    public override bool KeyAutoRemoved
    {
      get => Door.m_bAutoRemoveKey.ToBool();
      set => Door.m_bAutoRemoveKey = value.ToInt();
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

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("UTD", (resGff, resStruct) =>
      {
        Door.SaveObjectState(resGff, resStruct);
        return Door.SaveDoor(resGff, resStruct).ToBool();
      });
    }

    public static NwDoor Deserialize(byte[] serialized)
    {
      CNWSDoor door = null;

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTD"))
        {
          return false;
        }

        door = new CNWSDoor(INVALID);
        if (door.LoadDoor(resGff, resStruct).ToBool())
        {
          door.LoadObjectState(resGff, resStruct);
          GC.SuppressFinalize(door);
          return true;
        }

        door.Dispose();
        return false;
      });

      return result && door != null ? door.ToNwObject<NwDoor>() : null;
    }
  }
}
