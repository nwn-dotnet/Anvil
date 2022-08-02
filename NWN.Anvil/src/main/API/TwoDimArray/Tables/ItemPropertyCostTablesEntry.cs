namespace Anvil.API
{
  public sealed class ItemPropertyCostTablesEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    /// <summary>
    /// Gets the referenced table.
    /// </summary>
    public TwoDimArray<ItemPropertyCostTableEntry>? Table { get; private set; }

    /// <summary>
    /// Gets the 2da resref of the cost table.
    /// </summary>
    public string? TableResRef { get; private set; }

    /// <summary>
    /// Gets the label of this cost table.
    /// </summary>
    public string? Label { get; private set; }

    /// <summary>
    /// Gets if this row's cost table is one a client can safely load, rather than relying on the server to provide.
    /// </summary>
    public bool? ClientLoad { get; private set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Table = entry.GetTable<ItemPropertyCostTableEntry>("Name");
      TableResRef = entry.GetString("Name");
      Label = entry.GetString("Label");
      ClientLoad = entry.GetBool("ClientLoad");
    }
  }
}
