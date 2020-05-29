using NWM.API;
using NWMX.API.Constants;
using NWNX;

namespace NWMX.API.Events
{
  public struct AttackData
  {
    public int AttackNumber;
    public AttackResult AttackResult;
    public AttackType AttackType;
    public SneakAttack SneakAttack;

    internal static AttackData FromNative(AttackEventData attackEventData)
    {
      AttackData attackData;
      attackData.AttackNumber = attackEventData.iAttackNumber;
      attackData.AttackResult = (AttackResult) attackEventData.iAttackResult;
      attackData.AttackType = (AttackType) attackEventData.iAttackType;
      attackData.SneakAttack = (SneakAttack) attackEventData.iSneakAttack;

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
      attackEventData.iAttackType = (int) AttackType;
      attackEventData.iSneakAttack = (int) SneakAttack;

      return attackEventData;
    }
  }
}