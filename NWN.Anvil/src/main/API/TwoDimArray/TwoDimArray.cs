using System;
using System.Collections;
using System.Collections.Generic;
using NWN.Native.API;

namespace Anvil.API
{
  public class TwoDimArray : IReadOnlyList<string[]>
  {
    protected readonly C2DA Array;

    public string[] Columns { get; private set; }

    private readonly CExoString tmp = new CExoString();

    public TwoDimArray(string resRef)
    {
      resRef = resRef.Replace(".2da", string.Empty);
      Array = NWNXLib.Rules().m_p2DArrays.GetCached2DA(resRef.ToExoString(), true.ToInt());
      if (Array == null)
      {
        throw new ArgumentException("Invalid 2DA resref.", nameof(resRef));
      }

      Init();
    }

    internal TwoDimArray(C2DA array)
    {
      Array = array;
      if (array == null)
      {
        throw new ArgumentNullException(nameof(array));
      }

      Init();
    }

    private void Init()
    {
      CExoStringArray columnArray = CExoStringArray.FromPointer(Array.m_pColumnLabel);
      Columns = new string[Array.m_nNumColumns];

      for (int i = 0; i < Array.m_nNumColumns; i++)
      {
        string columnName = columnArray.GetItem(i).ToString();
        Columns[i] = columnName;
      }
    }

    public string this[int rowIndex, int columnIndex]
    {
      get
      {
        Array.GetCExoStringEntry(rowIndex, columnIndex, tmp);
        return tmp.ToString();
      }
    }

    public string this[int rowIndex, string columnName]
    {
      get
      {
        int columnIndex = GetColumnIndex(columnName);
        return columnIndex < 0 ? null : this[rowIndex, columnIndex];
      }
    }

    public string[] this[int rowIndex]
    {
      get
      {
        string[] retVal = new string[Array.m_nNumColumns];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = this[rowIndex, i];
        }

        return retVal;
      }
    }

    public int GetColumnIndex(string columnName)
    {
      return System.Array.IndexOf(Columns, columnName);
    }

    public int RowCount => Array.m_nNumRows;

    public int ColumnCount => Array.m_nNumColumns;

    public IEnumerator<string[]> GetEnumerator()
    {
      for (int i = 0; i < RowCount; i++)
      {
        yield return this[i];
      }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public int Count => RowCount;
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
      string[] rowValues = base[rowIndex];
      if (rowValues == null)
      {
        return default;
      }

      TwoDimArrayEntry entry = new TwoDimArrayEntry(Columns, rowValues);
      T retVal = new T();
      retVal.InterpretEntry(entry);
      return retVal;
    }

    public TwoDimArray(string resRef) : base(resRef) {}
    internal TwoDimArray(C2DA array) : base(array) {}

    public new IEnumerator<T> GetEnumerator()
    {
      return Rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public new T this[int index] => throw new NotImplementedException();
  }
}
