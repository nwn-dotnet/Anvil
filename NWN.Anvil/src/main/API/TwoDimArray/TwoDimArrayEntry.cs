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
  }
}
