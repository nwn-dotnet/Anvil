using NWN.Core;

namespace Anvil.API
{
  public sealed partial class Talent
  {
    /// <summary>
    /// Creates a talent representing the specified skill.
    /// </summary>
    /// <param name="skill">The skill to represent as a talent.</param>
    /// <returns>The created talent.</returns>
    public static implicit operator Talent(NwSkill skill)
    {
      return NWScript.TalentSkill(skill.Id);
    }

    /// <summary>
    /// Creates a talent representing the specified spell.
    /// </summary>
    /// <param name="spell">The spell to represent as a talent.</param>
    /// <returns>The created talent.</returns>
    public static implicit operator Talent(NwSpell spell)
    {
      return NWScript.TalentSpell(spell.Id);
    }

    /// <summary>
    /// Creates a talent representing the specified feat.
    /// </summary>
    /// <param name="feat">The feat to represent as a talent.</param>
    /// <returns>The created talent.</returns>
    public static implicit operator Talent(NwFeat feat)
    {
      return NWScript.TalentFeat(feat.Id);
    }

    /// <summary>
    /// Creates a talent representing the specified skill constant.
    /// </summary>
    /// <param name="skill">The skill constant.</param>
    /// <returns>The created talent.</returns>
    public static implicit operator Talent(Skill skill)
    {
      return NWScript.TalentSkill((int)skill);
    }

    /// <summary>
    /// Creates a talent representing the specified spell constant.
    /// </summary>
    /// <param name="spell">The spell constant.</param>
    /// <returns>The created talent.</returns>
    public static implicit operator Talent(Spell spell)
    {
      return NWScript.TalentSpell((int)spell);
    }

    /// <summary>
    /// Creates a talent representing the specified feat constant.
    /// </summary>
    /// <param name="feat">The feat constant.</param>
    /// <returns>The created talent.</returns>
    public static implicit operator Talent(Feat feat)
    {
      return NWScript.TalentFeat((int)feat);
    }
  }
}
