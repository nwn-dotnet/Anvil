namespace Anvil.API
{
  public sealed class ItemPropertyParamTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    /// <summary>
    /// Gets the name of the parameter.
    /// </summary>
    public StrRef? Name { get; private set; }

    /// <summary>
    /// Gets the label of the parameter.
    /// </summary>
    public string? Label { get; private set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Name = entry.GetStrRef("Name");
      Label = entry.GetString("Label") ?? entry.GetString("Lable");
    }
  }
}
