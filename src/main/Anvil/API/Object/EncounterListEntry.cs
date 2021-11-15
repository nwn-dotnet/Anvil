using NWN.Native.API;

namespace Anvil.API
{
  public sealed class EncounterListEntry
  {
    private readonly CEncounterListEntry encounterListEntry;

    public EncounterListEntry(CEncounterListEntry encounterListEntry)
    {
      this.encounterListEntry = encounterListEntry;
    }

    public string CreatureResRef
    {
      get => encounterListEntry.m_cCreatureResRef.ToString();
      set => encounterListEntry.m_cCreatureResRef = value.ToResRef();
    }

    public float ChallengeRating
    {
      get => encounterListEntry.m_fCR;
      set => encounterListEntry.m_fCR = value;
    }

    public float CreaturePoints
    {
      get => encounterListEntry.m_fCreaturePoints;
      set => encounterListEntry.m_fCreaturePoints = value;
    }

    public bool AlreadyUsed
    {
      get => encounterListEntry.m_bAlreadyUsed.ToBool();
      set => encounterListEntry.m_bAlreadyUsed = value.ToInt();
    }

    public bool AlreadyChecked
    {
      get => encounterListEntry.m_bAlreadyChecked.ToBool();
      set => encounterListEntry.m_bAlreadyChecked = value.ToInt();
    }

    public bool Unique
    {
      get => encounterListEntry.m_bUnique.ToBool();
      set => encounterListEntry.m_bUnique = value.ToInt();
    }
  }
}
