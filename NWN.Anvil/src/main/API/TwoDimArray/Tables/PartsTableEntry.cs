namespace Anvil.API
{
  public sealed class PartsTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    public int? CostModifier { get; private set; }

    public float? ACBonus { get; private set; }

    public ArmorTableEntry? ArmorTableEntry { get; private set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      CostModifier = entry.GetInt("COSTMODIFIER");
      ACBonus = entry.GetFloat("ACBONUS");
      ArmorTableEntry = ACBonus.HasValue ? NwGameTables.ArmorTable[(int)ACBonus.Value] : null;
    }
  }
}
