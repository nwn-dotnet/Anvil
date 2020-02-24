using NWN;

namespace NWM.API
{
  public class NwEncounter : NwGameObject
  {
    protected internal NwEncounter(uint objectId) : base(objectId) {}

    public bool Active
    {
      get => NWScript.GetEncounterActive(this).ToBool();
      set => NWScript.SetEncounterActive(value.ToInt(), this);
    }

    public Difficulty Difficulty
    {
      get => (Difficulty) NWScript.GetEncounterDifficulty(this);
      set => NWScript.SetEncounterDifficulty((int)value, ObjectId);
    }

    public int Spawns
    {
      get => NWScript.GetEncounterSpawnsCurrent(this);
      set => NWScript.SetEncounterSpawnsCurrent(value, this);
    }

    public int MaxSpawns
    {
      get => NWScript.GetEncounterSpawnsMax(this);
      set => NWScript.SetEncounterSpawnsMax(value, this);
    }
  }

  public enum Difficulty
  {
    VeryEasy = 0,
    Easy = 1,
    Normal = 2,
    Hard = 3,
    Impossible = 4
  }
}