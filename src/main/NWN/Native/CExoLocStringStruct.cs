using System;
using System.Runtime.InteropServices;
using NWN.Native.API;

namespace NWN.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct CExoLocStringStruct
  {
    public readonly IntPtr m_pExoLocStringInternal;
    public readonly uint m_dwStringRef;

    public static unsafe implicit operator CExoLocString(CExoLocStringStruct locStringStruct)
    {
      return new CExoLocString(&locStringStruct, false);
    }
  }
}
