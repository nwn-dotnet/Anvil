using System;
using System.Collections.Generic;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.AreaOfEffect, ObjectType.AreaOfEffect)]
  public class NwAreaOfEffect : NwGameObject
  {
    internal readonly CNWSAreaOfEffectObject AreaOfEffect;

    internal NwAreaOfEffect(CNWSAreaOfEffectObject areaOfEffectObject) : base(areaOfEffectObject)
    {
      this.AreaOfEffect = areaOfEffectObject;
    }

    /// <inheritdoc cref="NWN.API.Events.AreaOfEffectEvents.OnEnter"/>
    public event Action<AreaOfEffectEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnEnter, GameEventFactory>(this, value)
        .Register<AreaOfEffectEvents.OnEnter>(this);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnEnter, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.AreaOfEffectEvents.OnExit"/>
    public event Action<AreaOfEffectEvents.OnExit> OnExit
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnExit, GameEventFactory>(this, value)
        .Register<AreaOfEffectEvents.OnExit>(this);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnExit, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.AreaOfEffectEvents.OnHeartbeat"/>
    public event Action<AreaOfEffectEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnHeartbeat, GameEventFactory>(this, value)
        .Register<AreaOfEffectEvents.OnHeartbeat>(this);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnHeartbeat, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.AreaOfEffectEvents.OnUserDefined"/>
    public event Action<AreaOfEffectEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<AreaOfEffectEvents.OnUserDefined, GameEventFactory>(this, value)
        .Register<AreaOfEffectEvents.OnUserDefined>(this);
      remove => EventService.Unsubscribe<AreaOfEffectEvents.OnUserDefined, GameEventFactory>(this, value);
    }

    public override Location Location
    {
      set
      {
        AreaOfEffect.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z, true.ToInt());
        Rotation = value.Rotation;
      }
    }

    /// <summary>
    /// Gets the creator of this Area of Effect.
    /// </summary>
    public NwGameObject Creator
    {
      get => NWScript.GetAreaOfEffectCreator(this).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets all objects of the given type that are currently in this area of effect.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <returns>An enumerable containing all objects currently in the effect area.</returns>
    public IEnumerable<T> GetObjectsInEffectArea<T>() where T : NwGameObject
    {
      int objType = (int) GetObjectType<T>();
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != INVALID; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        T gameObject = obj.ToNwObjectSafe<T>();
        if (gameObject != null)
        {
          yield return gameObject;
        }
      }
    }

    /// <summary>
    /// Gets all objects of the given types that are currently in this area of effect.
    /// </summary>
    /// <param name="objectTypes">The types of object to return.</param>
    /// <returns>An enumerable containing all objects currently in the effect area.</returns>
    public IEnumerable<NwGameObject> GetObjectsInEffectArea(ObjectTypes objectTypes)
    {
      int objType = (int) objectTypes;
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != INVALID; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<NwGameObject>();
      }
    }

    public override byte[] Serialize()
      => throw new NotSupportedException();
  }
}
