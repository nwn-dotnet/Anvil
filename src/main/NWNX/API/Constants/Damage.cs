namespace NWNX.API.Constants
{
  public enum AttackType
  {
    MainHand = 1,
    OffHand = 2,
    CreatureLeft = 3,
    CreatureRight = 4,
    CreatureSpecial = 5,
    Haste = 6
  }

  public enum AttackResult
  {
    Hit = 1,
    CriticalHit = 3,
    Miss = 4,
    Concealed = 8
  }

  public enum SneakAttack
  {
    None = 0,
    SneakAttack = 1,
    DeathAttack = 2,
    SneakAndDeathAttack = 3
  }
}
