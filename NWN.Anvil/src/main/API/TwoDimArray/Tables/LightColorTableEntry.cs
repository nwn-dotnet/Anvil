namespace Anvil.API
{
  public sealed class LightColorTableEntry : ITwoDimArrayEntry
  {
    public float? Blue { get; private set; }

    public float? Green { get; private set; }

    public string Label { get; private set; }

    public float? Red { get; private set; }
    public int RowIndex { get; init; }

    public float? ToolsetBlue { get; private set; }

    public float? ToolsetGreen { get; private set; }

    public float? ToolsetRed { get; private set; }

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("LABEL");
      Red = entry.GetFloat("RED");
      Green = entry.GetFloat("GREEN");
      Blue = entry.GetFloat("BLUE");
      ToolsetRed = entry.GetFloat("TOOLSETRED");
      ToolsetGreen = entry.GetFloat("TOOLSETGREEN");
      ToolsetBlue = entry.GetFloat("TOOLSETBLUE");
    }
  }
}
