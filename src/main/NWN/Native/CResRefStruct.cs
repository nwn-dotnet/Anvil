using System.Runtime.InteropServices;

namespace NWN.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly unsafe struct CResRefStruct
  {
    public readonly char* m_resRefLowerCase;
    public readonly char* m_resRef;
  }
}
