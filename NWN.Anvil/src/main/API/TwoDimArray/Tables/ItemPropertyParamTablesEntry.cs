namespace Anvil.API
{
  public sealed class ItemPropertyParamTablesEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    /// <summary>
    /// Gets the name of the table.
    /// </summary>
    public StrRef? Name { get; private set; }

    /// <summary>
    /// Gets the label for the table.
    /// </summary>
    public string? Label { get; private set; }

    /// <summary>
    /// Gets the referenced table.
    /// </summary>
    public TwoDimArray<ItemPropertyParamTableEntry>? Table { get; private set; }

    /// <summary>
    /// Gets the 2da resref of the table.
    /// </summary>
    public string? TableResRef { get; private set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Name = entry.GetStrRef("Name");
      Label = entry.GetString("Lable"); // Intentional Typo
      Table = entry.GetTable<ItemPropertyParamTableEntry>("TableResRef");
      TableResRef = entry.GetString("TableResRef");
    }
  }
}
