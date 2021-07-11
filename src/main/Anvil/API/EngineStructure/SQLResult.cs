using System.Numerics;
using NWN.Core;

namespace Anvil.API
{
  public sealed class SQLResult
  {
    private readonly SQLQuery query;

    internal SQLResult(SQLQuery query)
    {
      this.query = query;
    }

    /// <summary>
    /// Gets the int result for the specified column.
    /// </summary>
    /// <param name="columnIndex">The 0-based index of the column to query.</param>
    /// <returns>The int result. Returns 0 on an error.</returns>
    public int GetInt(int columnIndex)
    {
      return NWScript.SqlGetInt(query, columnIndex);
    }

    /// <summary>
    /// Gets the float result for the specified column.
    /// </summary>
    /// <param name="columnIndex">The 0-based index of the column to query.</param>
    /// <returns>The float result. Returns 0.0f on an error.</returns>
    public float GetFloat(int columnIndex)
    {
      return NWScript.SqlGetFloat(query, columnIndex);
    }

    /// <summary>
    /// Gets the string result for the specified column.
    /// </summary>
    /// <param name="columnIndex">The 0-based index of the column to query.</param>
    /// <returns>The string result. Returns "" on an error.</returns>
    public string GetString(int columnIndex)
    {
      return NWScript.SqlGetString(query, columnIndex);
    }

    /// <summary>
    /// Gets the <see cref="Vector3"/> result for the specified column.
    /// </summary>
    /// <param name="columnIndex">The 0-based index of the column to query.</param>
    /// <returns>The <see cref="Vector3"/> result. Returns <see cref="Vector3.Zero"/> on an error.</returns>
    public Vector3 GetVector3(int columnIndex)
    {
      return NWScript.SqlGetVector(query, columnIndex);
    }

    /// <summary>
    /// Gets the serialized object result for the specified column, and spawns the object at the specified location and inventory.
    /// </summary>
    /// <param name="columnIndex">The 0-based index of the column to query.</param>
    /// <param name="spawnLocation">The location to spawn the object.</param>
    /// <param name="targetInventory">(Items only) The target inventory for the item.</param>
    /// <returns>The deserialized object. Returns null on an error.</returns>
    public T GetObject<T>(int columnIndex, Location spawnLocation, NwGameObject targetInventory = null) where T : NwObject
    {
      return NWScript.SqlGetObject(query, columnIndex, spawnLocation, targetInventory).ToNwObject<T>();
    }
  }
}
