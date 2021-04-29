using System.Runtime.InteropServices;

namespace NWN.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal unsafe struct CResRefStruct
  {
#pragma warning disable SA1307
#pragma warning disable SA1308
    public fixed byte m_resRefLowerCase[16];
    public fixed byte m_resRef[16];
#pragma warning restore SA1307
#pragma warning restore SA1308
  }
}
