using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public partial class Talent
  {
    public static implicit operator Talent(Skill skill)
    {
      return NWScript.TalentSkill((int) skill);
    }

    public static implicit operator Talent(Spell spell)
    {
      return NWScript.TalentSpell((int) spell);
    }

    public static implicit operator Talent(Feat feat)
    {
      return NWScript.TalentFeat((int) feat);
    }
  }
}
