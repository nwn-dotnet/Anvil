using NWN.API;

namespace NWN.Services
{
  public sealed class DevastatingCriticalData
  {
    public readonly NwItem Weapon;
    public readonly NwGameObject Target;
    public int Damage;
    public bool Bypass;
  }
}
