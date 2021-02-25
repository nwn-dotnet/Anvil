using System;
using System.Collections.Generic;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Encounter, ObjectType.Encounter)]
  public sealed class NwEncounter : NwGameObject
  {
    internal readonly CNWSEncounter Encounter;

    internal NwEncounter(uint objectId, CNWSEncounter encounter) : base(objectId, encounter)
    {
      this.Encounter = encounter;
    }

    public static implicit operator CNWSEncounter(NwEncounter encounter)
    {
      return encounter?.Encounter;
    }

    public event Action<EncounterEvents.OnEnter> OnEnter
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<EncounterEvents.OnExit> OnExit
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<EncounterEvents.OnHeartbeat> OnHeartbeat
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<EncounterEvents.OnExhausted> OnExhausted
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<EncounterEvents.OnUserDefined> OnUserDefined
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public override Location Location
    {
      set
      {
        Encounter.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z);
        Rotation = value.Rotation;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this encounter is spawned and active.
    /// </summary>
    public bool Active
    {
      get => NWScript.GetEncounterActive(this).ToBool();
      set => NWScript.SetEncounterActive(value.ToInt(), this);
    }

    /// <summary>
    /// Gets or sets the difficulty of this encounter.
    /// </summary>
    public EncounterDifficulty Difficulty
    {
      get => (EncounterDifficulty) NWScript.GetEncounterDifficulty(this);
      set => NWScript.SetEncounterDifficulty((int) value, ObjectId);
    }

    /// <summary>
    /// Gets or sets the total amount of spawns this encounter has generated.
    /// </summary>
    public int Spawns
    {
      get => NWScript.GetEncounterSpawnsCurrent(this);
      set => NWScript.SetEncounterSpawnsCurrent(value, this);
    }

    /// <summary>
    /// Gets or sets the max amount of spawns this encounter can generate.
    /// </summary>
    public int MaxSpawns
    {
      get => NWScript.GetEncounterSpawnsMax(this);
      set => NWScript.SetEncounterSpawnsMax(value, this);
    }

    /// <summary>
    /// Gets all objects of the given type that are currently in this encounter trigger.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <returns>An enumerable containing all objects currently in the effect area.</returns>
    public IEnumerator<T> GetObjectsInEncounterArea<T>() where T : NwGameObject
    {
      int objType = (int) GetObjectType<T>();
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != INVALID; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<T>();
      }
    }

    /// <summary>
    /// Gets all objects of the given types that are currently in this area of effect.
    /// </summary>
    /// <param name="objectTypes">The types of object to return.</param>
    /// <returns>An enumerable containing all objects currently in the effect area.</returns>
    public IEnumerator<NwGameObject> GetObjectsInEncounterArea(ObjectTypes objectTypes)
    {
      int objType = (int) objectTypes;
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != INVALID; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<NwGameObject>();
      }
    }
  }
}
