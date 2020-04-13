namespace NWNX
{
  [NWNXPlugin(NWNX_Effect)]
  internal class EffectPlugin
  {
    public const string NWNX_Effect = "NWNX_Effect";

    /// /< @private
    /// / An unpacked effect
    /// / @brief Convert native effect type to unpacked structure.
    /// / @param e The effect to convert.
    /// / @return A constructed NWNX_EffectUnpacked.
    public static EffectUnpacked UnpackEffect(NWN.Effect e)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Effect, "UnpackEffect");
      NWN.Internal.NativeFunctions.StackPushEffect(e.Handle);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      EffectUnpacked retVal;
      retVal.sTag = NWN.Internal.NativeFunctions.StackPopString();
      retVal.oParam3 = NWN.Internal.NativeFunctions.StackPopObject();
      retVal.oParam2 = NWN.Internal.NativeFunctions.StackPopObject();
      retVal.oParam1 = NWN.Internal.NativeFunctions.StackPopObject();
      retVal.oParam0 = NWN.Internal.NativeFunctions.StackPopObject();
      retVal.sParam5 = NWN.Internal.NativeFunctions.StackPopString();
      retVal.sParam4 = NWN.Internal.NativeFunctions.StackPopString();
      retVal.sParam3 = NWN.Internal.NativeFunctions.StackPopString();
      retVal.sParam2 = NWN.Internal.NativeFunctions.StackPopString();
      retVal.sParam1 = NWN.Internal.NativeFunctions.StackPopString();
      retVal.sParam0 = NWN.Internal.NativeFunctions.StackPopString();
      retVal.fParam3 = NWN.Internal.NativeFunctions.StackPopFloat();
      retVal.fParam2 = NWN.Internal.NativeFunctions.StackPopFloat();
      retVal.fParam1 = NWN.Internal.NativeFunctions.StackPopFloat();
      retVal.fParam0 = NWN.Internal.NativeFunctions.StackPopFloat();
      retVal.nParam7 = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nParam6 = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nParam5 = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nParam4 = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nParam3 = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nParam2 = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nParam1 = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nParam0 = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nNumIntegers = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.bLinkRightValid = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.eLinkRight = new NWN.Effect(NWN.Internal.NativeFunctions.StackPopEffect());
      retVal.bLinkLeftValid = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.eLinkLeft = new NWN.Effect(NWN.Internal.NativeFunctions.StackPopEffect());
      retVal.nCasterLevel = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.bShowIcon = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.bExpose = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nSpellId = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.oCreator = NWN.Internal.NativeFunctions.StackPopObject();
      retVal.nExpiryTimeOfDay = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nExpiryCalendarDay = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.fDuration = NWN.Internal.NativeFunctions.StackPopFloat();
      retVal.nSubType = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.nType = NWN.Internal.NativeFunctions.StackPopInteger();
      return retVal;
    }

    /// / @brief Convert unpacked effect structure to native type.
    /// / @param e The NWNX_EffectUnpacked structure to convert.
    /// / @return The effect.
    public static NWN.Effect PackEffect(EffectUnpacked e)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Effect, "PackEffect");
      NWN.Internal.NativeFunctions.StackPushInteger(e.nType);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nSubType);
      NWN.Internal.NativeFunctions.StackPushFloat(e.fDuration);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nExpiryCalendarDay);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nExpiryTimeOfDay);
      NWN.Internal.NativeFunctions.StackPushObject(e.oCreator);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nSpellId);
      NWN.Internal.NativeFunctions.StackPushInteger(e.bExpose);
      NWN.Internal.NativeFunctions.StackPushInteger(e.bShowIcon);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nCasterLevel);
      NWN.Internal.NativeFunctions.StackPushEffect(e.eLinkLeft.Handle);
      NWN.Internal.NativeFunctions.StackPushInteger(e.bLinkLeftValid);
      NWN.Internal.NativeFunctions.StackPushEffect(e.eLinkRight.Handle);
      NWN.Internal.NativeFunctions.StackPushInteger(e.bLinkRightValid);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nNumIntegers);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nParam0);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nParam1);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nParam2);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nParam3);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nParam4);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nParam5);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nParam6);
      NWN.Internal.NativeFunctions.StackPushInteger(e.nParam7);
      NWN.Internal.NativeFunctions.StackPushFloat(e.fParam0);
      NWN.Internal.NativeFunctions.StackPushFloat(e.fParam1);
      NWN.Internal.NativeFunctions.StackPushFloat(e.fParam2);
      NWN.Internal.NativeFunctions.StackPushFloat(e.fParam3);
      NWN.Internal.NativeFunctions.StackPushString(e.sParam0);
      NWN.Internal.NativeFunctions.StackPushString(e.sParam1);
      NWN.Internal.NativeFunctions.StackPushString(e.sParam2);
      NWN.Internal.NativeFunctions.StackPushString(e.sParam3);
      NWN.Internal.NativeFunctions.StackPushString(e.sParam4);
      NWN.Internal.NativeFunctions.StackPushString(e.sParam5);
      NWN.Internal.NativeFunctions.StackPushObject(e.oParam0);
      NWN.Internal.NativeFunctions.StackPushObject(e.oParam1);
      NWN.Internal.NativeFunctions.StackPushObject(e.oParam2);
      NWN.Internal.NativeFunctions.StackPushObject(e.oParam3);
      NWN.Internal.NativeFunctions.StackPushString(e.sTag);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return new NWN.Effect(NWN.Internal.NativeFunctions.StackPopEffect());
    }

    /// / @brief Set a script with optional data that runs when an effect expires
    /// / @param e The effect.
    /// / @param script The script to run when the effect expires.
    /// / @param data Any other data you wish to send back to the script.
    /// / @remark OBJECT_SELF in the script is the object the effect is applied to.
    /// / @note Only works for TEMPORARY and PERMANENT effects applied to an object.
    public static NWN.Effect SetEffectExpiredScript(NWN.Effect e, string script, string data = "")
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Effect, "SetEffectExpiredScript");
      NWN.Internal.NativeFunctions.StackPushString(data);
      NWN.Internal.NativeFunctions.StackPushString(script);
      NWN.Internal.NativeFunctions.StackPushEffect(e.Handle);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return new NWN.Effect(NWN.Internal.NativeFunctions.StackPopEffect());
    }

    /// / @brief Get the data set with NWNX_Effect_SetEffectExpiredScript()
    /// / @note Should only be called from a script set with NWNX_Effect_SetEffectExpiredScript().
    /// / @return The data attached to the effect.
    public static string GetEffectExpiredData()
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Effect, "GetEffectExpiredData");
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopString();
    }

    /// / @brief Get the effect creator.
    /// / @note Should only be called from a script set with NWNX_Effect_SetEffectExpiredScript().
    /// / @return The object from which the effect originated.
    public static uint GetEffectExpiredCreator()
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Effect, "GetEffectExpiredCreator");
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      return NWN.Internal.NativeFunctions.StackPopObject();
    }

    /// / @}
  }

  public struct EffectUnpacked
  {
    public int nType;
    public int nSubType;
    public float fDuration;
    public int nExpiryCalendarDay;
    public int nExpiryTimeOfDay;
    public uint oCreator;
    public int nSpellId;
    public int bExpose;
    public int bShowIcon;
    public int nCasterLevel;
    public NWN.Effect eLinkLeft;
    public int bLinkLeftValid;
    public NWN.Effect eLinkRight;
    public int bLinkRightValid;
    public int nNumIntegers;
    public int nParam0;
    public int nParam1;
    public int nParam2;
    public int nParam3;
    public int nParam4;
    public int nParam5;
    public int nParam6;
    public int nParam7;
    public float fParam0;
    public float fParam1;
    public float fParam2;
    public float fParam3;
    public string sParam0;
    public string sParam1;
    public string sParam2;
    public string sParam3;
    public string sParam4;
    public string sParam5;
    public uint oParam0;
    public uint oParam1;
    public uint oParam2;
    public uint oParam3;
    public string sTag;
  }
}
