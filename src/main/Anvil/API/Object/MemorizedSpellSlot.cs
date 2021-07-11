using NWN.API.Constants;
using NWN.Native.API;

namespace NWN.API
{
  public sealed class MemorizedSpellSlot
  {
    private readonly CNWSCreatureStats_ClassInfo classInfo;
    private readonly byte spellLevel;
    private readonly byte spellSlot;

    public Spell Spell
    {
      get => (Spell)classInfo.GetMemorizedSpellInSlot(spellLevel, spellSlot);
      set => classInfo.SetMemorizedSpellSlot(spellLevel, spellSlot, (uint)value, IsDomainSpell.ToInt(), (byte)MetaMagic);
    }

    public bool IsReady
    {
      get => classInfo.GetMemorizedSpellInSlotReady(spellLevel, spellSlot).ToBool();
      set => classInfo.SetMemorizedSpellInSlotReady(spellLevel, spellSlot, value.ToInt());
    }

    public MetaMagic MetaMagic
    {
      get => (MetaMagic)classInfo.GetMemorizedSpellInSlotMetaType(spellLevel, spellSlot);
      set => classInfo.SetMemorizedSpellSlot(spellLevel, spellSlot, (uint)Spell, IsDomainSpell.ToInt(), (byte)value);
    }

    public bool IsDomainSpell
    {
      get => classInfo.GetIsDomainSpell(spellLevel, spellSlot).ToBool();
      set => classInfo.SetMemorizedSpellSlot(spellLevel, spellSlot, (uint)Spell, value.ToInt(), (byte)MetaMagic);
    }

    public bool IsPopulated
    {
      get => classInfo.GetMemorizedSpellInSlotDetails(spellLevel, spellSlot) != null;
    }

    internal MemorizedSpellSlot(CNWSCreatureStats_ClassInfo classInfo, byte spellLevel, byte spellSlot)
    {
      this.classInfo = classInfo;
      this.spellLevel = spellLevel;
      this.spellSlot = spellSlot;
    }

    public void ClearMemorizedSpell()
    {
      classInfo.ClearMemorizedSpellSlot(spellLevel, spellSlot);
    }
  }
}
