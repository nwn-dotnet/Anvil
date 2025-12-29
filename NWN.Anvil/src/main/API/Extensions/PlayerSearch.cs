using System;

namespace Anvil.API
{
  /// <summary>
  /// Specifies how to resolve an <see cref="NwPlayer"/> from an object ID.
  /// </summary>
  [Flags]
  public enum PlayerSearch
  {
    /// <summary>
    /// No player resolution.
    /// </summary>
    None = 0,

    /// <summary>
    /// Resolve by controlling player (directly controlled or possessed).
    /// </summary>
    Controlled = 1 << 0,

    /// <summary>
    /// Resolve by login player (initial owner/controlling player of the creature)
    /// </summary>
    Login = 1 << 1,

    /// <summary>
    /// Resolve using both controlled and login strategies.
    /// </summary>
    All = Controlled | Login,
  }
}
