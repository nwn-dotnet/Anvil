using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public struct DamageData
  {
    public int Bludgeoning;
    public int Pierce;
    public int Slash;
    public int Magical;
    public int Acid;
    public int Cold;
    public int Divine;
    public int Electrical;
    public int Fire;
    public int Negative;
    public int Positive;
    public int Sonic;
    public int Base;

    internal static DamageData FromNative(AttackEventData attackEventData)
    {
      DamageData damageData;
      damageData.Bludgeoning = attackEventData.iBludgeoning;
      damageData.Pierce = attackEventData.iPierce;
      damageData.Slash = attackEventData.iSlash;
      damageData.Magical = attackEventData.iMagical;
      damageData.Acid = attackEventData.iAcid;
      damageData.Cold = attackEventData.iCold;
      damageData.Divine = attackEventData.iDivine;
      damageData.Electrical = attackEventData.iElectrical;
      damageData.Fire = attackEventData.iFire;
      damageData.Negative = attackEventData.iNegative;
      damageData.Positive = attackEventData.iPositive;
      damageData.Sonic = attackEventData.iSonic;
      damageData.Base = attackEventData.iBase;

      return damageData;
    }

    internal static DamageData FromNative(DamageEventData damageEventData)
    {
      DamageData damageData;
      damageData.Bludgeoning = damageEventData.iBludgeoning;
      damageData.Pierce = damageEventData.iPierce;
      damageData.Slash = damageEventData.iSlash;
      damageData.Magical = damageEventData.iMagical;
      damageData.Acid = damageEventData.iAcid;
      damageData.Cold = damageEventData.iCold;
      damageData.Divine = damageEventData.iDivine;
      damageData.Electrical = damageEventData.iElectrical;
      damageData.Fire = damageEventData.iFire;
      damageData.Negative = damageEventData.iNegative;
      damageData.Positive = damageEventData.iPositive;
      damageData.Sonic = damageEventData.iSonic;
      damageData.Base = damageEventData.iBase;

      return damageData;
    }

    internal DamageEventData ToNative(NwObject attacker)
    {
      DamageEventData damageEventData;
      damageEventData.oDamager = attacker;
      damageEventData.iBludgeoning = Bludgeoning;
      damageEventData.iPierce = Pierce;
      damageEventData.iSlash = Slash;
      damageEventData.iMagical = Magical;
      damageEventData.iAcid = Acid;
      damageEventData.iCold = Cold;
      damageEventData.iDivine = Divine;
      damageEventData.iElectrical = Electrical;
      damageEventData.iFire = Fire;
      damageEventData.iNegative = Negative;
      damageEventData.iPositive = Positive;
      damageEventData.iSonic = Sonic;
      damageEventData.iBase = Base;

      return damageEventData;
    }
  }
}