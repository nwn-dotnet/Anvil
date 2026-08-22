namespace Anvil.API.Events
{
  /// <summary>
  /// Represents the resolved outcome of an individual attack.
  /// </summary>
  public enum AttackResult
  {
    /// <summary>
    /// Outcome could not be determined.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The attack hit the target.
    /// </summary>
    Hit = 1,

    /// <summary>
    /// The attack was parried.
    /// </summary>
    Parried = 2,

    /// <summary>
    /// The attack resulted in a critical hit.
    /// </summary>
    CriticalHit = 3,

    /// <summary>
    /// The attack missed the target.
    /// </summary>
    Miss = 4,

    /// <summary>
    /// The attack was resisted.
    /// </summary>
    Resisted = 5,

    /// <summary>
    /// The attack was an automatic hit.
    /// </summary>
    AutomaticHit = 7,

    /// <summary>
    /// The target was concealed.
    /// </summary>
    Concealed = 8,

    /// <summary>
    /// The attack failed due to miss chance.
    /// </summary>
    MissChance = 9,

    /// <summary>
    /// The attack resulted in a devastating critical hit.
    /// </summary>
    DevastatingCritical = 10,
  }
}
