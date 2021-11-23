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

    public Vector3 Position
    {
      get => spawnPoint.m_vPosition.ToManagedVector();
    }

    public float Orientation
    {
      get => spawnPoint.m_fOrientation;
    }
  }
}
