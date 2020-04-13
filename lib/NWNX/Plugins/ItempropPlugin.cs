namespace NWNX
{
  [NWNXPlugin(NWNX_ItemProperty)]
  internal class ItempropPlugin
  {
    public const string NWNX_ItemProperty = "NWNX_ItemProperty";

    /// /< @private
    /// / @brief An unpacked itemproperty.
    /// / @brief Convert native itemproperty type to unpacked structure.
    /// / @param ip The itemproperty to convert.
    /// / @return A constructed NWNX_IPUnpacked.
    public static IPUnpacked UnpackIP(NWN.ItemProperty ip)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_ItemProperty, "UnpackIP");
      NWN.Internal.NativeFunctions.StackPushItemProperty(ip.Handle);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      IPUnpacked retVal;
      retVal.sTag = NWN.Internal.NativeFunctions.StackPopString();
      retVal.oCreator = NWN.Internal.NativeFunctions.StackPopObject();
      retVal.nSpellId = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.bUsable = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nChanceToAppear = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nUsesPerDay = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nParam1Value = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nParam1 = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nCostTableValue = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nCostTable = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nSubType = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nProperty = NWN.Internal.NativeFunctions.StackPopInteger();
      return retVal;
    }

    /// / @brief Convert unpacked itemproperty structure to native type.
    /// / @param ip The NWNX_IPUnpacked structure to convert.
    /// / @return The itemproperty.
    public static NWN.ItemProperty PackIP(IPUnpacked ip)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_ItemProperty, "PackIP");
      NWN.Internal.NativeFunctions.StackPushInteger(ip.nProperty);
      NWN.Internal.NativeFunctions.StackPushInteger(ip.nSubType);
      NWN.Internal.NativeFunctions.StackPushInteger(ip.nCostTable);
      NWN.Internal.NativeFunctions.StackPushInteger(ip.nCostTableValue);
      NWN.Internal.NativeFunctions.StackPushInteger(ip.nParam1);
      NWN.Internal.NativeFunctions.StackPushInteger(ip.nParam1Value);
      NWN.Internal.NativeFunctions.StackPushInteger(ip.nUsesPerDay);
      NWN.Internal.NativeFunctions.StackPushInteger(ip.nChanceToAppear);
      NWN.Internal.NativeFunctions.StackPushInteger(ip.bUsable);
      NWN.Internal.NativeFunctions.StackPushInteger(ip.nSpellId);
      NWN.Internal.NativeFunctions.StackPushObject(ip.oCreator);
      NWN.Internal.NativeFunctions.StackPushString(ip.sTag);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return new NWN.ItemProperty(NWN.Internal.NativeFunctions.StackPopItemProperty());
    }

    /// / @}
  }

  public struct IPUnpacked
  {
    public int nProperty;
    public int nSubType;
    public int nCostTable;
    public int nCostTableValue;
    public int nParam1;
    public int nParam1Value;
    public int nUsesPerDay;
    public int nChanceToAppear;
    public int bUsable;
    public int nSpellId;
    public uint oCreator;
    public string sTag;
  }
}
