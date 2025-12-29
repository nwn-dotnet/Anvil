namespace Anvil.API.Events
{
  /// <summary>
  /// Specifies override behavior when entering stealth mode.
  /// </summary>
  public enum StealthModeOverride
  {
    /// <summary>
    /// No override; use default behavior.
    /// </summary>
    None,

    /// <summary>
    /// Force entering stealth mode, temporarily granting Hide in Plain Sight if needed.
    /// </summary>
    ForceEnter,

    /// <summary>
    /// Prevent entering stealth via Hide in Plain Sight.
    /// </summary>
    PreventHIPSEnter,

    /// <summary>
    /// Prevent entering stealth mode.
    /// </summary>
    PreventEnter,
  }
}
