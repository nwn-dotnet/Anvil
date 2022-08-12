using System;
using System.Runtime.InteropServices;
using NWN.Native.API;

namespace Anvil.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct CExoStringData
  {
    public readonly IntPtr m_sString; // char*
    public readonly uint m_nBufferLength; // uint32_t

    public override string? ToString()
    {
      return m_sString != IntPtr.Zero ? m_sString.ReadNullTerminatedString() : null;
    }
  }
}
