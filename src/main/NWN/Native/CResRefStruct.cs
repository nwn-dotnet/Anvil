using System.Runtime.InteropServices;

namespace NWN.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly unsafe struct CResRefStruct
  {
#pragma warning disable SA1307
#pragma warning disable SA1308
    public readonly char* m_resRefLowerCase;
    public readonly char* m_resRef;
#pragma warning restore SA1307
#pragma warning restore SA1308
  }
}
