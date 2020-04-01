using System;
using System.Runtime.CompilerServices;

namespace NWM
{
  public static class InternalBootstrap
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Bootstrap(IntPtr arg, int argLength) => NWN.Internal.Bootstrap(arg, argLength);
  }
}