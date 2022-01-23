namespace Anvil.API
{
  public sealed class TwoDimArrayEntry
  {
    private readonly TwoDimArray array;
    private readonly int rowIndex;

    internal TwoDimArrayEntry(TwoDimArray array, int rowIndex)
    {
      this.array = array;
      this.rowIndex = rowIndex;
    }

    public string GetString(string columnName)
    {
      return array.GetString(rowIndex, columnName);
    }

    public string GetString(int columnIndex)
    {
      return array.GetString(rowIndex, columnIndex);
    }

    public int? GetInt(string columnName)
    {
      return array.GetInt(rowIndex, columnName);
    }

    public int? GetInt(int columnIndex)
    {
      return array.GetInt(rowIndex, columnIndex);
    }

    public bool? GetBool(string columnName)
    {
      return array.GetBool(rowIndex, columnName);
    }

    public bool? GetBool(int columnIndex)
    {
      return array.GetBool(rowIndex, columnIndex);
    }

    public float? GetFloat(string columnName)
    {
      return array.GetFloat(rowIndex, columnName);
    }

    public float? GetFloat(int columnIndex)
    {
      return array.GetFloat(rowIndex, columnIndex);
    }

    public StrRef? GetStrRef(string columnName)
    {
      return array.GetStrRef(rowIndex, columnName);
    }

    public StrRef? GetStrRef(int columnIndex)
    {
      return array.GetStrRef(rowIndex, columnIndex);
    }
  }
}
