namespace Anvil.API.Events
{
  /// <summary>
  /// Indicates whether an attack qualifies for sneak/death attack bonuses.
  /// </summary>
  public enum SneakAttack
  {
    /// <summary>
    /// No sneak or death attack applies.
    /// </summary>
    None = 0,

    /// <summary>
    /// Sneak attack applies.
    /// </summary>
    SneakAttack = 1,

    /// <summary>
    /// Death attack applies.
    /// </summary>
    DeathAttack = 2,

    /// <summary>
    /// Both sneak and death attack apply.
    /// </summary>
    SneakDeathAttack = 3,
  }
}
