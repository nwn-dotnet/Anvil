using System;
using System.Collections.Generic;
using NWM.API.Constants;
using NWM.Core;

namespace NWM.API
{
  public sealed class SkillVsItemCost : ITwoDimArray
  {
    private readonly List<Entry> entries = new List<Entry>();

    public bool MeetsUMDRequirement(NwCreature creature, NwItem item, RestrictionType restrictionType)
    {
      return MeetsSkillRequirement(creature.GetSkillRank(Skill.UseMagicDevice), item, restrictionType);
    }

    public bool MeetsSkillRequirement(int skillValue, NwItem item, RestrictionType restrictionType)
    {
      return MeetsSkillRequirement(skillValue, item.GoldValue, restrictionType);
    }

    public bool MeetsSkillRequirement(int skillValue, int itemCost, RestrictionType restrictionType)
    {
      Entry entry = GetSkillRequirement(itemCost);
      int skillRequired = entry.GetRequirementValue(restrictionType);

      return skillRequired != -1 && skillValue >= skillRequired;
    }

    public Entry GetSkillRequirement(NwItem item)
    {
      return GetSkillRequirement(item.GoldValue);
    }

    public Entry GetSkillRequirement(int itemCost)
    {
      foreach (Entry skillRequirement in entries)
      {
        if (Math.Max(skillRequirement.DeviceCostMax, itemCost) == skillRequirement.DeviceCostMax)
        {
          return skillRequirement;
        }
      }

      return entries[^1];
    }

    void ITwoDimArray.DeserializeRow(int rowIndex, TwoDimEntry twoDimEntry)
    {
      string value = twoDimEntry("DeviceCostMax");
      if (string.IsNullOrEmpty(value))
      {
        return;
      }

      int deviceCost = value.ParseInt();
      int classReq = twoDimEntry("SkillReq_Class").ParseInt(-1);
      int raceReq = twoDimEntry("SkillReq_Race").ParseInt(-1);
      int alignReq = twoDimEntry("SkillReq_Align").ParseInt(-1);

      entries.Add(new Entry(deviceCost, classReq, raceReq, alignReq));
    }

    public sealed class Entry
    {
      public Entry(int deviceCostMax, int classReq, int raceReq, int alignmentReq)
      {
        DeviceCostMax = deviceCostMax;
        ClassReq = classReq;
        RaceReq = raceReq;
        AlignmentReq = alignmentReq;
      }

      public readonly int DeviceCostMax;
      public readonly int ClassReq;
      public readonly int RaceReq;
      public readonly int AlignmentReq;

      public int GetRequirementValue(RestrictionType restrictionType)
      {
        switch (restrictionType)
        {
          case RestrictionType.Class:
            return ClassReq;
          case RestrictionType.Race:
            return RaceReq;
          case RestrictionType.Alignment:
            return AlignmentReq;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }
  }

  public enum RestrictionType
  {
    Class,
    Race,
    Alignment
  }
}