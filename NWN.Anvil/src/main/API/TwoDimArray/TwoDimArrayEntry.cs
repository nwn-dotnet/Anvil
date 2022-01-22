using System;

namespace Anvil.API
{
  public sealed class TwoDimArrayEntry
  {
    private readonly string[] columns;
    private readonly string[] values;

    internal TwoDimArrayEntry(string[] columns, string[] values)
    {
      this.columns = columns;
      this.values = values;
    }

    public string this[int columnIndex] => values[columnIndex];

    public string this[string columnName]
    {
      get
      {
        int index = Array.IndexOf(columns, columnName);
        if (index == -1)
        {
          throw new ArgumentException($"Unknown column name {columnName}", columnName);
        }

        return values[index];
      }
    }
  }
}
