using System.Numerics;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed class EncounterSpawnPoint(CEncounterSpawnPoint spawnPoint)
  {
    public float Orientation => spawnPoint.m_fOrientation;

    public Vector3 Position => spawnPoint.m_vPosition.ToManagedVector();
  }
}
