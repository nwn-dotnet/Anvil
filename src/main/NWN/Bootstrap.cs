using System;
using JetBrains.Annotations;

namespace NWN
{
  // @cond INTERNAL
  public static class Internal
  {
    [UsedImplicitly] // Called by NWNX
    public static int Bootstrap(IntPtr arg, int argLength)
      => NManager.Init(arg, argLength);
  }

  // @endcond
}
