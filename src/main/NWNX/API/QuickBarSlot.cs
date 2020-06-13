using NWN.API;
using NWNX.API.Constants;

namespace NWNX.API
{
  public struct QuickBarSlot
  {
    public NwObject Item { get; set; }
    public NwObject SecondaryItem { get; set; }
    public QuickBarSlotType ObjectType { get; set; }
    public int MultiClass { get; set; }
    public string Resref { get; set; }
    public string CommandLabel { get; set; }
    public string CommandLine { get; set; }
    public string ToolTip { get; set; }
    public int INTParam1 { get; set; }
    public int MetaType { get; set; }
    public int DomainLevel { get; set; }
    public int AssociateType { get; set; }
    public NwObject Associate { get; set; }

    public static implicit operator QuickBarSlot(NWN.Core.NWNX.QuickBarSlot slot)
    {
      return new QuickBarSlot
      {
        Item = slot.oItem.ToNwObject(),
        SecondaryItem = slot.oItem.ToNwObject(),
        ObjectType = (QuickBarSlotType) slot.nObjectType,
        MultiClass = slot.nMultiClass,
        Resref = slot.sResRef,
        CommandLabel = slot.sCommandLabel,
        CommandLine = slot.sCommandLine,
        ToolTip = slot.sToolTip,
        INTParam1 = slot.nINTParam1,
        MetaType = slot.nMetaType,
        DomainLevel = slot.nDomainLevel,
        AssociateType = slot.nAssociateType,
        Associate = slot.oAssociate.ToNwObject()
      };
    }

    public static implicit operator NWN.Core.NWNX.QuickBarSlot(QuickBarSlot slot)
    {
      return new NWN.Core.NWNX.QuickBarSlot
      {
        oItem = slot.Item,
        oSecondaryItem = slot.SecondaryItem,
        nObjectType = (int) slot.ObjectType,
        nMultiClass = slot.MultiClass,
        sResRef = slot.Resref,
        sCommandLabel = slot.CommandLabel,
        sCommandLine = slot.CommandLine,
        sToolTip = slot.ToolTip,
        nINTParam1 = slot.INTParam1,
        nMetaType = slot.MetaType,
        nDomainLevel = slot.DomainLevel,
        nAssociateType = slot.AssociateType,
        oAssociate = slot.Associate
      };
    }
  }
}