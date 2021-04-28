using NWN.API.Constants;
using NWN.Native.API;

namespace NWN.API
{
  public sealed class MemorizedSpellSlot
  {
    private readonly CNWSCreatureStats_ClassInfo classInfo;
    private readonly CNWSStats_Spell spellStats;
    private readonly byte spellLevel;
    private readonly byte spellSlot;

    public Spell Spell
    {
      get => (Spell)spellStats.m_nSpellId;
      set => classInfo.SetMemorizedSpellSlot(spellLevel, spellSlot, (uint)value, IsDomainSpell.ToInt(), (byte)MetaMagic);
    }

    public bool Ready
    {
      get => spellStats.m_bReadied.ToBool();
      set => classInfo.SetMemorizedSpellInSlotReady(spellLevel, spellSlot, value.ToInt());
    }

    public MetaMagic MetaMagic
    {
      get => (MetaMagic)spellStats.m_nMetaType;
      set => classInfo.SetMemorizedSpellSlot(spellLevel, spellSlot, (uint)Spell, IsDomainSpell.ToInt(), (byte)value);
    }

    public bool IsDomainSpell
    {
      get => spellStats.m_bDomainSpell.ToBool();
      set => classInfo.SetMemorizedSpellSlot(spellLevel, spellSlot, (uint)Spell, value.ToInt(), (byte)MetaMagic);
    }

    internal MemorizedSpellSlot(CNWSCreatureStats_ClassInfo classInfo, CNWSStats_Spell spellStats, byte spellLevel, byte spellSlot)
    {
      this.classInfo = classInfo;
      this.spellStats = spellStats;
      this.spellLevel = spellLevel;
      this.spellSlot = spellSlot;
    }

    public void ClearMemorizedSpell()
    {
      classInfo.ClearMemorizedSpellSlot(spellLevel, spellSlot);
    }
  }
}
