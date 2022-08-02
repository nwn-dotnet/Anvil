using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Anvil.Native;
using NWN.Native.API;

namespace Anvil.API
{
  //! ## Examples
  //! @include XPReportService.cs

  /// <summary>
  /// A two dimensional array data resource.
  /// </summary>
  public class TwoDimArray
  {
    private readonly string?[,] arrayData;

    /// <summary>
    /// Gets the number of columns in this 2da.
    /// </summary>
    public int ColumnCount { get; }

    /// <summary>
    /// Gets the column labels/names for this 2da.
    /// </summary>
    public string[] Columns { get; }

    /// <summary>
    /// Gets the number of rows in this 2da.
    /// </summary>
    public int RowCount { get; }

    internal unsafe TwoDimArray(C2DA array)
    {
      if (array == null)
      {
        throw new ArgumentNullException(nameof(array));
      }

      RowCount = array.m_nNumRows;
      ColumnCount = array.m_nNumColumns;

      CExoStringArray columnArray = CExoStringArray.FromPointer(array.m_pColumnLabel);
      Columns = new string[array.m_nNumColumns];

      for (int i = 0; i < array.m_nNumColumns; i++)
      {
        string columnName = columnArray.GetItem(i).ToString();
        Columns[i] = columnName;
      }

      arrayData = new string[RowCount, ColumnCount];
      for (int i = 0; i < RowCount; i++)
      {
        for (int j = 0; j < ColumnCount; j++)
        {
          CExoStringData data = ((CExoStringData**)array.m_pArrayData)[i][j];
          arrayData[i, j] = data.ToString();
        }
      }
    }

    /// <summary>
    /// Gets the specified boolean value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public bool? GetBool(int rowIndex, string columnName)
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetBool(rowIndex, columnIndex) : null;
    }

    /// <summary>
    /// Gets the specified boolean value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public bool? GetBool(int rowIndex, int columnIndex)
    {
      string? data = arrayData[rowIndex, columnIndex];
      if (int.TryParse(data, out int retVal))
      {
        return retVal.ToBool();
      }

      return null;
    }

    /// <summary>
    /// Gets the index of the column with the specified name/label.
    /// </summary>
    /// <param name="columnName">The column to lookup.</param>
    /// <returns>The zero-based index of the column if found, otherwise -1.</returns>
    public int GetColumnIndex(string columnName)
    {
      return Array.FindIndex(Columns, column => columnName.Equals(column, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets the specified <see cref="StrRef"/> value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public T? GetEnum<T>(int rowIndex, string columnName) where T : struct, Enum
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetEnum<T>(rowIndex, columnIndex) : null;
    }

    /// <summary>
    /// Gets the specified enum value value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public T? GetEnum<T>(int rowIndex, int columnIndex) where T : struct, Enum
    {
      if (Unsafe.SizeOf<T>() != Unsafe.SizeOf<int>())
      {
        throw new ArgumentOutOfRangeException(nameof(T), "Specified enum must be backed by a signed int32 (int)");
      }

      string? data = arrayData[rowIndex, columnIndex];
      if (int.TryParse(data, out int retVal))
      {
        return Unsafe.As<int, T>(ref retVal);
      }

      return null;
    }

    /// <summary>
    /// Gets the specified float value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public float? GetFloat(int rowIndex, string columnName)
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetFloat(rowIndex, columnIndex) : null;
    }

    /// <summary>
    /// Gets the specified float value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public float? GetFloat(int rowIndex, int columnIndex)
    {
      string? data = arrayData[rowIndex, columnIndex];
      if (float.TryParse(data, out float retVal))
      {
        return retVal;
      }

      return null;
    }

    /// <summary>
    /// Gets the specified int value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public int? GetInt(int rowIndex, string columnName)
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetInt(rowIndex, columnIndex) : null;
    }

    /// <summary>
    /// Gets the specified int value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public int? GetInt(int rowIndex, int columnIndex)
    {
      string? data = arrayData[rowIndex, columnIndex];
      if (int.TryParse(data, out int retVal))
      {
        return retVal;
      }

      return null;
    }

    /// <summary>
    /// Gets the specified string value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public string? GetString(int rowIndex, string columnName)
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetString(rowIndex, columnIndex) : null;
    }

    /// <summary>
    /// Gets the specified string value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public string? GetString(int rowIndex, int columnIndex)
    {
      return arrayData[rowIndex, columnIndex];
    }

    /// <summary>
    /// Gets the specified <see cref="StrRef"/> value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public StrRef? GetStrRef(int rowIndex, string columnName)
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetStrRef(rowIndex, columnIndex) : null;
    }

    /// <summary>
    /// Gets the specified <see cref="StrRef"/> value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public StrRef? GetStrRef(int rowIndex, int columnIndex)
    {
      string? data = arrayData[rowIndex, columnIndex];
      if (int.TryParse(data, out int retVal))
      {
        return new StrRef(retVal);
      }

      return null;
    }

    /// <summary>
    /// Interprets the specified value as a table index, and returns the associated table entry.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <param name="table">The table that should be used to resolve the value.</param>
    /// <typeparam name="T">The type of table entry.</typeparam>
    /// <returns>The associated value, otherwise the default array entry value (typically null)</returns>
    public T? GetTableEntry<T>(int rowIndex, string columnName, TwoDimArray<T> table) where T : class, ITwoDimArrayEntry, new()
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetTableEntry(rowIndex, columnIndex, table) : null;
    }

    /// <summary>
    /// Interprets the specified value as a table index, and returns the associated table entry.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <param name="table">The table that should be used to resolve the value.</param>
    /// <typeparam name="T">The type of table entry.</typeparam>
    /// <returns>The associated value, otherwise the default array entry value (typically null)</returns>
    public T? GetTableEntry<T>(int rowIndex, int columnIndex, TwoDimArray<T> table) where T : class, ITwoDimArrayEntry, new()
    {
      string? data = arrayData[rowIndex, columnIndex];
      if (int.TryParse(data, out int index) && index < table.RowCount)
      {
        return table[index];
      }

      return default;
    }

    /// <summary>
    /// Interprets the specified value as a table name, and returns the associated table.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnName">The name/label of the column to query.</param>
    /// <typeparam name="T">The type of table entry.</typeparam>
    /// <returns>The associated value, otherwise null.</returns>
    public TwoDimArray<T>? GetTable<T>(int rowIndex, string columnName) where T : class, ITwoDimArrayEntry, new()
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetTable<T>(rowIndex, columnIndex) : null;
    }

    /// <summary>
    /// Interprets the specified value as a table name, and returns the associated table.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <typeparam name="T">The type of table entry.</typeparam>
    /// <returns>The associated value, otherwise null.</returns>
    public TwoDimArray<T>? GetTable<T>(int rowIndex, int columnIndex) where T : class, ITwoDimArrayEntry, new()
    {
      string? tableName = GetString(rowIndex, columnIndex);
      return string.IsNullOrEmpty(tableName) ? null : NwGameTables.GetTable<T>(tableName, true, false);
    }

    /// <summary>
    /// Gets the specified Vector3 value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnNameX">The name/label of the column containing the x component of the vector.</param>
    /// <param name="columnNameY">The name/label of the column containing the y component of the vector.</param>
    /// <param name="columnNameZ">The name/label of the column containing the z component of the vector.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public Vector3? GetVector3(int rowIndex, string columnNameX, string columnNameY, string columnNameZ)
    {
      int columnIndexX = GetColumnIndex(columnNameX);
      int columnIndexY = GetColumnIndex(columnNameY);
      int columnIndexZ = GetColumnIndex(columnNameZ);

      return columnIndexX >= 0 && columnIndexY >= 0 && columnIndexZ >= 0 ? GetVector3(rowIndex, columnIndexX, columnIndexY, columnIndexZ) : null;
    }

    /// <summary>
    /// Gets the specified Vector3 value.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnIndexX">The index of the column containing the x component of the vector.</param>
    /// <param name="columnIndexY">The index of the column containing the y component of the vector.</param>
    /// <param name="columnIndexZ">The index of the column containing the z component of the vector.</param>
    /// <returns>The associated value. null if no value is set.</returns>
    public Vector3? GetVector3(int rowIndex, int columnIndexX, int columnIndexY, int columnIndexZ)
    {
      string? xData = arrayData[rowIndex, columnIndexX];
      string? yData = arrayData[rowIndex, columnIndexY];
      string? zData = arrayData[rowIndex, columnIndexZ];

      if (float.TryParse(xData, out float xVal) &&
        float.TryParse(yData, out float yVal) &&
        float.TryParse(zData, out float zVal))
      {
        return new Vector3(xVal, yVal, zVal);
      }

      return null;
    }
  }
}
