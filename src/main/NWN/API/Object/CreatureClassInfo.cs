using System.Collections.Generic;
using NWN.Native.API;
using ClassType = NWN.API.Constants.ClassType;

namespace NWN.API
{
  public sealed class CreatureClassInfo
  {
    private readonly CNWSCreatureStats_ClassInfo classInfo;

    internal CreatureClassInfo(CNWSCreatureStats_ClassInfo classInfo)
    {
      this.classInfo = classInfo;
    }

    public ClassType Type
    {
      get => (ClassType)classInfo.m_nClass;
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

    public int GetMemorizedSpellSlotCountByLevel(byte spellLevel)
    {
      return classInfo.GetNumberMemorizedSpellSlots(spellLevel);
    }

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
  }
}
