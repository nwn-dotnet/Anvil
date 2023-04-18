using System;
using NWN.Core;

namespace Anvil.API
{
  [Flags]
  public enum SettleFlags
  {
    /// <summary>
    /// Reloads the area's grass. Use if the old tile, or new tile has grass.
    /// </summary>
    ReloadGrass = NWScript.SETTILE_FLAG_RELOAD_GRASS,

    /// <summary>
    /// Reloads the edge tile border. Use if the tile is on the edge of an area.
    /// </summary>
    ReloadBorder = NWScript.SETTILE_FLAG_RELOAD_BORDER,

    /// <summary>
    /// Recomputes the area lighting and static shadows. Use most of the time.
    /// </summary>
    RecomputeLighting = NWScript.SETTILE_FLAG_RECOMPUTE_LIGHTING,
  }
}
