using NWN.API;
using NWN.API.Constants;

namespace NWN.Services
{
  public class OnELCValidationFailure
  {
    public ValidationFailureType Type { get; init; }

    public ValidationFailureSubType SubType { get; init; }

    public int StrRef { get; set; }

    public bool IgnoreFailure { get; set; }
  }

  public sealed class OnELCItemValidationFailure : OnELCValidationFailure
  {
    public NwItem Item { get; init; }
  }

  public class OnELCLevelValidationFailure : OnELCValidationFailure
  {
    public int Level { get; init; }
  }

  public class OnELCSkillValidationFailutre : OnELCValidationFailure
  {
    public Skill Skill { get; init; }
  }

  public class OnELCFeatValidationFailutre : OnELCValidationFailure
  {
    public Feat Feat { get; init; }
  }

  public class OnELCSpellValidationFailutre : OnELCValidationFailure
  {
    public Spell Spell { get; init; }
  }
}
