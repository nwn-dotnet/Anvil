using System;
using NWN.Core;

namespace Anvil.API
{
  [Flags]
  public enum SettleFlag
  {
    ReloadGrass = NWScript.SETTILE_FLAG_RELOAD_GRASS,
    ReloadBorder = NWScript.SETTILE_FLAG_RELOAD_BORDER,
    RecomputeLighting = NWScript.SETTILE_FLAG_RECOMPUTE_LIGHTING,
  }
}
