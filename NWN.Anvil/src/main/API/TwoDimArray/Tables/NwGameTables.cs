namespace Anvil.API
{
  public static partial class NwGameTables
  {
    /// <summary>
    /// Gets the appearance table (appearance.2da)
    /// </summary>
    public static TwoDimArray<AppearanceTableEntry> AppearanceTable { get; private set; } = null!;

    /// <summary>
    /// Gets the armor table (armor.2da)
    /// </summary>
    public static TwoDimArray<ArmorTableEntry> ArmorTable { get; private set; } = null!;

    /// <summary>
    /// Gets the body bag table (bodybag.2da)
    /// </summary>
    public static TwoDimArray<BodyBagTableEntry> BodyBagTable { get; private set; } = null!;

    /// <summary>
    /// Gets the damage level table (damagelevels.2da)
    /// </summary>
    public static TwoDimArray<DamageLevelEntry> DamageLevelTable { get; private set; } = null!;

    /// <summary>
    /// Gets the effect icon table (effecticons.2da)
    /// </summary>
    public static TwoDimArray<EffectIconTableEntry> EffectIconTable { get; private set; } = null!;

    /// <summary>
    /// Gets the environment preset table (environment.2da)
    /// </summary>
    public static TwoDimArray<EnvironmentPreset> EnvironmentPresetTable { get; private set; } = null!;

    /// <summary>
    /// Gets the experience point/level progression table (exptable.2da)
    /// </summary>
    public static TwoDimArray<ExpTableEntry> ExpTable { get; private set; } = null!;

    /// <summary>
    /// Gets the light color table (lightcolor.2da)
    /// </summary>
    public static TwoDimArray<LightColorTableEntry> LightColorTable { get; private set; } = null!;

    /// <summary>
    /// Gets the loading screen table (loadscreens.2da)
    /// </summary>
    public static TwoDimArray<LoadScreenTableEntry> LoadScreenTable { get; private set; } = null!;

    /// <summary>
    /// Gets the belt parts table (parts_belt.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsBeltTable { get; private set; } = null!;

    /// <summary>
    /// Gets the bicep parts table (parts_bicep.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsBicepTable { get; private set; } = null!;

    /// <summary>
    /// Gets the chest parts table (parts_chest.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsChestTable { get; private set; } = null!;

    /// <summary>
    /// Gets the foot parts table (parts_foot.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsFootTable { get; private set; } = null!;

    /// <summary>
    /// Gets the forearm parts table (parts_forearm.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsForearmTable { get; private set; } = null!;

    /// <summary>
    /// Gets the hand parts table (parts_hand.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsHandTable { get; private set; } = null!;

    /// <summary>
    /// Gets the leg parts table (parts_leg.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsLegTable { get; private set; } = null!;

    /// <summary>
    /// Gets the neck parts table (parts_neck.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsNeckTable { get; private set; } = null!;

    /// <summary>
    /// Gets the pelvis parts table (parts_pelvis.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsPelvisTable { get; private set; } = null!;

    /// <summary>
    /// Gets the robe parts table (parts_robe.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsRobeTable { get; private set; } = null!;

    /// <summary>
    /// Gets the shin parts table (parts_shin.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsShinTable { get; private set; } = null!;

    /// <summary>
    /// Gets the shoulder parts table (parts_shoulder.2da)
    /// </summary>
    public static TwoDimArray<PartsTableEntry> PartsShoulderTable { get; private set; } = null!;

    /// <summary>
    /// Gets the placeable sound table (placeableobjsnds.2da)
    /// </summary>
    public static TwoDimArray<PlaceableSoundTableEntry> PlaceableSoundTable { get; private set; } = null!;

    /// <summary>
    /// Gets the placeable table (placeables.2da)
    /// </summary>
    public static TwoDimArray<PlaceableTableEntry> PlaceableTable { get; private set; } = null!;

    /// <summary>
    /// Gets the placeable type/category table (placeabletypes.2da)
    /// </summary>
    public static TwoDimArray<PlaceableTypeTableEntry> PlaceableTypeTable { get; private set; } = null!;

    /// <summary>
    /// Gets the polymorph table (polymorph.2da)
    /// </summary>
    public static TwoDimArray<PolymorphTableEntry> PolymorphTable { get; private set; } = null!;

    /// <summary>
    /// Gets the portraits table (portraits.2da)
    /// </summary>
    public static TwoDimArray<PortraitTableEntry> PortraitTable { get; private set; } = null!;

    /// <summary>
    /// Gets the programmed effect table (progfx.2da)
    /// </summary>
    public static TwoDimArray<ProgrammedEffectTableEntry> ProgrammedEffectTable { get; private set; } = null!;

    /// <summary>
    /// Gets the programmed effect table (vfx_persistent.2da)
    /// </summary>
    public static TwoDimArray<PersistentVfxTableEntry> PersistentEffectTable { get; private set; } = null!;

    /// <summary>
    /// Gets the item/skill cost table (skillvsitemcost.2da)
    /// </summary>
    public static TwoDimArray<SkillItemCostTableEntry> SkillItemCostTable { get; private set; } = null!;

    /// <summary>
    /// Gets surface material table (surfacemat.2da)
    /// </summary>
    public static TwoDimArray<SurfaceMaterialTableEntry> SurfaceMaterialTable { get; private set; } = null!;

    /// <summary>
    /// Gets the visual effect table (visualeffects.2da)
    /// </summary>
    public static TwoDimArray<VisualEffectTableEntry> VisualEffectTable { get; private set; } = null!;

    /// <summary>
    /// Gets the item property item mapping table (itemprops.2da)
    /// </summary>
    public static TwoDimArray<ItemPropertyItemMapTableEntry> ItemPropertyItemMapTable { get; private set; } = null!;

    /// <summary>
    /// Gets the item property table. (itempropdef.2da)
    /// </summary>
    public static TwoDimArray<ItemPropertyTableEntry> ItemPropertyTable { get; private set; } = null!;

    /// <summary>
    /// Gets the item property cost tables. (iprp_costtable.2da)
    /// </summary>
    public static TwoDimArray<ItemPropertyCostTablesEntry> ItemPropertyCostTables { get; private set; } = null!;

    /// <summary>
    /// Gets the item property param tables. (iprp_paramtable.2da)
    /// </summary>
    public static TwoDimArray<ItemPropertyParamTablesEntry> ItemPropertyParamTables { get; private set; } = null!;
  }
}
