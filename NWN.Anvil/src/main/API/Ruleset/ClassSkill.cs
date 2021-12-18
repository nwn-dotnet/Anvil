using NWN.Native.API;

namespace Anvil.API
{
  public sealed class ClassSkill
  {
    private readonly CNWClass_Skill skill;

    public ClassSkill(CNWClass_Skill skill)
    {
      this.skill = skill;
    }

    public bool IsClassSkill
    {
      get => skill.bClassSkill.ToBool();
    }

    public NwSkill Skill
    {
      get => NwSkill.FromSkillId(skill.nSkill);
    }
  }
}
