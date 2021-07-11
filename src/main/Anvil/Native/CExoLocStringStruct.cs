#pragma warning disable SA1307
#pragma warning disable SA1308

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

    public static unsafe explicit operator CExoLocString(CExoLocStringStruct locStringStruct)
    {
      return CExoLocString.FromPointer(&locStringStruct);
    }
  }
}
