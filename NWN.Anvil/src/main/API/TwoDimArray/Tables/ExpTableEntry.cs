namespace Anvil.API
{
  public sealed class ExpTableEntry : ITwoDimArrayEntry
  {
    /// <summary>
    /// Gets the character level.
    /// </summary>
    public int? Level { get; set; }

    public int RowIndex { get; init; }

    /// <summary>
    /// Gets the XP required to gain <see cref="Level"/>.
    /// </summary>
    public uint? XP { get; set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Level = entry.GetInt("Level");
      XP = unchecked((uint?)entry.GetInt("XP"));
    }
  }
}
