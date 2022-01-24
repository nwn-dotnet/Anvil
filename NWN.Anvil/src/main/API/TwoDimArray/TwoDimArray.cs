using System;
using System.Collections;
using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
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

    public int ColumnCount => array.m_nNumColumns;

    public string[] Columns { get; private set; }

    public int RowCount => array.m_nNumRows;

    public static bool operator ==(TwoDimArray left, TwoDimArray right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(TwoDimArray left, TwoDimArray right)
    {
      return !Equals(left, right);
    }

    public bool Equals(TwoDimArray other)
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

    public override bool Equals(object obj)
    {
      return ReferenceEquals(this, obj) || obj is TwoDimArray other && Equals(other);
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

    public int GetColumnIndex(string columnName)
    {
      return Array.FindIndex(Columns, column => columnName.Equals(column, StringComparison.OrdinalIgnoreCase));
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

    public override int GetHashCode()
    {
      return array.GetHashCode();
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

    public StrRef? GetStrRef(int rowIndex, string columnName)
    {
      int columnIndex = GetColumnIndex(columnName);
      return columnIndex >= 0 ? GetStrRef(rowIndex, columnIndex) : null;
    }

    public unsafe StrRef? GetStrRef(int rowIndex, int columnIndex)
    {
      int retVal;
      if (array.GetINTEntry(rowIndex, columnIndex, &retVal).ToBool())
      {
        return new StrRef(retVal);
      }

      return null;
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
  }

  public sealed class TwoDimArray<T> : TwoDimArray, IReadOnlyList<T> where T : ITwoDimArrayEntry, new()
  {
    public TwoDimArray(string resRef) : base(resRef) {}
    internal TwoDimArray(C2DA array) : base(array) {}

    public int Count => RowCount;

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

    public T this[int index] => GetRow(index);

    public IEnumerator<T> GetEnumerator()
    {
      return Rows.GetEnumerator();
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

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
