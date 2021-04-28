using System.Runtime.InteropServices;
using NWN.Native.API;

namespace NWN.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly unsafe struct CExoStringStruct
  {
    public readonly char* m_sString;
    public readonly uint m_nBufferLength;

    public static implicit operator CExoString(CExoStringStruct exoStringStruct)
    {
      return new CExoString(&exoStringStruct, false);
    }
  }
}
