namespace Anvil.API.Events
{
  /// <summary>
  /// Specifies how to override the engine's behavior when applying a new combat mode.
  /// </summary>
  public enum ForceNewModeOverride
  {
    /// <summary>
    /// No override; use the default engine behavior.
    /// </summary>
    None,

    /// <summary>
    /// Force the new combat mode to apply.
    /// </summary>
    Force,

    /// <summary>
    /// Do not force the new combat mode.
    /// </summary>
    DontForce,
  }
}
