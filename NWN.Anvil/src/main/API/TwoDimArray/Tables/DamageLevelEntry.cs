namespace Anvil.API
{
  public class DamageLevelEntry : ITwoDimArrayEntry
  {
    public string Label { get; private set; }

    public int RowIndex { get; init; }

    public StrRef? StrRef { get; private set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("LABEL");
      StrRef = entry.GetStrRef("STRING_REF");
    }
  }
}
