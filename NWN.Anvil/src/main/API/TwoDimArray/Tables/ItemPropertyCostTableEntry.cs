namespace Anvil.API
{
  public sealed class ItemPropertyCostTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    /// <summary>
    /// Gets the name of this cost table entry.
    /// </summary>
    public StrRef? Name { get; private set; }

    /// <summary>
    /// Gets the label of this cost table entry.
    /// </summary>
    public string? Label { get; private set; }

    /// <summary>
    /// Gets the additional cost defined by this cost table entry.
    /// </summary>
    public float? Cost { get; private set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Name = entry.GetStrRef("Name");
      Label = entry.GetString("Label") ?? entry.GetString("Lable");
      Cost = entry.GetFloat("Cost");
    }
  }
}
