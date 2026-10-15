using NWN.Core;

namespace Anvil.API
{
  public enum ProjectilePathType : uint
  {
    Default = NWScript.PROJECTILE_PATH_TYPE_DEFAULT,
    Homing = NWScript.PROJECTILE_PATH_TYPE_HOMING,
    Ballistic = NWScript.PROJECTILE_PATH_TYPE_BALLISTIC,
    HighBallistic = NWScript.PROJECTILE_PATH_TYPE_HIGH_BALLISTIC,
    Accelerating = NWScript.PROJECTILE_PATH_TYPE_ACCELERATING,
    BurstUp = 5,
    Spiral = 6,
    Linked = 7,
    Bounce = 8,
    Burst = 9,
    LinkedBurstUp = 10,
    TripleBallisticHit = 11,
    TripleBallisticMiss = 12,
    DoubleBallistic = 13,
  }
}
