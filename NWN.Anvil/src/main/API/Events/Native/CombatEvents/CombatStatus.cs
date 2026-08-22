namespace Anvil.API.Events
{
  /// <summary>
  /// Indicates a player's combat status change.
  /// </summary>
  public enum CombatStatus
  {
    /// <summary>
    /// The player entered combat.
    /// </summary>
    EnterCombat,

    /// <summary>
    /// The player exited combat.
    /// </summary>
    ExitCombat,
  }
}
