using System;
using System.Collections.Generic;
using Anvil.Native;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed unsafe class CreatureClassInfo
  {
    private const int KnownSpellArraySize = 10; // Cantrips + 9 spell levels
    private static readonly int KnownSpellArrayListStructSize = sizeof(IntPtr) + sizeof(int) + sizeof(int);

    private readonly CNWSCreatureStats_ClassInfo classInfo;

    internal CreatureClassInfo(CNWSCreatureStats_ClassInfo classInfo)
    {
      this.classInfo = classInfo;
    }

    /// <summary>
    /// Gets the associated class.
    /// </summary>
    public NwClass Class => NwClass.FromClassId(classInfo.m_nClass)!;

    /// <summary>
    /// Gets the 2 domains set for this class.<br/>
    /// Domains can be modified by editing the contents of this array.
    /// </summary>
    /// <remarks>By default, a non-domain class will be populated with <see cref="Domain.Air"/> and <see cref="Domain.Animal"/> (index 0 and 1 respectively).</remarks>
    public IArray<NwDomain?> Domains => new ArrayWrapper<byte, NwDomain?>(classInfo.m_nDomain, id => NwDomain.FromDomainId(id), domain => domain?.Id ?? 0);

    /// <summary>
    /// Gets a mutable list of known spells.<br/>
    /// The returned array is indexed by spell level, 0 = cantrips, 1 = level 1 spells, etc.
    /// </summary>
    /// <remarks>
    /// When used on players, you also need to update <see cref="CreatureLevelInfo.AddedKnownSpells"/> and <see cref="CreatureLevelInfo.RemovedKnownSpells"/> on the relevant level taken in this class, otherwise players will fail ELC checks.
    /// </remarks>
    public IReadOnlyList<IList<NwSpell>> KnownSpells
    {
      get
      {
        IList<NwSpell>[] spells = new IList<NwSpell>[KnownSpellArraySize];
        IntPtr ptr = classInfo.m_pKnownSpellList.Pointer;

        for (int i = 0; i < spells.Length; i++)
        {
          CExoArrayListUInt32 spellList = CExoArrayListUInt32.FromPointer(ptr + i * KnownSpellArrayListStructSize);
          spells[i] = new ListWrapper<uint, NwSpell>(spellList, spellId => NwSpell.FromSpellId((int)spellId)!, spell => (uint)spell.Id);
        }

        return spells;
      }
    }

    /// <summary>
    /// Gets the amount of levels in this class.
    /// </summary>
    public byte Level => classInfo.m_nLevel;

    /// <summary>
    /// Gets any negative levels applied to this class (e.g. through level drain).
    /// </summary>
    public byte NegativeLevels => classInfo.m_nNegativeLevels;

    /// <summary>
    /// Gets the spell school for this class.
    /// </summary>
    public SpellSchool School => (SpellSchool)classInfo.m_nSchool;

    /// <summary>
    /// Adds the specified spell as a known spell at the specified spell level.
    /// </summary>
    /// <param name="spell">The spell to be added.</param>
    /// <param name="spellLevel">The spell level for the spell to be added.</param>
    [Obsolete("Use the KnownSpells property instead.")]
    public void AddKnownSpell(NwSpell spell, byte spellLevel)
    {
      classInfo.AddKnownSpell(spellLevel, spell.Id.AsUInt());
    }

    /// <summary>
    /// Clears the specified spell from the creature's spellbook.
    /// </summary>
    /// <param name="spell">The spell to clear.</param>
    public void ClearMemorizedKnownSpells(NwSpell spell)
    {
      classInfo.ClearMemorizedKnownSpells(spell.Id.AsUInt());
    }

    /// <summary>
    /// Gets the number of spells known by this creature at the specified level.
    /// </summary>
    /// <param name="spellLevel">The spell level to query.</param>
    /// <returns>An integer representing the number of spells known.</returns>
    [Obsolete("Use the KnownSpells property instead.")]
    public ushort GetKnownSpellCountByLevel(byte spellLevel)
    {
      return classInfo.GetNumberKnownSpells(spellLevel);
    }

    /// <summary>
    /// Gets this creature's known spells for the specified spell level.
    /// </summary>
    /// <param name="spellLevel">The spell level to query.</param>
    /// <returns>A list containing the creatures known spells.</returns>
    [Obsolete("Use the KnownSpells property instead.")]
    public IReadOnlyList<NwSpell> GetKnownSpells(byte spellLevel)
    {
      int spellCount = GetKnownSpellCountByLevel(spellLevel);
      NwSpell[] retVal = new NwSpell[spellCount];

      for (byte i = 0; i < spellCount; i++)
      {
        retVal[i] = NwSpell.FromSpellId((int)classInfo.GetKnownSpell(spellLevel, i))!;
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
    [Obsolete("Use the KnownSpells property instead.")]
    public void RemoveKnownSpell(byte spellLevel, NwSpell spell)
    {
      classInfo.RemoveKnownSpell(spellLevel, spell.Id.AsUInt());
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
