using System;
using System.Collections;
using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
  public class TwoDimArray
  {
    private readonly C2DA array;

    public string[] Columns { get; private set; }

    public TwoDimArray(string resRef)
    {
      resRef = resRef.Replace(".2da", string.Empty);
      array = NWNXLib.Rules().m_p2DArrays.GetCached2DA(resRef.ToExoString(), true.ToInt());
      if (array == null)
      {
        throw new ArgumentException("Invalid 2DA resref.", nameof(resRef));
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

    public string GetString(int rowIndex, string columnName)
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetString(rowIndex, columnIndex) : null;
    }

    public string GetString(int rowIndex, int columnIndex)
    {
      using CExoString retVal = new CExoString();
      if (array.GetCExoStringEntry(rowIndex, columnIndex, retVal).ToBool())
      {
        return retVal.ToString();
      }

      return null;
    }

    public int? GetInt(int rowIndex, string columnName)
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetInt(rowIndex, columnIndex) : null;
    }

    public unsafe int? GetInt(int rowIndex, int columnIndex)
    {
      int retVal;
      if (array.GetINTEntry(rowIndex, columnIndex, &retVal).ToBool())
      {
        return retVal;
      }

      return null;
    }

    public bool? GetBool(int rowIndex, string columnName)
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetBool(rowIndex, columnIndex) : null;
    }

    public unsafe bool? GetBool(int rowIndex, int columnIndex)
    {
      int retVal;
      if (array.GetINTEntry(rowIndex, columnIndex, &retVal).ToBool())
      {
        return retVal.ToBool();
      }

      return null;
    }

    public float? GetFloat(int rowIndex, string columnName)
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetFloat(rowIndex, columnIndex) : null;
    }

    public unsafe float? GetFloat(int rowIndex, int columnIndex)
    {
      float retVal;
      if (array.GetFLOATEntry(rowIndex, columnIndex, &retVal).ToBool())
      {
        return retVal;
      }

      return null;
    }

    public int GetColumnIndex(string columnName)
    {
      return Array.FindIndex(Columns, column => columnName.Equals(column, StringComparison.OrdinalIgnoreCase));
    }

    public int RowCount => array.m_nNumRows;

    public int ColumnCount => array.m_nNumColumns;
  }

  public sealed class TwoDimArray<T> : TwoDimArray, IReadOnlyList<T> where T : ITwoDimArrayEntry, new()
  {
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

    public TwoDimArray(string resRef) : base(resRef) {}
    internal TwoDimArray(C2DA array) : base(array) {}

    public IEnumerator<T> GetEnumerator()
    {
      return Rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public T this[int index] => GetRow(index);

    public int Count => RowCount;
  }
}
