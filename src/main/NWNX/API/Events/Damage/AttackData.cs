using NWN.API;
using NWN.Core.NWNX;
using NWNX.API.Constants;

namespace NWNX.API.Events
{
  public struct AttackData
  {
    public int AttackNumber;
    public AttackResult AttackResult;
    public WeaponAttackType WeaponAttackType;
    public SneakAttack SneakAttack;
    public bool KillingBlow;
    public int AttackType;

    internal static AttackData FromNative(AttackEventData attackEventData)
    {
      AttackData attackData;
      attackData.AttackNumber = attackEventData.iAttackNumber;
      attackData.AttackResult = (AttackResult) attackEventData.iAttackResult;
      attackData.WeaponAttackType = (WeaponAttackType) attackEventData.iWeaponAttackType;
      attackData.SneakAttack = (SneakAttack) attackEventData.iSneakAttack;
      attackData.KillingBlow = attackEventData.bKillingBlow.ToBool();
      attackData.AttackType = attackEventData.iAttackType;

      return attackData;
    }

    internal AttackEventData ToNative(NwObject target, DamageData damageData)
    {
      AttackEventData attackEventData;
      attackEventData.oTarget = target;
      attackEventData.iBludgeoning = damageData.Bludgeoning;
      attackEventData.iPierce = damageData.Pierce;
      attackEventData.iSlash = damageData.Slash;
      attackEventData.iMagical = damageData.Magical;
      attackEventData.iAcid = damageData.Acid;
      attackEventData.iCold = damageData.Cold;
      attackEventData.iDivine = damageData.Divine;
      attackEventData.iElectrical = damageData.Electrical;
      attackEventData.iFire = damageData.Fire;
      attackEventData.iNegative = damageData.Negative;
      attackEventData.iPositive = damageData.Positive;
      attackEventData.iSonic = damageData.Sonic;
      attackEventData.iBase = damageData.Base;

      attackEventData.iAttackNumber = AttackNumber;
      attackEventData.iAttackResult = (int) AttackResult;
      attackEventData.iWeaponAttackType = (int) WeaponAttackType;
      attackEventData.iSneakAttack = (int) SneakAttack;
      attackEventData.bKillingBlow = KillingBlow.ToInt();
      attackEventData.iAttackType = AttackType;

      return attackEventData;
    }
  }
}
