namespace NWNX
{
  [NWNXPlugin(NWNX_Damage)]
  internal class DamagePlugin
  {
    public const string NWNX_Damage = "NWNX_Damage";

    /// /< @private
    /// / @struct NWNX_Damage_DamageEventData
    /// / @brief Damage Event Data
    /// / @struct NWNX_Damage_AttackEventData
    /// / @brief Attack Event Data
    /// / @struct NWNX_Damage_DamageData
    /// / @brief Used for DealDamage
    /// / @brief Sets the script to run with a damage event.
    /// / @param sScript The script that will handle the damage event.
    /// / @param oOwner An object if only executing for a specific object or OBJECT_INVALID for global.
    public static void SetDamageEventScript(string sScript, uint oOwner = NWN.NWScript.OBJECT_INVALID)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Damage, "SetDamageEventScript");
      NWN.Internal.NativeFunctions.StackPushObject(oOwner);
      NWN.Internal.NativeFunctions.StackPushString(sScript);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Get Damage Event Data
    /// / @return A NWNX_Damage_DamageEventData struct.
    /// / @note To use only in the Damage Event Script.
    public static DamageEventData GetDamageEventData()
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Damage, "GetDamageEventData");
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      DamageEventData retVal;
      retVal.iBase = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iSonic = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iPositive = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iNegative = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iFire = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iElectrical = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iDivine = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iCold = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iAcid = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iMagical = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iSlash = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iPierce = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iBludgeoning = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.oDamager = NWN.Internal.NativeFunctions.StackPopObject();
      return retVal;
    }

    /// / @brief Set Damage Event Data
    /// / @param data A NWNX_Damage_DamageEventData struct.
    /// / @note To use only in the Damage Event Script.
    public static void SetDamageEventData(DamageEventData data)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Damage, "SetDamageEventData");
      NWN.Internal.NativeFunctions.StackPushObject(data.oDamager);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iBludgeoning);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iPierce);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iSlash);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iMagical);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iAcid);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iCold);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iDivine);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iElectrical);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iFire);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iNegative);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iPositive);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iSonic);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iBase);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Sets the script to run with an attack event.
    /// / @param sScript The script that will handle the attack event.
    /// / @param oOwner An object if only executing for a specific object or OBJECT_INVALID for global.
    public static void SetAttackEventScript(string sScript, uint oOwner = NWN.NWScript.OBJECT_INVALID)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Damage, "SetAttackEventScript");
      NWN.Internal.NativeFunctions.StackPushObject(oOwner);
      NWN.Internal.NativeFunctions.StackPushString(sScript);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Get Attack Event Data
    /// / @return A NWNX_Damage_AttackEventData struct.
    /// / @note To use only in the Attack Event Script.
    public static AttackEventData GetAttackEventData()
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Damage, "GetAttackEventData");
      NWN.Internal.NativeFunctions.nwnxCallFunction();
      AttackEventData retVal;
      retVal.iSneakAttack = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iAttackType = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iAttackResult = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iAttackNumber = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iBase = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iSonic = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iPositive = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iNegative = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iFire = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iElectrical = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iDivine = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iCold = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iAcid = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iMagical = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iSlash = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iPierce = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.iBludgeoning = NWN.Internal.NativeFunctions.StackPopInteger();
      retVal.oTarget = NWN.Internal.NativeFunctions.StackPopObject();
      return retVal;
    }

    /// / @brief Set Attack Event Data
    /// / @param data A NWNX_Damage_AttackEventData struct.
    /// / @note To use only in the Attack Event Script.
    public static void SetAttackEventData(AttackEventData data)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Damage, "SetAttackEventData");
      NWN.Internal.NativeFunctions.StackPushObject(data.oTarget);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iBludgeoning);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iPierce);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iSlash);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iMagical);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iAcid);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iCold);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iDivine);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iElectrical);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iFire);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iNegative);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iPositive);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iSonic);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iBase);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iAttackNumber);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iAttackResult);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iAttackType);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iSneakAttack);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @brief Deal damage to a target.
    /// / @remark Permits multiple damage types and checks enhancement bonus for overcoming DR.
    /// / @param data A NWNX_Damage_DamageData struct.
    /// / @param oTarget The target object on whom the damage is dealt.
    /// / @param oSource The source of the damage.
    /// / @param iRanged Whether the attack should be treated as ranged by the engine (for example when considering damage inflicted by Acid Sheath and other such effects)
    public static void DealDamage(DamageData data, uint oTarget, uint oSource = NWN.NWScript.OBJECT_INVALID, int iRanged = NWN.NWScript.FALSE)
    {
      NWN.Internal.NativeFunctions.nwnxSetFunction(NWNX_Damage, "DealDamage");
      NWN.Internal.NativeFunctions.StackPushInteger(iRanged);
      NWN.Internal.NativeFunctions.StackPushObject(oSource);
      NWN.Internal.NativeFunctions.StackPushObject(oTarget);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iBludgeoning);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iPierce);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iSlash);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iMagical);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iAcid);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iCold);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iDivine);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iElectrical);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iFire);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iNegative);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iPositive);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iSonic);
      NWN.Internal.NativeFunctions.StackPushInteger(data.iPower);
      NWN.Internal.NativeFunctions.nwnxCallFunction();
    }

    /// / @}
  }

  public struct DamageEventData
  {
    public uint oDamager;
    public int iBludgeoning;
    public int iPierce;
    public int iSlash;
    public int iMagical;
    public int iAcid;
    public int iCold;
    public int iDivine;
    public int iElectrical;
    public int iFire;
    public int iNegative;
    public int iPositive;
    public int iSonic;
    public int iBase;
  }

  public struct AttackEventData
  {
    public uint oTarget;
    public int iBludgeoning;
    public int iPierce;
    public int iSlash;
    public int iMagical;
    public int iAcid;
    public int iCold;
    public int iDivine;
    public int iElectrical;
    public int iFire;
    public int iNegative;
    public int iPositive;
    public int iSonic;
    public int iBase;
    public int iAttackNumber;
    public int iAttackResult;
    public int iAttackType;
    public int iSneakAttack;
  }

  public struct DamageData
  {
    public int iBludgeoning;
    public int iPierce;
    public int iSlash;
    public int iMagical;
    public int iAcid;
    public int iCold;
    public int iDivine;
    public int iElectrical;
    public int iFire;
    public int iNegative;
    public int iPositive;
    public int iSonic;
    public int iPower;
  }
}
