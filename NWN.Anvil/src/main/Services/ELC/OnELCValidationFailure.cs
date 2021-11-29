using Anvil.API;

namespace Anvil.Services
{
  public class OnELCValidationFailure
  {
    public bool IgnoreFailure { get; set; }
    public NwPlayer Player { get; init; }

    public int StrRef { get; set; }

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
    public Skill Skill { get; init; }
  }

  public sealed class OnELCFeatValidationFailure : OnELCValidationFailure
  {
    public Feat Feat { get; init; }
  }

  public sealed class OnELCSpellValidationFailure : OnELCValidationFailure
  {
    public Spell Spell { get; init; }
  }
}
