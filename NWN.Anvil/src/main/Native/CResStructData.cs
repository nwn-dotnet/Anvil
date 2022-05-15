using System;
using System.Runtime.InteropServices;

namespace Anvil.Native
{
  [StructLayout(LayoutKind.Sequential)]
  public readonly struct CResStructData
  {
    public readonly CResStructHeaderData m_cHeader; // CResStructHeader
    public readonly IntPtr m_pLookUp; // CResStructLookUp*
    public readonly IntPtr m_pData; // void*
    public readonly IntPtr m_pWriteStructInternal; // CResStructInternal*
    public readonly int m_bIncludeStringHashTable; // BOOL
    public readonly IntPtr m_pStringFieldIDs; // CStringFieldIDType*
  }
}
