namespace Anvil.API.Events
{
  /// <summary>
  /// Specifies the type of dispel magic effect.
  /// </summary>
  public enum DispelMagicType
  {
    /// <summary>
    /// Attempts to dispel the single strongest effect.
    /// </summary>
    Best,

    /// <summary>
    /// Attempts to dispel all eligible effects.
    /// </summary>
    All,
  }
}
