using System;
using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace NWN
{
  // @cond INTERNAL
  public static class Internal
  {
    [UsedImplicitly] // Called by NWNX
    public static int Bootstrap(IntPtr arg, int argLength)
    {
      return Anvil.AnvilCore.Init(arg, argLength);
    }
  }

  // @endcond
}
