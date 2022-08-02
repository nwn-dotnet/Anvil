namespace Anvil.API
{
  public sealed class ItemPropertyTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    public ItemPropertyItemMapTableEntry? ItemMap { get; private set; }

    /// <summary>
    /// Gets the name of this item property.
    /// </summary>
    public StrRef? Name { get; private set; }

    /// <summary>
    /// Gets the label of this item property.
    /// </summary>
    public string? Label { get; private set; }

    /// <summary>
    /// If defined, the sub-type table associated with this item property.
    /// </summary>
    public TwoDimArray<ItemPropertySubTypeTableEntry>? SubTypeTable { get; private set; }

    /// <summary>
    /// Gets the base cost of this item property.
    /// </summary>
    public float? Cost { get; private set; }

    /// <summary>
    /// If defined, the cost table associated with this item property.
    /// </summary>
    public TwoDimArray<ItemPropertyCostTableEntry>? CostTable { get; private set; }

    /// <summary>
    /// If defined, the parameter table associated with the 1st parameter of this item property.
    /// </summary>
    public TwoDimArray<ItemPropertyParamTableEntry>? Param1Table { get; private set; }

    /// <summary>
    /// Gets the name/string shown in-game e.g. for item examine.
    /// </summary>
    public StrRef? GameStrRef { get; private set; }

    /// <summary>
    /// Gets the description of the item property.
    /// </summary>
    public StrRef? Description { get; private set; }

    /// <summary>
    /// Gets the associated enum constant for this item property.
    /// </summary>
    public ItemPropertyType PropertyType => (ItemPropertyType)RowIndex;

    void ITwoDimArrayEntry.InterpretEntry(TwoDimArrayEntry entry)
    {
      ItemMap = NwGameTables.ItemPropertyItemMapTable.GetRow(RowIndex);
      Name = entry.GetStrRef("Name");
      Label = entry.GetString("Label");
      SubTypeTable = entry.GetTable<ItemPropertySubTypeTableEntry>("SubTypeResRef");
      Cost = entry.GetFloat("Cost");
      CostTable = entry.GetTableEntry("CostTableResRef", NwGameTables.ItemPropertyCostTables)?.Table;
      Param1Table = entry.GetTableEntry("Param1ResRef", NwGameTables.ItemPropertyParamTables)?.Table;
      GameStrRef = entry.GetStrRef("GameStrRef");
      Description = entry.GetStrRef("Description");
    }
  }
}
