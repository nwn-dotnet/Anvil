using NWN.Core;

namespace Anvil.API
{
  public sealed partial class Talent
  {
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
