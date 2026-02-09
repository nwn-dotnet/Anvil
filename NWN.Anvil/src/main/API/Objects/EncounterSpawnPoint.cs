using System.Numerics;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// Describes a specific spawn location used by an encounter, including position and facing.
  /// </summary>
  /// <param name="spawnPoint">The native spawn point backing this wrapper.</param>
  public sealed class EncounterSpawnPoint(CEncounterSpawnPoint spawnPoint)
  {
    /// <summary>
    /// Gets the facing angle in degrees for creatures spawned at this point.
    /// </summary>
    public float Orientation => spawnPoint.m_fOrientation;

    /// <summary>
    /// Gets the world position where creatures will be created.
    /// </summary>
    public Vector3 Position => spawnPoint.m_vPosition.ToManagedVector();
  }
}
