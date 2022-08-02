using System;
using System.Collections;
using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A two dimensional array resource, with a decoded row type.
  /// </summary>
  /// <typeparam name="T">The row/entry type to decode the array.</typeparam>
  public sealed class TwoDimArray<T> : TwoDimArray, IReadOnlyList<T> where T : class, ITwoDimArrayEntry, new()
  {
    private readonly T?[] cachedRows;

    internal TwoDimArray(C2DA array) : base(array)
    {
      cachedRows = new T[RowCount];
    }

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

      T? retVal = cachedRows[rowIndex];
      if (retVal != null)
      {
        return retVal;
      }

      TwoDimArrayEntry entry = new TwoDimArrayEntry(this, rowIndex);
      retVal = new T
      {
        RowIndex = rowIndex,
      };
      retVal.InterpretEntry(entry);
      cachedRows[rowIndex] = retVal;

      return retVal;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
