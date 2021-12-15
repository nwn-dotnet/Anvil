using NWN.Native.API;

namespace Anvil.API
{
  public sealed class MemorizedSpellSlot
  {
    private readonly CNWSCreatureStats_ClassInfo classInfo;
    private readonly byte spellLevel;
    private readonly byte spellSlot;

    internal MemorizedSpellSlot(CNWSCreatureStats_ClassInfo classInfo, byte spellLevel, byte spellSlot)
    {
      this.classInfo = classInfo;
      this.spellLevel = spellLevel;
      this.spellSlot = spellSlot;
    }

    public bool IsDomainSpell
    {
      get => classInfo.GetIsDomainSpell(spellLevel, spellSlot).ToBool();
      set => classInfo.SetMemorizedSpellSlot(spellLevel, spellSlot, Spell.Id, value.ToInt(), (byte)MetaMagic);
    }

    public bool IsPopulated
    {
      get => classInfo.GetMemorizedSpellInSlotDetails(spellLevel, spellSlot) != null;
    }

    public bool IsReady
    {
      get => classInfo.GetMemorizedSpellInSlotReady(spellLevel, spellSlot).ToBool();
      set => classInfo.SetMemorizedSpellInSlotReady(spellLevel, spellSlot, value.ToInt());
    }

    public MetaMagic MetaMagic
    {
      get => (MetaMagic)classInfo.GetMemorizedSpellInSlotMetaType(spellLevel, spellSlot);
      set => classInfo.SetMemorizedSpellSlot(spellLevel, spellSlot, Spell.Id, IsDomainSpell.ToInt(), (byte)value);
    }

    public NwSpell Spell
    {
      get => NwSpell.FromSpellId((int)classInfo.GetMemorizedSpellInSlot(spellLevel, spellSlot));
      set => classInfo.SetMemorizedSpellSlot(spellLevel, spellSlot, value.Id, IsDomainSpell.ToInt(), (byte)MetaMagic);
    }

    public void ClearMemorizedSpell()
    {
      classInfo.ClearMemorizedSpellSlot(spellLevel, spellSlot);
    }
  }
}
