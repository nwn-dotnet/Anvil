using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed class CreatureClassInfo
  {
    private readonly CNWSCreatureStats_ClassInfo classInfo;

    internal CreatureClassInfo(CNWSCreatureStats_ClassInfo classInfo)
    {
      this.classInfo = classInfo;
    }

    /// <summary>
    /// Gets the amount of levels in this class.
    /// </summary>
    public byte Level
    {
      get => classInfo.m_nLevel;
    }

    /// <summary>
    /// Gets any negative levels applied to this class (e.g. through level drain).
    /// </summary>
    public byte NegativeLevels
    {
      get => classInfo.m_nNegativeLevels;
    }

    public ClassType Type
    {
      get => (ClassType)classInfo.m_nClass;
    }

    /// <summary>
    /// Adds the specified spell as a known spell at the specified spell level.
    /// </summary>
    /// <param name="spell">The spell to be added.</param>
    /// <param name="spellLevel">The spell level for the spell to be added.</param>
    public void AddKnownSpell(Spell spell, byte spellLevel)
    {
      classInfo.AddKnownSpell(spellLevel, (uint)spell);
    }

    /// <summary>
    /// Clears the specified spell from the creature's spellbook.
    /// </summary>
    /// <param name="spell">The spell to clear.</param>
    public void ClearMemorizedKnownSpells(Spell spell)
    {
      classInfo.ClearMemorizedKnownSpells((uint)spell);
    }

    /// <summary>
    /// Gets the number of spells known by this creature at the specified level.
    /// </summary>
    /// <param name="spellLevel">The spell level to query.</param>
    /// <returns>An integer representing the number of spells known.</returns>
    public ushort GetKnownSpellCountByLevel(byte spellLevel)
    {
      return classInfo.GetNumberKnownSpells(spellLevel);
    }

    /// <summary>
    /// Gets this creature's known spells for the specified spell level.
    /// </summary>
    /// <param name="spellLevel">The spell level to query.</param>
    /// <returns>A list containing the creatures known spells.</returns>
    public IReadOnlyList<Spell> GetKnownSpells(byte spellLevel)
    {
      int spellCount = GetKnownSpellCountByLevel(spellLevel);
      Spell[] retVal = new Spell[spellCount];

      for (byte i = 0; i < spellCount; i++)
      {
        retVal[i] = (Spell)classInfo.GetKnownSpell(spellLevel, i);
      }

      return retVal;
    }

    /// <summary>
    /// Gets the number of spell slots available for a specific spell level.
    /// </summary>
    /// <param name="spellLevel">The spell level to query.</param>
    /// <returns>An integer representing the number of spell slots available.</returns>
    public int GetMemorizedSpellSlotCountByLevel(byte spellLevel)
    {
      return classInfo.GetNumberMemorizedSpellSlots(spellLevel);
    }

    /// <summary>
    /// Gets a list of spell slots available at the given spell level. The returned slots can be modified to change and remove spells.
    /// </summary>
    /// <param name="spellLevel">The spell level to query.</param>
    /// <returns>A list containing the creature's current spell slots.</returns>
    public IReadOnlyList<MemorizedSpellSlot> GetMemorizedSpellSlots(byte spellLevel)
    {
      int spellCount = GetMemorizedSpellSlotCountByLevel(spellLevel);

      List<MemorizedSpellSlot> memorizedSpells = new List<MemorizedSpellSlot>();
      for (byte i = 0; i < spellCount; i++)
      {
        memorizedSpells.Add(new MemorizedSpellSlot(classInfo, spellLevel, i));
      }

      return memorizedSpells.AsReadOnly();
    }

    /// <summary>
    /// Gets the number of remaining, unspent spell slots for the given spell level.
    /// </summary>
    /// <param name="spellLevel">The spell level to query.</param>
    /// <returns></returns>
    public byte GetRemainingSpellSlots(byte spellLevel)
    {
      return classInfo.GetSpellsPerDayLeft(spellLevel);
    }

    /// <summary>
    /// Removes the known spell at the specified level with the specified index, as returned by <see cref="GetKnownSpells"/>.
    /// </summary>
    /// <param name="spellLevel">The spell level to query.</param>
    /// <param name="spell">The spell to remove.</param>
    public void RemoveKnownSpell(byte spellLevel, Spell spell)
    {
      classInfo.RemoveKnownSpell(spellLevel, (uint)spell);
    }

    /// <summary>
    /// Sets the number of unspent spell slots for the given spell level.
    /// </summary>
    /// <param name="spellLevel">The spell level to modify.</param>
    /// <param name="slotsRemaining">The new amount of spell slot remaining.</param>
    public void SetRemainingSpellSlots(byte spellLevel, byte slotsRemaining)
    {
      classInfo.SetSpellsPerDayLeft(spellLevel, slotsRemaining);
    }
  }
}
