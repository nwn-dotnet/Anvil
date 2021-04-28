using System.Runtime.InteropServices;
using NWN.Native.API;

namespace NWN.Native
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct CNWSQuickbarButtonStruct
  {
#pragma warning disable SA1307
#pragma warning disable SA1308
    public readonly uint m_oidItem;
    public readonly uint m_oidSecondaryItem;
    public readonly byte m_nObjectType;
    public readonly byte m_nMultiClass;
    public readonly CResRefStruct m_cResRef;
    public readonly CExoStringStruct m_sCommandLabel;
    public readonly CExoStringStruct m_sCommandLine;
    public readonly CExoStringStruct m_sToolTip;
    public readonly int m_nINTParam1;
    public readonly byte m_nMetaType;
    public readonly byte m_nDomainLevel;
    public readonly ushort m_nAssociateType;
    public readonly uint m_oidAssociate;
#pragma warning restore SA1307
#pragma warning restore SA1308

    public static unsafe implicit operator CNWSQuickbarButton(CNWSQuickbarButtonStruct quickBarButtonStruct)
    {
      return new CNWSQuickbarButton(&quickBarButtonStruct, false);
    }
  }
}
