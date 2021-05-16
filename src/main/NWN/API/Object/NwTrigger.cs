using System;
using System.Collections.Generic;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Trigger, ObjectType.Trigger)]
  public sealed class NwTrigger : NwTrappable
  {
    internal readonly CNWSTrigger Trigger;

    internal NwTrigger(CNWSTrigger trigger) : base(trigger)
    {
      this.Trigger = trigger;
    }

    public static implicit operator CNWSTrigger(NwTrigger trigger)
    {
      return trigger?.Trigger;
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnHeartbeat"/>
    public event Action<TriggerEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<TriggerEvents.OnHeartbeat, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnHeartbeat, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnEnter"/>
    public event Action<TriggerEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<TriggerEvents.OnEnter, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnEnter, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnExit"/>
    public event Action<TriggerEvents.OnExit> OnExit
    {
      add => EventService.Subscribe<TriggerEvents.OnExit, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnExit, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnUserDefined"/>
    public event Action<TriggerEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<TriggerEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnUserDefined, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnTrapTriggered"/>
    public event Action<TriggerEvents.OnTrapTriggered> OnTrapTriggered
    {
      add => EventService.Subscribe<TriggerEvents.OnTrapTriggered, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnTrapTriggered, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnDisarmed"/>
    public event Action<TriggerEvents.OnDisarmed> OnDisarmed
    {
      add => EventService.Subscribe<TriggerEvents.OnDisarmed, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnDisarmed, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.TriggerEvents.OnClicked"/>
    public event Action<TriggerEvents.OnClicked> OnClicked
    {
      add => EventService.Subscribe<TriggerEvents.OnClicked, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<TriggerEvents.OnClicked, GameEventFactory>(this, value);
    }

    public override Location Location
    {
      set
      {
        Trigger.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z, true.ToInt());

        // If the trigger is trapped it needs to be added to the area's trap list for it to be detectable by players.
        if (IsTrapped)
        {
          value.Area.Area.m_pTrapList.Add(this);
        }

        Rotation = value.Rotation;
      }
    }

    /// <summary>
    /// Gets all objects of the given type that are currently in this trigger.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <returns>An enumerable containing all objects currently in the trigger.</returns>
    public IEnumerator<T> GetObjectsInTrigger<T>() where T : NwGameObject
    {
      int objType = (int) GetObjectType<T>();
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != INVALID; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<T>();
      }
    }

    /// <summary>
    /// Gets all objects of the given types that are currently in this trigger.
    /// </summary>
    /// <param name="objectTypes">The types of object to return.</param>
    /// <returns>An enumerable containing all objects currently in the trigger.</returns>
    public IEnumerator<NwGameObject> GetObjectsInTrigger(ObjectTypes objectTypes)
    {
      int objType = (int) objectTypes;
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != INVALID; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<NwGameObject>();
      }
    }

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("UTT", (resGff, resStruct) =>
      {
        Trigger.SaveObjectState(resGff, resStruct);
        return Trigger.SaveTrigger(resGff, resStruct).ToBool();
      });
    }

    public static NwTrigger Deserialize(byte[] serialized)
    {
      CNWSTrigger trigger = null;

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTT"))
        {
          return false;
        }

        trigger = new CNWSTrigger(INVALID);
        if (trigger.LoadTrigger(resGff, resStruct).ToBool())
        {
          trigger.LoadObjectState(resGff, resStruct);
          GC.SuppressFinalize(trigger);
          return true;
        }

        trigger.Dispose();
        return false;
      });

      return result && trigger != null ? trigger.m_idSelf.ToNwObject<NwTrigger>() : null;
    }
  }
}
