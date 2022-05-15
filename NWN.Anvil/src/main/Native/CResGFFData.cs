using System;
using System.Runtime.InteropServices;

namespace Anvil.Native
{
  [StructLayout(LayoutKind.Sequential)]
  public unsafe struct CResGFFData
  {
    public readonly IntPtr VPtr;

    // CRes
    public readonly ushort m_nDemands; // uint16_t
    public readonly ushort m_nRequests; // uint16_t
    public readonly RESIDData m_nID; // RESID
    public readonly uint m_status; // uint32_t
    public readonly IntPtr m_pResource; // void*
    public readonly IntPtr m_pKeyEntry; // CKeyTableEntry*
    public readonly uint m_nSize; // uint32_t
    public readonly uint m_nRefs; // uint32_t
    public readonly int m_bAllocHeaderData; // BOOL
    public readonly int m_bAllocTrailingData; // BOOL
    public readonly IntPtr m_pos; // CExoLinkedListPosition
    public readonly int m_bAllowCaching; // BOOL

    // CResGff
    public readonly uint STRUCT_GROWSIZE; // uint32_t
    public readonly uint FIELD_GROWSIZE; // uint32_t
    public readonly uint LABEL_GROWSIZE; // uint32_t
    public readonly uint DATAFIELD_GROWSIZE; // uint32_t
    public readonly uint DATALAYOUTFIELD_GROWSIZE; // uint32_t
    public readonly uint DATALAYOUTLIST_GROWSIZE; // uint32_t
    public readonly IntPtr m_pFileHeader; // CResGFFFileHeader*
    public readonly IntPtr m_pDDFileHeader; // uint8_t*
    public readonly uint m_nHeaderOffset; // uint32_t
    public readonly IntPtr m_pStruct; // CResGFFStruct*
    public readonly uint m_nStructAllocated; // uint32_t
    public readonly IntPtr m_pField; // CResGFFField*
    public readonly uint m_nFieldAllocated; // uint32_t
    public readonly IntPtr m_pLabel; // CResGFFLabel*
    public readonly uint m_nLabelAllocated; // uint32_t
    public readonly IntPtr m_pDataField; // uint8_t*
    public readonly uint m_nDataFieldAllocated; // uint32_t
    public readonly IntPtr m_pDataLayoutField; // uint8_t*
    public readonly uint m_nDataLayoutFieldAllocated; // uint32_t
    public readonly uint m_nDataLayoutFieldWasted; // uint32_t
    public readonly IntPtr m_pDataLayoutList; // uint8_t*
    public readonly uint m_nDataLayoutListAllocated; // uint32_t
    public readonly uint m_nDataLayoutListWasted; // uint32_t
    public fixed byte m_pLabelBuffer[17]; // char m_pLabelBuffer[17];
    public fixed byte m_pFileType[4]; // char m_pFileType[4];
    public readonly int m_bLoaded; // BOOL
    public readonly int m_bResourceLoaded; // BOOL
    public readonly int m_bSelfDemanded; // BOOL
    public readonly int m_bDataPtrOwned; // BOOL
    public readonly int m_bReplaceExistingFields; // BOOL
    public readonly int m_bValidationFailed; // BOOL
  }
}
