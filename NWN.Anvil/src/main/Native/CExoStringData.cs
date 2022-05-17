using System;
using System.Runtime.InteropServices;

namespace Anvil.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct CExoStringData
  {
    public readonly IntPtr m_sString; // char*
    public readonly uint m_nBufferLength; // uint32_t
  }
}
