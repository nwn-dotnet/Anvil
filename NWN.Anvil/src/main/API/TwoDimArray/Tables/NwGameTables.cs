using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  public static class NwGameTables
  {
    /// <summary>
    /// Gets the game appearance table (appearance.2da)
    /// </summary>
    public static TwoDimArray<AppearanceTableEntry> AppearanceTable { get; private set; }

    /// <summary>
    /// Gets the game body bag table (bodybag.2da)
    /// </summary>
    public static TwoDimArray<BodyBagTableEntry> BodyBagTable { get; private set; }

    /// <summary>
    /// Gets the game environment preset table (environment.2da)
    /// </summary>
    public static TwoDimArray<EnvironmentPreset> EnvironmentPresetTable { get; private set; }

    /// <summary>
    /// Gets the game light color table (lightcolor.2da)
    /// </summary>
    public static TwoDimArray<LightColorTableEntry> LightColorTable { get; private set; }

    /// <summary>
    /// Gets the game placeable sound table (placeableobjsnds.2da)
    /// </summary>
    public static TwoDimArray<PlaceableSoundTableEntry> PlaceableSoundTable { get; private set; }

    /// <summary>
    /// Gets the game placeable table (placeables.2da)
    /// </summary>
    public static TwoDimArray<PlaceableTableEntry> PlaceableTable { get; private set; }

    [ServiceBinding(typeof(Factory))]
    [ServiceBindingOptions(InternalBindingPriority.API)]
    internal sealed unsafe class Factory
    {
      private readonly FunctionHook<ReloadAllHook> reloadAllHook;

      private delegate void ReloadAllHook(void* pRules);

      public Factory(HookService hookService)
      {
        reloadAllHook = hookService.RequestHook<ReloadAllHook>(OnReloadAll, FunctionsLinux._ZN8CNWRules9ReloadAllEv, HookOrder.Latest);
        LoadTables();
      }

      private static void LoadTables()
      {
        CTwoDimArrays arrays = NWNXLib.Rules().m_p2DArrays;

        AppearanceTable = new TwoDimArray<AppearanceTableEntry>(arrays.m_pAppearanceTable);
        BodyBagTable = new TwoDimArray<BodyBagTableEntry>(arrays.m_pBodyBagTable);
        EnvironmentPresetTable = new TwoDimArray<EnvironmentPreset>("environment.2da");
        LightColorTable = new TwoDimArray<LightColorTableEntry>(arrays.m_pLightColorTable);
        PlaceableSoundTable = new TwoDimArray<PlaceableSoundTableEntry>("placeableobjsnds.2da"); // arrays.m_pPlaceableSoundsTable does not exist in nwserver.
        PlaceableTable = new TwoDimArray<PlaceableTableEntry>(arrays.m_pPlaceablesTable);
      }

      private void OnReloadAll(void* pRules)
      {
        reloadAllHook.CallOriginal(pRules);
        LoadTables();
      }
    }
  }
}
