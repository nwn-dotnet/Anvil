namespace Anvil.API
{
  public sealed class PlaceableSoundTableEntry : ITwoDimArrayEntry
  {
    public string ArmorType { get; private set; }

    public string Closed { get; private set; }

    public string Destroyed { get; private set; }

    public string Label { get; private set; }

    public string Locked { get; private set; }

    public string Opened { get; private set; }

    public int RowIndex { get; init; }

    public string Used { get; private set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("Label");
      ArmorType = entry.GetString("ArmorType");
      Opened = entry.GetString("Opened");
      Closed = entry.GetString("Closed");
      Destroyed = entry.GetString("Destroyed");
      Used = entry.GetString("Used");
      Locked = entry.GetString("Locked");
    }
  }
}
