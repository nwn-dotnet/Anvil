namespace Anvil.API
{
  public static class TalentExtensions
  {
    public static Talent ToTalent(this NwSkill skill)
    {
      return skill;
    }

    public static Talent ToTalent(this NwSpell spell)
    {
      return spell;
    }

    public static Talent ToTalent(this NwFeat feat)
    {
      return feat;
    }
  }
}
