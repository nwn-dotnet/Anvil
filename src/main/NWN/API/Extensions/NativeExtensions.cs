using System.Runtime.InteropServices;
using NWN.Native.API;

namespace NWN.API
{
  public static class NativeExtensions
  {
    public static CNWLevelStats Read(this SWIGTYPE_p_p_CNWLevelStats ptr)
      => new CNWLevelStats(Marshal.ReadIntPtr(ptr.Pointer), false);
  }
}
