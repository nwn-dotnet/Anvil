using System;
using System.Numerics;

namespace Anvil.API
{
  //! ## Examples
  //! @include XPReportService.cs

  /// <summary>
  /// 2da row data.
  /// </summary>
  public sealed class TwoDimArrayEntry
  {
    private readonly TwoDimArray array;
    private readonly int rowIndex;

    internal TwoDimArrayEntry(TwoDimArray array, int rowIndex)
    {
      this.array = array;
      this.rowIndex = rowIndex;
    }

    /// <summary>
    /// Gets the specified boolean value.
    /// </summary>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public bool? GetBool(string columnName)
    {
      return array.GetBool(rowIndex, columnName);
    }

    /// <summary>
    /// Gets the specified boolean value.
    /// </summary>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public bool? GetBool(int columnIndex)
    {
      return array.GetBool(rowIndex, columnIndex);
    }

    /// <summary>
    /// Gets the specified float value.
    /// </summary>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public float? GetFloat(string columnName)
    {
      return array.GetFloat(rowIndex, columnName);
    }

    /// <summary>
    /// Gets the specified float value.
    /// </summary>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public float? GetFloat(int columnIndex)
    {
      return array.GetFloat(rowIndex, columnIndex);
    }

    /// <summary>
    /// Gets the specified int value.
    /// </summary>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public int? GetInt(string columnName)
    {
      return array.GetInt(rowIndex, columnName);
    }

    /// <summary>
    /// Gets the specified int value.
    /// </summary>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public int? GetInt(int columnIndex)
    {
      return array.GetInt(rowIndex, columnIndex);
    }

    /// <summary>
    /// Gets the specified string value.
    /// </summary>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public string GetString(string columnName)
    {
      return array.GetString(rowIndex, columnName);
    }

    /// <summary>
    /// Gets the specified string value.
    /// </summary>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public string GetString(int columnIndex)
    {
      return array.GetString(rowIndex, columnIndex);
    }

    /// <summary>
    /// Gets the specified <see cref="StrRef"/> value.
    /// </summary>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public StrRef? GetStrRef(string columnName)
    {
      return array.GetStrRef(rowIndex, columnName);
    }

    /// <summary>
    /// Gets the specified <see cref="StrRef"/> value.
    /// </summary>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public StrRef? GetStrRef(int columnIndex)
    {
      return array.GetStrRef(rowIndex, columnIndex);
    }

    /// <summary>
    /// Interprets the specified value as a table index, and returns the associated table entry.
    /// </summary>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <param name="table">The table that should be used to resolve the value.</param>
    /// <typeparam name="T">The type of table entry.</typeparam>
    /// <returns>The associated value, otherwise the default array entry value (typically null)</returns>
    public T GetTableEntry<T>(string columnName, TwoDimArray<T> table) where T : ITwoDimArrayEntry, new()
    {
      return array.GetTableEntry(rowIndex, columnName, table);
    }

    /// <summary>
    /// Interprets the specified value as a table index, and returns the associated table entry.
    /// </summary>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <param name="table">The table that should be used to resolve the value.</param>
    /// <typeparam name="T">The type of table entry.</typeparam>
    /// <returns>The associated value, otherwise the default array entry value (typically null)</returns>
    public T GetTableEntry<T>(int columnIndex, TwoDimArray<T> table) where T : ITwoDimArrayEntry, new()
    {
      return array.GetTableEntry(rowIndex, columnIndex, table);
    }

    /// <summary>
    /// Gets the specified Vector3 value.
    /// </summary>
    /// <param name="columnNameX">The name/label of the column containing the x component of the vector.</param>
    /// <param name="columnNameY">The name/label of the column containing the y component of the vector.</param>
    /// <param name="columnNameZ">The name/label of the column containing the z component of the vector.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public Vector3? GetVector3(string columnNameX, string columnNameY, string columnNameZ)
    {
      return array.GetVector3(rowIndex, columnNameX, columnNameY, columnNameZ);
    }

    /// <summary>
    /// Gets the specified Vector3 value.
    /// </summary>
    /// <param name="columnIndexX">The index of the column containing the x component of the vector.</param>
    /// <param name="columnIndexY">The index of the column containing the y component of the vector.</param>
    /// <param name="columnIndexZ">The index of the column containing the z component of the vector.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public Vector3? GetVector3(int columnIndexX, int columnIndexY, int columnIndexZ)
    {
      return array.GetVector3(rowIndex, columnIndexX, columnIndexY, columnIndexZ);
    }

    /// <summary>
    /// Gets the specified enum value.
    /// </summary>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public T? GetEnum<T>(string columnName) where T : struct, Enum
    {
      return array.GetEnum<T>(rowIndex, columnName);
    }

    /// <summary>
    /// Gets the specified enum value.
    /// </summary>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public T? GetEnum<T>(int columnIndex) where T : struct, Enum
    {
      return array.GetEnum<T>(rowIndex, columnIndex);
    }
  }
}
