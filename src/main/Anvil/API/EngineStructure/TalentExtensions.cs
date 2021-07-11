using NWN.API.Constants;

namespace NWN.API
{
  public static class TalentExtensions
  {
    public static Talent ToTalent(this Skill skill)
    {
      return skill;
    }

    public static Talent ToTalent(this Spell spell)
    {
      return spell;
    }

    public static Talent ToTalent(this Feat feat)
    {
      return feat;
    }
  }
}
