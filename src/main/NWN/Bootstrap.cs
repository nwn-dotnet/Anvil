using System;
using JetBrains.Annotations;
using NWN.Services;

namespace NWN
{
  public static class Internal
  {
    [UsedImplicitly] // Called by NWNX
    public static int Bootstrap(IntPtr arg, int argLength)
      => NManager.Init(arg, argLength, new ServiceBindingInstaller());
  }
}