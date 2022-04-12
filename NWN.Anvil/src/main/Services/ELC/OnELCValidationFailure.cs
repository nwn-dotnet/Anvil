using Anvil.API;

namespace Anvil.Services
{
  public class OnELCValidationFailure
  {
    public bool IgnoreFailure { get; set; }
    public NwPlayer Player { get; init; }

    public StrRef StrRef { get; set; }

    public ValidationFailureSubType SubType { get; init; }

    public ValidationFailureType Type { get; init; }
  }

  public sealed class OnELCItemValidationFailure : OnELCValidationFailure
  {
    public NwItem Item { get; init; }
  }

  public sealed class OnELCLevelValidationFailure : OnELCValidationFailure
  {
    public int Level { get; init; }
  }

  public sealed class OnELCSkillValidationFailure : OnELCValidationFailure
  {
    public NwSkill Skill { get; init; }
  }

  public sealed class OnELCFeatValidationFailure : OnELCValidationFailure
  {
    public NwFeat Feat { get; init; }
  }

  public sealed class OnELCSpellValidationFailure : OnELCValidationFailure
  {
    public NwSpell Spell { get; init; }
  }
}
