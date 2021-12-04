using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A creature/character skill.
  /// </summary>
  public sealed class NwSkill
  {
    [Inject]
    private static RulesetService RulesetService { get; set; }

    [Inject]
    private static TlkTable TlkTable { get; set; }

    private readonly CNWSkill skillInfo;

    public NwSkill(Skill skillType, CNWSkill skillInfo)
    {
      this.skillInfo = skillInfo;
      SkillType = skillType;
    }

    /// <summary>
    /// Gets if this skill can be used by all classes.
    /// </summary>
    public bool AllClassesCanUse
    {
      get => skillInfo.m_bAllClassesCanUse.ToBool();
    }

    /// <summary>
    /// Gets if this skill is subject to the armor check penalty.
    /// </summary>
    public bool ArmorCheckPenalty
    {
      get => skillInfo.m_bArmorCheckPenalty.ToBool();
    }

    /// <summary>
    /// Gets the description of this skill.
    /// </summary>
    public string Description
    {
      get => TlkTable.GetSimpleString((uint)skillInfo.m_nDescriptionStrref);
    }

    /// <summary>
    /// Gets the ResRef of the GUI icon representing this skill.
    /// </summary>
    public string IconResRef
    {
      get => skillInfo.m_sIconName.ToString();
    }

    /// <summary>
    /// Gets if this skill is considered a hostile action.
    /// </summary>
    public bool IsHostileSkill
    {
      get => skillInfo.m_bHostileSkill.ToBool();
    }

    /// <summary>
    /// Gets if this skill is untrained. Trained skills require at least a single rank before they can be used.
    /// </summary>
    public bool IsUntrained
    {
      get => skillInfo.m_bUntrained.ToBool();
    }

    /// <summary>
    /// Gets the ability that the skill uses as
    /// </summary>
    public Ability KeyAbility
    {
      get => (Ability)skillInfo.m_nKeyAbility;
    }

    /// <summary>
    /// Gets the name of this skill.
    /// </summary>
    public string Name
    {
      get => TlkTable.GetSimpleString((uint)skillInfo.m_nNameStrref);
    }

    public Skill SkillType { get; }

    public static NwSkill FromSkillId(int skillId)
    {
      return RulesetService.Skills[skillId];
    }

    public static NwSkill FromSkillType(Skill skillType)
    {
      return RulesetService.Skills[(int)skillType];
    }
  }
}
