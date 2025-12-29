namespace Anvil.API.Events
{
  /// <summary>
  /// An override value for a creature's item proficiency check.
  /// </summary>
  public enum CheckProficiencyOverride
  {
    /// <summary>
    /// No override; defer to the default proficiency behavior.
    /// </summary>
    None = 0,

    /// <summary>
    /// Treat the creature as having proficiency for the item.
    /// </summary>
    HasProficiency,

    /// <summary>
    /// Treat the creature as lacking proficiency for the item.
    /// </summary>
    NoProficiency,
  }
}
