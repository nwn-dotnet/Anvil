using NWN.API;
using NWN.API.Constants;

namespace NWN.Services
{
  public class OnELCValidationFailure
  {
    public NwPlayer Player { get; init; }

    public ValidationFailureType Type { get; init; }

    public ValidationFailureSubType SubType { get; init; }

    public int StrRef { get; set; }

    public bool IgnoreFailure { get; set; }
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
