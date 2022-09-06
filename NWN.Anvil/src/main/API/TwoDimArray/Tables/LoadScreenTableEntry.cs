namespace Anvil.API
{
  public sealed class LoadScreenTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    public string? Label { get; private set; }

    public string? ScriptingName { get; private set; }

    public string? BMPResRef { get; private set; }

    public string? TileSet { get; private set; }

    public StrRef? StrRef { get; private set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("Label");
      ScriptingName = entry.GetString("ScriptingName");
      BMPResRef = entry.GetString("BMPResRef");
      TileSet = entry.GetString("TileSet");
      StrRef = entry.GetStrRef("StrRef");
    }
  }
}
