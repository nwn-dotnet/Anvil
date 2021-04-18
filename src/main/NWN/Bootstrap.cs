using System;
using Anvil;
using JetBrains.Annotations;

namespace NWN
{
  // @cond INTERNAL
  public static class Internal
  {
    [UsedImplicitly] // Called by NWNX
    public static int Bootstrap(IntPtr arg, int argLength)
      => Anvil.AnvilCore.Init(arg, argLength);
  }

  // @endcond
}
