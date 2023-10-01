using System.Linq;

namespace Anvil.API
{
  /// <summary>
  /// A creature polymorph table entry (polymorph.2da)
  /// </summary>
  public sealed class PolymorphTableEntry : ITwoDimArrayEntry
  {
    public int RowIndex { get; init; }

    public string? Name { get; private set; }

    public AppearanceTableEntry? AppearanceType { get; private set; }

    public NwRace? RacialType { get; private set; }

    public PortraitTableEntry? PortraitId { get; private set; }

    public string? PortraitCustom { get; private set; }

    public string? CreatureWeapon1 { get; private set; }

    public string? CreatureWeapon2 { get; private set; }

    public string? CreatureWeapon3 { get; private set; }

    public string? CreatureHideItem { get; private set; }

    public string? EquippedItem { get; private set; }

    public int? Strength { get; private set; }

    public int? Constitution { get; private set; }

    public int? Dexterity { get; private set; }

    public int? NaturalAcBonus { get; private set; }

    public int? HpBonus { get; private set; }

    public NwSpell? Spell1 { get; private set; }

    public NwSpell? Spell2 { get; private set; }

    public NwSpell? Spell3 { get; private set; }

    public bool? MergeW { get; private set; }

    public bool? MergeI { get; private set; }

    public bool? MergeA { get; private set; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Name = entry.GetString("Name");
      AppearanceType = entry.GetTableEntry("AppearanceType", NwGameTables.AppearanceTable);
      RacialType = NwRace.FromRaceId(entry.GetInt("RacialType"));
      PortraitId = entry.GetTableEntry("PortraitId", NwGameTables.PortraitTable);
      PortraitCustom = entry.GetString("Portrait");
      CreatureWeapon1 = entry.GetString("CreatureWeapon1");
      CreatureWeapon2 = entry.GetString("CreatureWeapon2");
      CreatureWeapon3 = entry.GetString("CreatureWeapon3");
      CreatureHideItem = entry.GetString("HideItem");
      EquippedItem = entry.GetString("EQUIPPED");
      Strength = entry.GetInt("STR");
      Constitution = entry.GetInt("CON");
      Dexterity = entry.GetInt("DEX");
      NaturalAcBonus = entry.GetInt("NATURALACBONUS");
      HpBonus = entry.GetInt("HPBONUS");
      Spell1 = NwSpell.FromSpellId(entry.GetInt("SPELL1"));
      Spell2 = NwSpell.FromSpellId(entry.GetInt("SPELL2"));
      Spell3 = NwSpell.FromSpellId(entry.GetInt("SPELL3"));
      MergeW = entry.GetBool("MergeW");
      MergeI = entry.GetBool("MergeI");
      MergeA = entry.GetBool("MergeA");
    }

    public static implicit operator PolymorphTableEntry?(AppearanceType appearanceType)
    {
      return NwGameTables.PolymorphTable.ElementAtOrDefault((int)appearanceType);
    }
  }
}
