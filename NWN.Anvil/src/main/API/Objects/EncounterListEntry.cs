using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// Represents a single creature entry in an encounter's spawn list, including blueprint, challenge rating, uniqueness, and usage state.
  /// </summary>
  /// <param name="encounter">The owning encounter used for point budget calculations.</param>
  /// <param name="encounterListEntry">The native list entry backing this wrapper.</param>
  public sealed class EncounterListEntry(CNWSEncounter encounter, CEncounterListEntry encounterListEntry)
  {
    /// <summary>
    /// Gets or sets whether this entry has already been used for spawning.
    /// </summary>
    /// <remarks>
    /// When marked as used, this entry may be skipped by the encounter until reset, depending on encounter configuration.
    /// </remarks>
    public bool AlreadyUsed
    {
      get => encounterListEntry.m_bAlreadyUsed.ToBool();
      set => encounterListEntry.m_bAlreadyUsed = value.ToInt();
    }

    /// <summary>
    /// Gets or sets the challenge rating for this entry.
    /// </summary>
    /// <remarks>
    /// Updating the challenge rating recalculates the internal creature point value via the owning encounter's budget rules.
    /// </remarks>
    public float ChallengeRating
    {
      get => encounterListEntry.m_fCR;
      set
      {
        encounterListEntry.m_fCR = value;
        encounterListEntry.m_fCreaturePoints = encounter.CalculatePointsFromCR(value);
      }
    }

    /// <summary>
    /// Gets or sets the blueprint resource reference of the creature to spawn.
    /// </summary>
    public string? CreatureResRef
    {
      get => encounterListEntry.m_cCreatureResRef.ToString();
      set => encounterListEntry.m_cCreatureResRef = value.ToResRef();
    }

    /// <summary>
    /// Gets or sets whether the creature represented by this entry is unique to the encounter.
    /// </summary>
    /// <remarks>
    /// Unique entries are typically limited to a single spawn instance within the encounter lifecycle.
    /// </remarks>
    public bool Unique
    {
      get => encounterListEntry.m_bUnique.ToBool();
      set => encounterListEntry.m_bUnique = value.ToInt();
    }
  }
}
