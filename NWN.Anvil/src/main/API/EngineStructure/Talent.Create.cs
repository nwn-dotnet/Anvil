using NWN.Core;

namespace Anvil.API
{
  public sealed partial class Talent
  {
    public static implicit operator Talent(NwSkill skill)
    {
      return NWScript.TalentSkill((int)skill.SkillType);
    }

    public static implicit operator Talent(NwSpell spell)
    {
      return NWScript.TalentSpell((int)spell.SpellType);
    }

    public static implicit operator Talent(NwFeat feat)
    {
      return NWScript.TalentFeat((int)feat.FeatType);
    }
  }
}
