namespace Anvil.API.Events
{
  public enum AttackResult
  {
    Unknown = 0,
    Hit = 1,
    Parried = 2,
    CriticalHit = 3,
    Miss = 4,
    Resisted = 5,
    AutomaticHit = 7,
    Concealed = 8,
    MissChance = 9,
    DevastatingCritical = 10,
  }
}
