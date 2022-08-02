using System;
using System.Collections.Generic;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  public static partial class NwGameTables
  {
    private static readonly Dictionary<string, TwoDimArray> TwoDimArrayCache = new Dictionary<string, TwoDimArray>();

    /// <summary>
    /// Gets the specified 2d array table.
    /// </summary>
    /// <param name="twoDimArrayName">The table name to query.</param>
    /// <param name="useCache">Enables/disables caching of the decoded 2da for future use.</param>
    /// <param name="checkCacheType">When using the cache, if the return type should be checked.</param>
    /// <typeparam name="T">The type of entries contained in this 2da.</typeparam>
    /// <exception cref="InvalidOperationException">Thrown if the entry type specified does not match the existing cache type and checkCacheType is set to true.</exception>
    public static TwoDimArray<T> GetTable<T>(string twoDimArrayName, bool useCache = true, bool checkCacheType = true) where T : class, ITwoDimArrayEntry, new()
    {
      twoDimArrayName = twoDimArrayName.Replace(".2da", string.Empty);
      if (useCache && TwoDimArrayCache.TryGetValue(twoDimArrayName, out TwoDimArray? cachedArray))
      {
        if (cachedArray is TwoDimArray<T> cachedTypedArray)
        {
          return cachedTypedArray;
        }

        if (checkCacheType)
        {
          throw new InvalidOperationException($"Specified 2da type {nameof(TwoDimArray)}<{typeof(T).Name}> does not match expected type {cachedArray.GetType().FullName}");
        }
      }

      TwoDimArray<T> twoDimArray = new TwoDimArray<T>(GetCached2DA(twoDimArrayName));
      if (useCache)
      {
        TwoDimArrayCache[twoDimArrayName] = twoDimArray;
      }

      return twoDimArray;
    }

    /// <summary>
    /// Gets the specified 2d array table.
    /// </summary>
    /// <param name="twoDimArrayName">The table name to query.</param>
    public static TwoDimArray GetTable(string twoDimArrayName)
    {
      twoDimArrayName = twoDimArrayName.Replace(".2da", string.Empty);
      if (TwoDimArrayCache.TryGetValue(twoDimArrayName, out TwoDimArray? cachedArray))
      {
        return cachedArray;
      }

      TwoDimArray twoDimArray = new TwoDimArray(GetCached2DA(twoDimArrayName));
      TwoDimArrayCache[twoDimArrayName] = twoDimArray;

      return twoDimArray;
    }

    internal static TwoDimArray<T> GetTable<T>(C2DA twoDimArray) where T : class, ITwoDimArrayEntry, new()
    {
      TwoDimArray<T> retVal = new TwoDimArray<T>(twoDimArray);
      return retVal;
    }

    private static C2DA GetCached2DA(string twoDimArrayName)
    {
      twoDimArrayName = twoDimArrayName.Replace(".2da", string.Empty);
      C2DA? array = NWNXLib.Rules().m_p2DArrays.GetCached2DA(twoDimArrayName.ToExoString(), false.ToInt());
      if (array == null)
      {
        throw new ArgumentException($"Invalid 2DA ResRef {twoDimArrayName}.2da", nameof(twoDimArrayName));
      }

      return array;
    }

    [ServiceBinding(typeof(Factory))]
    [ServiceBindingOptions(InternalBindingPriority.API)]
    internal sealed unsafe class Factory : IDisposable
    {
      private readonly FunctionHook<ReloadAllHook> reloadAllHook;

      public Factory(HookService hookService)
      {
        reloadAllHook = hookService.RequestHook<ReloadAllHook>(OnReloadAll, FunctionsLinux._ZN8CNWRules9ReloadAllEv, HookOrder.Latest);
        LoadTables();
      }

      private delegate void ReloadAllHook(void* pRules);

      private void LoadTables()
      {
        TwoDimArrayCache.Clear();
        CTwoDimArrays arrays = NWNXLib.Rules().m_p2DArrays;

        AppearanceTable = GetTable<AppearanceTableEntry>(arrays.m_pAppearanceTable);
        ArmorTable = GetTable<ArmorTableEntry>(arrays.m_pArmorTable);
        BodyBagTable = GetTable<BodyBagTableEntry>(arrays.m_pBodyBagTable);
        EnvironmentPresetTable = GetTable<EnvironmentPreset>("environment.2da");
        LightColorTable = GetTable<LightColorTableEntry>(arrays.m_pLightColorTable);
        PartsBeltTable = GetTable<PartsTableEntry>(arrays.m_pPartsBelt);
        PartsBicepTable = GetTable<PartsTableEntry>(arrays.m_pPartsBicep);
        PartsChestTable = GetTable<PartsTableEntry>(arrays.m_pPartsChest);
        PartsFootTable = GetTable<PartsTableEntry>(arrays.m_pPartsFoot);
        PartsForearmTable = GetTable<PartsTableEntry>(arrays.m_pPartsForearm);
        PartsHandTable = GetTable<PartsTableEntry>(arrays.m_pPartsHand);
        PartsLegTable = GetTable<PartsTableEntry>(arrays.m_pPartsLegs);
        PartsNeckTable = GetTable<PartsTableEntry>(arrays.m_pPartsNeck);
        PartsPelvisTable = GetTable<PartsTableEntry>(arrays.m_pPartsPelvis);
        PartsRobeTable = GetTable<PartsTableEntry>(arrays.m_pPartsRobe);
        PartsShinTable = GetTable<PartsTableEntry>(arrays.m_pPartsShin);
        PartsShoulderTable = GetTable<PartsTableEntry>(arrays.m_pPartsShoulder);
        PlaceableSoundTable = GetTable<PlaceableSoundTableEntry>("placeableobjsnds.2da"); // arrays.m_pPlaceableSoundsTable does not exist in nwserver.
        PlaceableTable = GetTable<PlaceableTableEntry>(arrays.m_pPlaceablesTable);
        VisualEffectTable = GetTable<VisualEffectTableEntry>(arrays.m_pVisualEffectTable);
        ProgrammedEffectTable = GetTable<ProgrammedEffectTableEntry>("progfx.2da"); // arrays.m_pProgFxTable does not exist in nwserver.
        DamageLevelTable = GetTable<DamageLevelEntry>("damagelevels.2da"); // arrays.m_pDamageLevelTable does not exist in nwserver.
        ExpTable = GetTable<ExpTableEntry>("exptable.2da");
        SkillItemCostTable = GetTable<SkillItemCostTableEntry>(arrays.m_pSkillVsItemCostTable);
      }

      private void OnReloadAll(void* pRules)
      {
        reloadAllHook.CallOriginal(pRules);
        LoadTables();
      }

      public void Dispose()
      {
        TwoDimArrayCache.Clear();
      }
    }
  }
}
