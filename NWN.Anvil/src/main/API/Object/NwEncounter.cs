using System;
using System.Collections.Generic;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// An encounter trigger that spawns creatures.
  /// </summary>
  [NativeObjectInfo(ObjectTypes.Encounter, ObjectType.Encounter)]
  public sealed partial class NwEncounter : NwGameObject
  {
    internal readonly CNWSEncounter Encounter;

    internal NwEncounter(CNWSEncounter encounter) : base(encounter)
    {
      Encounter = encounter;
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
    /// Gets or sets if this encounter respawns or not.
    /// </summary>
    public bool CanReset
    {
      get => Encounter.m_bReset.ToBool();
      set => Encounter.m_bReset = value.ToInt();
    }

    /// <summary>
    /// Gets the list of creatures that can be spawned by this encounter.
    /// </summary>
    public IReadOnlyList<EncounterListEntry> CreatureList
    {
      get
      {
        CEncounterListEntryArray cEncounterList = CEncounterListEntryArray.FromPointer(Encounter.m_pEncounterList);
        EncounterListEntry[] retVal = new EncounterListEntry[Encounter.m_nNumEncounterListEntries];

        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new EncounterListEntry(this, cEncounterList.GetItem(i));
        }

        return retVal;
      }
    }

    /// <summary>
    /// Gets or sets the difficulty of this encounter.
    /// </summary>
    public EncounterDifficulty Difficulty
    {
      get => (EncounterDifficulty)NWScript.GetEncounterDifficulty(this);
      set => NWScript.SetEncounterDifficulty((int)value, ObjectId);
    }

    /// <summary>
    /// Gets or sets the faction for this encounter.
    /// </summary>
    public NwFaction Faction
    {
      get => NwFaction.FromFactionId(Encounter.m_nFactionId);
      set => Encounter.m_nFactionId = value.FactionId;
    }

    /// <summary>
    /// Gets the maximum amount of creatures that this encounter will spawn.
    /// </summary>
    public int MaxSpawnedCreatures
    {
      get => Encounter.m_nMaxSpawnedCreatures;
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
    /// Gets the minimum amount of creatures that this encounter will spawn.
    /// </summary>
    public int MinSpawnedCreatures
    {
      get => Encounter.m_nMinNumSpawnedCreatures;
    }

    /// <summary>
    /// Gets the number of creatures that are spawned and alive.
    /// </summary>
    public int NumSpawnedCreatures
    {
      get => Encounter.m_nNumSpawnedCreatures;
    }

    /// <summary>
    /// Gets or sets if this encounter is player triggered only.
    /// </summary>
    public bool PlayerTriggeredOnly
    {
      get => Encounter.m_bPlayerTriggeredOnly.ToBool();
      set => Encounter.m_bPlayerTriggeredOnly = value.ToInt();
    }

    /// <summary>
    /// Gets or sets the reset time of this encounter.
    /// </summary>
    public TimeSpan ResetTime
    {
      get => TimeSpan.FromSeconds(Encounter.m_nResetTime);
      set => Encounter.m_nResetTime = (int)Math.Round(value.TotalSeconds, MidpointRounding.ToZero);
    }

    /// <summary>
    /// Gets the list of spawn points that creatures can spawn at from this encounter.
    /// </summary>
    public IReadOnlyList<EncounterSpawnPoint> SpawnPointList
    {
      get
      {
        CEncounterSpawnPointArray cSpawnPointList = CEncounterSpawnPointArray.FromPointer(Encounter.m_pSpawnPointList);
        EncounterSpawnPoint[] retVal = new EncounterSpawnPoint[Encounter.m_nNumSpawnPoints];

        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = new EncounterSpawnPoint(cSpawnPointList.GetItem(i));
        }

        return retVal;
      }
    }

    /// <summary>
    /// Gets or sets the total amount of spawns this encounter has generated.
    /// </summary>
    public int Spawns
    {
      get => NWScript.GetEncounterSpawnsCurrent(this);
      set => NWScript.SetEncounterSpawnsCurrent(value, this);
    }

    public static NwEncounter Deserialize(byte[] serialized)
    {
      CNWSEncounter encounter = null;

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTE"))
        {
          return false;
        }

        encounter = new CNWSEncounter(Invalid);
        if (encounter.LoadEncounter(resGff, resStruct).ToBool())
        {
          encounter.LoadObjectState(resGff, resStruct);
          GC.SuppressFinalize(encounter);
          return true;
        }

        encounter.Dispose();
        return false;
      });

      return result && encounter != null ? encounter.ToNwObject<NwEncounter>() : null;
    }

    public static implicit operator CNWSEncounter(NwEncounter encounter)
    {
      return encounter?.Encounter;
    }

    /// <summary>
    /// Gets all objects of the given type that are currently in this encounter trigger.
    /// </summary>
    /// <typeparam name="T">The type of objects to return.</typeparam>
    /// <returns>An enumerable containing all objects currently in the encounter area.</returns>
    public IEnumerable<T> GetObjectsInEncounterArea<T>() where T : NwGameObject
    {
      int objType = (int)GetObjectType<T>();
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != Invalid; obj = NWScript.GetNextInPersistentObject(this, objType))
      {
        yield return obj.ToNwObject<T>();
      }
    }

    /// <summary>
    /// Gets all objects of the given types that are currently in this encounter trigger.
    /// </summary>
    /// <param name="objectTypes">The types of object to return.</param>
    /// <returns>An enumerable containing all objects currently in the encounter area.</returns>
    public IEnumerable<NwGameObject> GetObjectsInEncounterArea(ObjectTypes objectTypes = ObjectTypes.All)
    {
      int objType = (int)objectTypes;
      for (uint obj = NWScript.GetFirstInPersistentObject(this, objType); obj != Invalid; obj = NWScript.GetNextInPersistentObject(this, objType))
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

    internal override void RemoveFromArea()
    {
      Encounter.RemoveFromArea();
    }

    private protected override void AddToArea(CNWSArea area, float x, float y, float z)
    {
      Encounter.AddToArea(area, x, y, z, true.ToInt());
    }
  }
}
