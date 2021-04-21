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

    internal NwEncounter(CNWSEncounter encounter) : base(encounter)
    {
      this.Encounter = encounter;
    }

    public static implicit operator CNWSEncounter(NwEncounter encounter)
    {
      return encounter?.Encounter;
    }

    /// <inheritdoc cref="NWN.API.Events.EncounterEvents.OnEnter"/>
    public event Action<EncounterEvents.OnEnter> OnEnter
    {
      add => EventService.Subscribe<EncounterEvents.OnEnter, GameEventFactory>(this, value)
        .Register<EncounterEvents.OnEnter>(this);
      remove => EventService.Unsubscribe<EncounterEvents.OnEnter, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.EncounterEvents.OnExit"/>
    public event Action<EncounterEvents.OnExit> OnExit
    {
      add => EventService.Subscribe<EncounterEvents.OnExit, GameEventFactory>(this, value)
        .Register<EncounterEvents.OnExit>(this);
      remove => EventService.Unsubscribe<EncounterEvents.OnExit, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.EncounterEvents.OnHeartbeat"/>
    public event Action<EncounterEvents.OnHeartbeat> OnHeartbeat
    {
      add => EventService.Subscribe<EncounterEvents.OnHeartbeat, GameEventFactory>(this, value)
        .Register<EncounterEvents.OnHeartbeat>(this);
      remove => EventService.Unsubscribe<EncounterEvents.OnHeartbeat, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.EncounterEvents.OnExhausted"/>
    public event Action<EncounterEvents.OnExhausted> OnExhausted
    {
      add => EventService.Subscribe<EncounterEvents.OnExhausted, GameEventFactory>(this, value)
        .Register<EncounterEvents.OnExhausted>(this);
      remove => EventService.Unsubscribe<EncounterEvents.OnExhausted, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.EncounterEvents.OnUserDefined"/>
    public event Action<EncounterEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<EncounterEvents.OnUserDefined, GameEventFactory>(this, value)
        .Register<EncounterEvents.OnUserDefined>(this);
      remove => EventService.Unsubscribe<EncounterEvents.OnUserDefined, GameEventFactory>(this, value);
    }

    public override Location Location
    {
      set
      {
        Encounter.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z, true.ToInt());
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

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("UTE", (resGff, resStruct) =>
      {
        Encounter.SaveObjectState(resGff, resStruct);
        return Encounter.SaveEncounter(resGff, resStruct).ToBool();
      });
    }

    public static NwEncounter Deserialize(byte[] serialized)
    {
      CNWSEncounter encounter = null;

      NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTE"))
        {
          return false;
        }

        encounter = new CNWSEncounter(INVALID);
        if (encounter.LoadEncounter(resGff, resStruct).ToBool())
        {
          encounter.LoadObjectState(resGff, resStruct);
          return true;
        }

        encounter.Dispose();
        return false;
      });

      return encounter != null ? encounter.m_idSelf.ToNwObject<NwEncounter>() : null;
    }
  }
}
