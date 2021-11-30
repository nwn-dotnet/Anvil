using NWN.Native.API;

namespace Anvil.API
{
  public sealed class EncounterListEntry
  {
    private readonly CNWSEncounter encounter;
    private readonly CEncounterListEntry encounterListEntry;

    public EncounterListEntry(CNWSEncounter encounter, CEncounterListEntry encounterListEntry)
    {
      this.encounter = encounter;
      this.encounterListEntry = encounterListEntry;
    }

    public bool AlreadyUsed
    {
      get => encounterListEntry.m_bAlreadyUsed.ToBool();
      set => encounterListEntry.m_bAlreadyUsed = value.ToInt();
    }

    public float ChallengeRating
    {
      get => encounterListEntry.m_fCR;
      set
      {
        encounterListEntry.m_fCR = value;
        encounterListEntry.m_fCreaturePoints = encounter.CalculatePointsFromCR(value);
      }
    }

    public string CreatureResRef
    {
      get => encounterListEntry.m_cCreatureResRef.ToString();
      set => encounterListEntry.m_cCreatureResRef = value.ToResRef();
    }

    public bool Unique
    {
      get => encounterListEntry.m_bUnique.ToBool();
      set => encounterListEntry.m_bUnique = value.ToInt();
    }
  }
}
