using System;
using System.Runtime.InteropServices;

namespace Anvil.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct CExoLocStringData
  {
    public readonly IntPtr m_pExoLocStringInternal; // CExoLocStringInternal*
    public readonly uint m_dwStringRef; // STRREF
  }
}
