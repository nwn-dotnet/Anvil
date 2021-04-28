using NWN.API.Constants;
using NWN.Native.API;

namespace NWN.API
{
  public sealed class PlayerQuickBarButton
  {
    public NwObject Item { get; set; }

    public NwObject SecondaryItem { get; set; }

    public QuickBarButtonType ObjectType { get; set; }

    public int MultiClass { get; set; }

    public string ResRef { get; set; }

    public string CommandLabel { get; set; }

    public string CommandLine { get; set; }

    public string ToolTip { get; set; }

    public int Param1 { get; set; }

    public byte MetaType { get; set; }

    public byte DomainLevel { get; set; }

    public int AssociateType { get; set; }

    public NwObject Associate { get; set; }

    public PlayerQuickBarButton() {}

    public PlayerQuickBarButton(CNWSQuickbarButton button)
    {
      Item = button.m_oidItem.ToNwObject();
      SecondaryItem = button.m_oidSecondaryItem.ToNwObject();
      ObjectType = (QuickBarButtonType)button.m_nObjectType;
      MultiClass = button.m_nMultiClass;
      ResRef = button.m_cResRef.GetResRefStr();
      CommandLabel = button.m_sCommandLabel.ToString();
      CommandLine = button.m_sCommandLine.ToString();
      ToolTip = button.m_sToolTip.ToString();
      Param1 = button.m_nINTParam1;
      MetaType = button.m_nMetaType;
      DomainLevel = button.m_nDomainLevel;
      AssociateType = button.m_nAssociateType;
      Associate = button.m_oidAssociate.ToNwObject();
    }

    internal void ApplyToNativeStructure(CNWSQuickbarButton button)
    {
      button.m_oidItem = Item;
      button.m_oidSecondaryItem = SecondaryItem;
      button.m_nObjectType = (byte)ObjectType;
      button.m_nMultiClass = button.m_nMultiClass;
      button.m_cResRef = new CResRef(ResRef);
      button.m_sCommandLabel = CommandLabel.ToExoString();
      button.m_sCommandLine = CommandLine.ToExoString();
      button.m_sToolTip = ToolTip.ToExoString();
      button.m_nINTParam1 = Param1;
      button.m_nMetaType = MetaType;
      button.m_nDomainLevel = DomainLevel;
      button.m_nAssociateType = (ushort)AssociateType;
      button.m_oidAssociate = Associate;
    }
  }
}
