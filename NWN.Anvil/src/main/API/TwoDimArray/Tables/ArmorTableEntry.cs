namespace Anvil.API
{
  public sealed class ArmorTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    public int? ACBonus { get; private set; }

    public int? DexBonus { get; private set; }

    public int? ACCheck { get; private set; }

    public int? ArcaneFailurePct { get; private set; }

    public int? Weight { get; private set; }

    public int? Cost { get; private set; }

    public StrRef? Description { get; private set; }

    public StrRef? BaseItemStats { get; private set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      ACBonus = entry.GetInt("ACBONUS");
      DexBonus = entry.GetInt("DEXBONUS");
      ACCheck = entry.GetInt("ACCHECK");
      ArcaneFailurePct = entry.GetInt("ARCANEFAILURE%");
      Weight = entry.GetInt("WEIGHT");
      Cost = entry.GetInt("COST");
      Description = entry.GetStrRef("DESCRIPTIONS");
      BaseItemStats = entry.GetStrRef("BASEITEMSTATREF");
    }
  }
}
