using System;

namespace Anvil.API
{
  [Flags]
  public enum ClassFeatListTypes : byte
  {
    /// <summary>
    /// Feat is automatically granted.
    /// </summary>
    Granted = 0,

    /// <summary>
    /// Feat is available on the normal feat list.
    /// </summary>
    Normal = 1 << 0,

    /// <summary>
    /// Feat is available on the class bonus feat list.
    /// </summary>
    Bonus = 1 << 1,
  }
}
