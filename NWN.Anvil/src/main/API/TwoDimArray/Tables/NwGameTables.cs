using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  public static class NwGameTables
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
    /// Gets the damage level table (damagelevels.2da
    /// </summary>
    public static TwoDimArray<DamageLevelEntry> DamageLevelTable { get; private set; } = null!;

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
    /// Gets the programmed effect table (progfx.2da)
    /// </summary>
    public static TwoDimArray<ProgrammedEffectTableEntry> ProgrammedEffectTable { get; private set; } = null!;

    /// <summary>
    /// Gets the item/skill cost table (skillvsitemcost.2da)
    /// </summary>
    public static TwoDimArray<SkillItemCostTableEntry> SkillItemCostTable { get; private set; } = null!;

    /// <summary>
    /// Gets the visual effect table (visualeffects.2da)
    /// </summary>
    public static TwoDimArray<VisualEffectTableEntry> VisualEffectTable { get; private set; } = null!;

    [ServiceBinding(typeof(Factory))]
    [ServiceBindingOptions(InternalBindingPriority.API)]
    internal sealed unsafe class Factory
    {
      private readonly FunctionHook<ReloadAllHook> reloadAllHook;

      public Factory(HookService hookService)
      {
        reloadAllHook = hookService.RequestHook<ReloadAllHook>(OnReloadAll, FunctionsLinux._ZN8CNWRules9ReloadAllEv, HookOrder.Latest);
        LoadTables();
      }

      private delegate void ReloadAllHook(void* pRules);

      private static void LoadTables()
      {
        CTwoDimArrays arrays = NWNXLib.Rules().m_p2DArrays;

        AppearanceTable = new TwoDimArray<AppearanceTableEntry>(arrays.m_pAppearanceTable);
        ArmorTable = new TwoDimArray<ArmorTableEntry>(arrays.m_pArmorTable);
        BodyBagTable = new TwoDimArray<BodyBagTableEntry>(arrays.m_pBodyBagTable);
        EnvironmentPresetTable = new TwoDimArray<EnvironmentPreset>("environment.2da");
        LightColorTable = new TwoDimArray<LightColorTableEntry>(arrays.m_pLightColorTable);
        PartsBeltTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsBelt);
        PartsBicepTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsBicep);
        PartsChestTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsChest);
        PartsFootTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsFoot);
        PartsForearmTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsForearm);
        PartsHandTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsHand);
        PartsLegTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsLegs);
        PartsNeckTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsNeck);
        PartsPelvisTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsPelvis);
        PartsRobeTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsRobe);
        PartsShinTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsShin);
        PartsShoulderTable = new TwoDimArray<PartsTableEntry>(arrays.m_pPartsShoulder);
        PlaceableSoundTable = new TwoDimArray<PlaceableSoundTableEntry>("placeableobjsnds.2da"); // arrays.m_pPlaceableSoundsTable does not exist in nwserver.
        PlaceableTable = new TwoDimArray<PlaceableTableEntry>(arrays.m_pPlaceablesTable);
        VisualEffectTable = new TwoDimArray<VisualEffectTableEntry>(arrays.m_pVisualEffectTable);
        ProgrammedEffectTable = new TwoDimArray<ProgrammedEffectTableEntry>("progfx.2da"); // arrays.m_pProgFxTable does not exist in nwserver.
        DamageLevelTable = new TwoDimArray<DamageLevelEntry>("damagelevels.2da"); // arrays.m_pDamageLevelTable does not exist in nwserver.
        ExpTable = new TwoDimArray<ExpTableEntry>("exptable.2da");
        SkillItemCostTable = new TwoDimArray<SkillItemCostTableEntry>(arrays.m_pSkillVsItemCostTable);
      }

      private void OnReloadAll(void* pRules)
      {
        reloadAllHook.CallOriginal(pRules);
        LoadTables();
      }
    }
  }
}
