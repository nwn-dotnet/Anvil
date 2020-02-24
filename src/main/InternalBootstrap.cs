using System;

namespace NWM
{
  public static class InternalBootstrap
  {
    public static int Bootstrap(IntPtr arg, int argLength) => NWN.Internal.Bootstrap(arg, argLength);
  }
}