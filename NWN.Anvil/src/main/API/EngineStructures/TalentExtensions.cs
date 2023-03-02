namespace Anvil.API
{
  public static class TalentExtensions
  {
    /// <summary>
    /// Converts the specified skill into a <see cref="Talent"/>.
    /// </summary>
    public static Talent ToTalent(this NwSkill skill)
    {
      return skill;
    }

    /// <summary>
    /// Converts the specified spell into a <see cref="Talent"/>.
    /// </summary>
    public static Talent ToTalent(this NwSpell spell)
    {
      return spell;
    }

    /// <summary>
    /// Converts the specified feat into a <see cref="Talent"/>.
    /// </summary>
    public static Talent ToTalent(this NwFeat feat)
    {
      return feat;
    }
  }
}
