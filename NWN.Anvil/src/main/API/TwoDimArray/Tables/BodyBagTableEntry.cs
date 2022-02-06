namespace Anvil.API
{
  public sealed class BodyBagTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    public string Label { get; private set; }

    public StrRef? Name { get; private set; }

    public PlaceableTableEntry Appearance { get; private set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("LABEL");
      Name = entry.GetStrRef("Name");
      Appearance = entry.GetTableEntry("Appearance", NwGameTables.PlaceableTable);
    }
  }
}
