using NWN.Core;

namespace Anvil.API
{
  public sealed partial class Talent
  {
    public static implicit operator Talent(NwSkill skill)
    {
      return NWScript.TalentSkill(skill.Id);
    }

    public static implicit operator Talent(NwSpell spell)
    {
      return NWScript.TalentSpell((int)spell.Id);
    }

    public static implicit operator Talent(NwFeat feat)
    {
      return NWScript.TalentFeat(feat.Id);
    }

    public static implicit operator Talent(Skill skill)
    {
      return NWScript.TalentSkill((int)skill);
    }

    public static implicit operator Talent(Spell spell)
    {
      return NWScript.TalentSpell((int)spell);
    }

    public static implicit operator Talent(Feat feat)
    {
      return NWScript.TalentFeat((int)feat);
    }
  }
}
