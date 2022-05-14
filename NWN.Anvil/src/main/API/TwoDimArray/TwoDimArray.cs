using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using NWN.Native.API;

namespace Anvil.API
{
  //! ## Examples
  //! @include XPReportService.cs

  /// <summary>
  /// A two dimensional array data resource.
  /// </summary>
  public class TwoDimArray : IEquatable<TwoDimArray>
  {
    private readonly C2DA array;

    public TwoDimArray(string resRef)
    {
      resRef = resRef.Replace(".2da", string.Empty);
      array = NWNXLib.Rules().m_p2DArrays.GetCached2DA(resRef.ToExoString(), true.ToInt());
      if (array == null)
      {
        throw new ArgumentException("Invalid 2DA ResRef.", nameof(resRef));
      }

      Init();
    }

    internal TwoDimArray(C2DA array)
    {
      this.array = array;
      if (array == null)
      {
        throw new ArgumentNullException(nameof(array));
      }

      Init();
    }

    /// <summary>
    /// Gets the number of columns in this 2da.
    /// </summary>
    public int ColumnCount => array.m_nNumColumns;

    /// <summary>
    /// Gets the column labels/names for this 2da.
    /// </summary>
    public string[] Columns { get; private set; }

    /// <summary>
    /// Gets the number of rows in this 2da.
    /// </summary>
    public int RowCount => array.m_nNumRows;

    public static bool operator ==(TwoDimArray? left, TwoDimArray? right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(TwoDimArray? left, TwoDimArray? right)
    {
      return !Equals(left, right);
    }

    public bool Equals(TwoDimArray? other)
    {
      if (ReferenceEquals(null, other))
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return array.Equals(other.array);
    }

    public override bool Equals(object? obj)
    {
      return ReferenceEquals(this, obj) || obj is TwoDimArray other && Equals(other);
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
    public unsafe bool? GetBool(int rowIndex, int columnIndex)
    {
      int retVal;
      if (array.GetINTEntry(rowIndex, columnIndex, &retVal).ToBool())
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
    public unsafe T? GetEnum<T>(int rowIndex, int columnIndex) where T : struct, Enum
    {
      if (Unsafe.SizeOf<T>() != Unsafe.SizeOf<int>())
      {
        throw new ArgumentOutOfRangeException(nameof(T), "Specified enum must be backed by a signed int32 (int)");
      }

      int retVal;
      if (array.GetINTEntry(rowIndex, columnIndex, &retVal).ToBool())
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
    public unsafe float? GetFloat(int rowIndex, int columnIndex)
    {
      float retVal;
      if (array.GetFLOATEntry(rowIndex, columnIndex, &retVal).ToBool())
      {
        return retVal;
      }

      return null;
    }

    public override int GetHashCode()
    {
      return array.GetHashCode();
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
    public unsafe int? GetInt(int rowIndex, int columnIndex)
    {
      int retVal;
      if (array.GetINTEntry(rowIndex, columnIndex, &retVal).ToBool())
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
      using CExoString retVal = new CExoString();
      if (array.GetCExoStringEntry(rowIndex, columnIndex, retVal).ToBool())
      {
        return retVal.ToString();
      }

      return null;
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
    public unsafe StrRef? GetStrRef(int rowIndex, int columnIndex)
    {
      int retVal;
      if (array.GetINTEntry(rowIndex, columnIndex, &retVal).ToBool())
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
    public T? GetTableEntry<T>(int rowIndex, string columnName, TwoDimArray<T> table) where T : ITwoDimArrayEntry, new()
    {
      int columnIndex = GetColumnIndex(columnName);
      return GetTableEntry(rowIndex, columnIndex, table);
    }

    /// <summary>
    /// Interprets the specified value as a table index, and returns the associated table entry.
    /// </summary>
    /// <param name="rowIndex">The index of the row to query.</param>
    /// <param name="columnIndex">The index of the column to query.</param>
    /// <param name="table">The table that should be used to resolve the value.</param>
    /// <typeparam name="T">The type of table entry.</typeparam>
    /// <returns>The associated value, otherwise the default array entry value (typically null)</returns>
    public unsafe T? GetTableEntry<T>(int rowIndex, int columnIndex, TwoDimArray<T> table) where T : ITwoDimArrayEntry, new()
    {
      int index;
      if (array.GetINTEntry(rowIndex, columnIndex, &index).ToBool() && index < table.RowCount)
      {
        return table[index];
      }

      return default;
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
    public unsafe Vector3? GetVector3(int rowIndex, int columnIndexX, int columnIndexY, int columnIndexZ)
    {
      float xVal;
      float yVal;
      float zVal;

      if (array.GetFLOATEntry(rowIndex, columnIndexX, &xVal).ToBool() &&
        array.GetFLOATEntry(rowIndex, columnIndexY, &yVal).ToBool() &&
        array.GetFLOATEntry(rowIndex, columnIndexZ, &zVal).ToBool())
      {
        return new Vector3(xVal, yVal, zVal);
      }

      return null;
    }

    [MemberNotNull(nameof(Columns))]
    private void Init()
    {
      CExoStringArray columnArray = CExoStringArray.FromPointer(array.m_pColumnLabel);
      Columns = new string[array.m_nNumColumns];

      for (int i = 0; i < array.m_nNumColumns; i++)
      {
        string columnName = columnArray.GetItem(i).ToString();
        Columns[i] = columnName;
      }
    }
  }

  /// <summary>
  /// A two dimensional array resource, with a decoded row type.
  /// </summary>
  /// <typeparam name="T">The row/entry type to decode the array.</typeparam>
  public sealed class TwoDimArray<T> : TwoDimArray, IReadOnlyList<T> where T : ITwoDimArrayEntry, new()
  {
    public TwoDimArray(string resRef) : base(resRef) {}
    internal TwoDimArray(C2DA array) : base(array) {}

    /// <inheritdoc cref="TwoDimArray.RowCount"/>
    public int Count => RowCount;

    /// <summary>
    /// Gets a read-only list of all rows in this 2da.
    /// </summary>
    public IReadOnlyList<T> Rows
    {
      get
      {
        T[] retVal = new T[RowCount];
        for (int i = 0; i < RowCount; i++)
        {
          retVal[i] = GetRow(i);
        }

        return retVal;
      }
    }

    /// <inheritdoc cref="GetRow"/>
    public T this[int rowIndex] => GetRow(rowIndex);

    public IEnumerator<T> GetEnumerator()
    {
      return Rows.GetEnumerator();
    }

    /// <summary>
    /// Gets the row at the specified index.
    /// </summary>
    /// <param name="rowIndex">The row index.</param>
    public T GetRow(int rowIndex)
    {
      if (rowIndex < 0 || rowIndex >= RowCount)
      {
        throw new ArgumentOutOfRangeException(nameof(rowIndex), "Row index was out of range. Must be non-negative and less than the size of the array.");
      }

      TwoDimArrayEntry entry = new TwoDimArrayEntry(this, rowIndex);
      T retVal = new T
      {
        RowIndex = rowIndex,
      };
      retVal.InterpretEntry(entry);

      return retVal;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
