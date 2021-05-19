using System;
using System.Runtime.InteropServices;
using NWN.Native.API;

namespace NWN.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct CExoLocStringStruct
  {
#pragma warning disable SA1307
#pragma warning disable SA1308
    public readonly IntPtr m_pExoLocStringInternal;
    public readonly uint m_dwStringRef;
#pragma warning restore SA1307
#pragma warning restore SA1308

    public static unsafe implicit operator CExoLocString(CExoLocStringStruct locStringStruct)
    {
      return CExoLocString.FromPointer(&locStringStruct);
    }
  }
}
