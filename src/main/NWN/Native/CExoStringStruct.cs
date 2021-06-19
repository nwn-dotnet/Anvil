#pragma warning disable SA1307
#pragma warning disable SA1308

using System;
using System.Runtime.InteropServices;
using NWN.Native.API;

namespace NWN.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct CExoStringStruct
  {
    public readonly IntPtr m_sString;
    public readonly uint m_nBufferLength;

    public static unsafe explicit operator CExoString(CExoStringStruct stringStruct)
    {
      return CExoString.FromPointer(&stringStruct);
    }
  }
}
