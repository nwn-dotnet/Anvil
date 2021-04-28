using System.Runtime.InteropServices;
using NWN.Native.API;

namespace NWN.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly unsafe struct CExoStringStruct
  {
#pragma warning disable SA1307
#pragma warning disable SA1308
    public readonly char* m_sString;
    public readonly uint m_nBufferLength;
#pragma warning restore SA1307
#pragma warning restore SA1308

    public static implicit operator CExoString(CExoStringStruct exoStringStruct)
    {
      return new CExoString(&exoStringStruct, false);
    }
  }
}
