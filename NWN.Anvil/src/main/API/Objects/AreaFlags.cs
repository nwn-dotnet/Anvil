using System;

namespace Anvil.API
{
  [Flags]
  /// <summary>
  /// Flags that describe high-level properties of an area.
  /// These values can be combined to represent multiple properties at once.
  /// </summary>
  public enum AreaFlags
  {
    /// <summary>
    /// Area is an interior location.
    /// </summary>
    Interior = 0x0001,
    /// <summary>
    /// Area is located underground.
    /// </summary>
    UnderGround = 0x0002,
    /// <summary>
    /// Area is a natural (outdoor/cavern) environment.
    /// </summary>
    Natural = 0x0004,
  }
}
