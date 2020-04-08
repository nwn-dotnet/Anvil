using NWN;

namespace NWM.API
{
  public sealed class NwEncounter : NwGameObject
  {
    internal NwEncounter(uint objectId) : base(objectId) {}

    /// <summary>
    /// Gets or sets whether this encounter is spawned and active.
    /// </summary>
    public bool Active
    {
      get => NWScript.GetEncounterActive(this).ToBool();
      set => NWScript.SetEncounterActive(value.ToInt(), this);
    }

    /// <summary>
    /// Gets or sets the difficulty of this encounter.
    /// </summary>
    public Difficulty Difficulty
    {
      get => (Difficulty) NWScript.GetEncounterDifficulty(this);
      set => NWScript.SetEncounterDifficulty((int)value, ObjectId);
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