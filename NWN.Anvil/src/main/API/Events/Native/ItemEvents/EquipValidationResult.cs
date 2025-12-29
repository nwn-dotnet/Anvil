namespace Anvil.API.Events
{
  /// <summary>
  /// Represents the result of validating whether an item can be equipped.
  /// </summary>
  public enum EquipValidationResult
  {
    /// <summary>
    /// Equipping is not allowed.
    /// </summary>
    Denied = 0,

    /// <summary>
    /// Equipping is allowed.
    /// </summary>
    Okay = 1,

    /// <summary>
    /// Equipping is allowed and will replace the current item in the slot.
    /// </summary>
    ReplaceCurrentItem = 2,
  }
}
