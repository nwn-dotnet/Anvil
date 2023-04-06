namespace Anvil.API
{
  /// <summary>
  /// An effect icon table entry (effecticons.2da)
  /// </summary>
  public sealed class EffectIconTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    /// <summary>
    /// Gets the developer label for this icon entry.
    /// </summary>
    public string? Label { get; private set; }

    /// <summary>
    /// Gets the localised string reference for this icon.
    /// </summary>
    public StrRef? StrRef { get; private set; }

    /// <summary>
    /// Gets the ResRef for this effect icon.
    /// </summary>
    public string? Icon { get; private set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("Label");
      Icon = entry.GetString("Icon");
      StrRef = entry.GetStrRef("StrRef");
    }
  }
}
