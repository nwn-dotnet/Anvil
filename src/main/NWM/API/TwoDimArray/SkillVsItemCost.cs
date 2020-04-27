using System;
using System.Collections.Generic;
using NWM.API.Constants;
using NWM.Core;

namespace NWM.API
{
  public sealed class SkillVsItemCost : ITwoDimArray
  {
    private readonly List<SkillRequirement> skillRequirements = new List<SkillRequirement>();

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
      SkillRequirement skillRequirement = GetSkillRequirement(itemCost);
      int skillRequired = skillRequirement.GetRequirementValue(restrictionType);

      return skillRequired != -1 && skillValue >= skillRequired;
    }

    public SkillRequirement GetSkillRequirement(NwItem item)
    {
      return GetSkillRequirement(item.GoldValue);
    }

    public SkillRequirement GetSkillRequirement(int itemCost)
    {
      foreach (SkillRequirement skillRequirement in skillRequirements)
      {
        if (Math.Max(skillRequirement.DeviceCostMax, itemCost) == skillRequirement.DeviceCostMax)
        {
          return skillRequirement;
        }
      }

      return skillRequirements[^1];
    }

    void ITwoDimArray.DeserializeRow(TwoDimEntry twoDimEntry)
    {
      string value = twoDimEntry("DeviceCostMax");
      string classReq = twoDimEntry("SkillReq_Class");
      string raceReq = twoDimEntry("SkillReq_Race");
      string alignReq = twoDimEntry("SkillReq_Align");

      if (string.IsNullOrEmpty(value))
      {
        return;
      }

      skillRequirements.Add(new SkillRequirement(value, classReq, raceReq, alignReq));
    }

    public sealed class SkillRequirement
    {
      public readonly int DeviceCostMax;
      public readonly int ClassReq;
      public readonly int RaceReq;
      public readonly int AlignmentReq;

      public SkillRequirement(string value, string classReq, string raceReq, string alignReq)
      {
        this.DeviceCostMax = value.ToInt();
        this.ClassReq = int.TryParse(classReq, out int classReqVal) ? classReqVal : -1;
        this.RaceReq = int.TryParse(raceReq, out int raceReqVal) ? raceReqVal : -1;
        this.AlignmentReq = int.TryParse(alignReq, out int alignReqVal) ? alignReqVal : -1;
      }

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