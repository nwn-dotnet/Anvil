using System.Numerics;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed class EncounterSpawnPoint
  {
    private readonly CEncounterSpawnPoint spawnPoint;

    public EncounterSpawnPoint(CEncounterSpawnPoint spawnPoint)
    {
      this.spawnPoint = spawnPoint;
    }

    public float Orientation => spawnPoint.m_fOrientation;

    public Vector3 Position => spawnPoint.m_vPosition.ToManagedVector();
  }
}
