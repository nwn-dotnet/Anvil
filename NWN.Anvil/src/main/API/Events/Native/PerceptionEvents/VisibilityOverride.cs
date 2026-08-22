namespace Anvil.API.Events
{
  /// <summary>
  /// Controls whether detection should be forced to succeed or fail, or use default behavior.
  /// </summary>
  public enum VisibilityOverride
  {
    /// <summary>
    /// Use the engine's default detection behavior.
    /// </summary>
    None,

    /// <summary>
    /// Force detection to succeed (target is considered visible).
    /// </summary>
    Visible,

    /// <summary>
    /// Force detection to fail (target is considered not visible).
    /// </summary>
    NotVisible,
  }
}
