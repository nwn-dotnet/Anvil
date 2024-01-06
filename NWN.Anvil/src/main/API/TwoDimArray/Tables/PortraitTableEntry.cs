namespace Anvil.API
{
  /// <summary>
  /// A portrait table entry (portraits.2da)
  /// </summary>
  public sealed class PortraitTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    public string? BaseResRef { get; private set; }

    public Gender? Gender { get; private set; }

    public NwRace? Race { get; private set; }

    public PlaceableTypeTableEntry? InanimateType { get; private set; }

    public bool? Plot { get; private set; }

    public string? BaseResRefLowGore { get; private set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      BaseResRef = entry.GetString("BaseResRef");
      Gender = entry.GetEnum<Gender>("Sex");
      Race = NwRace.FromRaceId(entry.GetInt("RacialType"));
      InanimateType = entry.GetTableEntry("InanimateType", NwGameTables.PlaceableTypeTable);
      Plot = entry.GetBool("Plot");
      BaseResRefLowGore = entry.GetString("LowGore");
    }
  }
}
