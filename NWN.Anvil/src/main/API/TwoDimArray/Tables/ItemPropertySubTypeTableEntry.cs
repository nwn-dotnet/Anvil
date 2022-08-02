namespace Anvil.API
{
  public sealed class ItemPropertySubTypeTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    /// <summary>
    /// Gets the name of this item property sub type.
    /// </summary>
    public StrRef? Name { get; private set; }

    /// <summary>
    /// Gets the label of this item property sub type.
    /// </summary>
    public string? Label { get; private set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Name = entry.GetStrRef("Name");
      Label = entry.GetString("Label") ?? entry.GetString("Lable");
    }
  }
}
