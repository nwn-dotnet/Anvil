using System;
using NWN;

namespace NWM
{
  public static class InternalBootstrap
  {
    public static int Bootstrap(IntPtr arg, int argLength) => Internal.Bootstrap(arg, argLength);
  }
}