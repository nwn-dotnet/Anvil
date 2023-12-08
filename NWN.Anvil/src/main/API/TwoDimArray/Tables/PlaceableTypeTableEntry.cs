namespace Anvil.API
{
  /// <summary>
  /// A placeable type table entry (placeabletypes.2da)
  /// </summary>
  public sealed class PlaceableTypeTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    public string? Label { get; private set; }

    public StrRef? StrRef { get; private set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("Label");
      StrRef = entry.GetStrRef("StrRef");
    }
  }
}
