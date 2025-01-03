using NWN.Native.API;

namespace Anvil.API
{
  public sealed class ClassSkill(CNWClass_Skill skill)
  {
    public bool IsClassSkill => skill.bClassSkill.ToBool();

    public NwSkill Skill => NwSkill.FromSkillId(skill.nSkill)!;
  }
}
