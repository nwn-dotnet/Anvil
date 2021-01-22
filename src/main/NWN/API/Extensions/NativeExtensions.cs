using System.Runtime.InteropServices;
using NWN.Native.API;

namespace NWN.API
{
  public static class NativeExtensions
  {
    public static short Read(this SWIGTYPE_p_short ptr)
      => Marshal.PtrToStructure<short>(ptr.Pointer);

    public static void Write(this SWIGTYPE_p_short ptr, short value)
      => Marshal.StructureToPtr(value, ptr.Pointer, false);

    public static int Read(this SWIGTYPE_p_int ptr)
      => Marshal.PtrToStructure<int>(ptr.Pointer);

    public static void Write(this SWIGTYPE_p_int ptr, int value)
      => Marshal.StructureToPtr(value, ptr.Pointer, false);

    public static ushort Read(this SWIGTYPE_p_unsigned_short ptr)
      => Marshal.PtrToStructure<ushort>(ptr.Pointer);

    public static void Write(this SWIGTYPE_p_unsigned_short ptr, ushort value)
      => Marshal.StructureToPtr(value, ptr.Pointer, false);
  }
}
