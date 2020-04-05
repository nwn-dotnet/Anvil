using NWN;

namespace NWM.API.Constants
{
  public enum ProjectilePathType
  {
    Default = NWScript.PROJECTILE_PATH_TYPE_DEFAULT,
    Homing = NWScript.PROJECTILE_PATH_TYPE_HOMING,
    Ballistic = NWScript.PROJECTILE_PATH_TYPE_BALLISTIC,
    HighBallistic = NWScript.PROJECTILE_PATH_TYPE_HIGH_BALLISTIC,
    Accelerating = NWScript.PROJECTILE_PATH_TYPE_ACCELERATING
  }
}