using System;
using System.Collections.Generic;
using Anvil.Native;
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
    public static TwoDimArray<T>? GetTable<T>(string? twoDimArrayName, bool useCache = true, bool checkCacheType = true) where T : class, ITwoDimArrayEntry, new()
    {
      if (string.IsNullOrEmpty(twoDimArrayName))
      {
        return null;
      }

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

      C2DA? nativeArray = GetCached2DA(twoDimArrayName);
      if (nativeArray == null)
      {
        return null;
      }

      TwoDimArray<T> twoDimArray = new TwoDimArray<T>(nativeArray);
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
    public static TwoDimArray? GetTable(string twoDimArrayName)
    {
      if (string.IsNullOrEmpty(twoDimArrayName))
      {
        return null;
      }

      twoDimArrayName = twoDimArrayName.Replace(".2da", string.Empty);
      if (TwoDimArrayCache.TryGetValue(twoDimArrayName, out TwoDimArray? cachedArray))
      {
        return cachedArray;
      }

      C2DA? nativeArray = GetCached2DA(twoDimArrayName);
      if (nativeArray == null)
      {
        return null;
      }

      TwoDimArray twoDimArray = new TwoDimArray(nativeArray);
      TwoDimArrayCache[twoDimArrayName] = twoDimArray;

      return twoDimArray;
    }

    internal static TwoDimArray<T> GetTable<T>(C2DA twoDimArray) where T : class, ITwoDimArrayEntry, new()
    {
      TwoDimArray<T> retVal = new TwoDimArray<T>(twoDimArray);
      return retVal;
    }

    private static C2DA? GetCached2DA(string twoDimArrayName)
    {
      twoDimArrayName = twoDimArrayName.Replace(".2da", string.Empty);
      C2DA? array = NWNXLib.Rules().m_p2DArrays.GetCached2DA(twoDimArrayName.ToExoString(), false.ToInt());

      return array;
    }

    [ServiceBinding(typeof(Factory))]
    [ServiceBindingOptions(InternalBindingPriority.API)]
    internal sealed unsafe class Factory : IDisposable
    {
      private readonly FunctionHook<Functions.CNWRules.ReloadAll> reloadAllHook;

      public Factory(HookService hookService)
      {
        reloadAllHook = hookService.RequestHook<Functions.CNWRules.ReloadAll>(OnReloadAll, HookOrder.Latest);
        LoadTables();
      }

      private void LoadTables()
      {
        TwoDimArrayCache.Clear();
        CTwoDimArrays arrays = NWNXLib.Rules().m_p2DArrays;

        AppearanceTable = GetTable<AppearanceTableEntry>(arrays.m_pAppearanceTable);
        ArmorTable = GetTable<ArmorTableEntry>(arrays.m_pArmorTable);
        BodyBagTable = GetTable<BodyBagTableEntry>(arrays.m_pBodyBagTable);
        EffectIconTable = GetTable<EffectIconTableEntry>("effecticons.2da")!;
        EnvironmentPresetTable = GetTable<EnvironmentPreset>("environment.2da")!;
        LightColorTable = GetTable<LightColorTableEntry>(arrays.m_pLightColorTable);
        LoadScreenTable = GetTable<LoadScreenTableEntry>("loadscreens.2da")!;
        ItemPropertyCostTables = GetTable<ItemPropertyCostTablesEntry>("iprp_costtable.2da")!;
        ItemPropertyParamTables = GetTable<ItemPropertyParamTablesEntry>("iprp_paramtable.2da")!;
        ItemPropertyItemMapTable = GetTable<ItemPropertyItemMapTableEntry>(arrays.m_pItemPropsTable);
        ItemPropertyTable = GetTable<ItemPropertyTableEntry>(arrays.m_pItemPropDefTable);
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
        PlaceableSoundTable = GetTable<PlaceableSoundTableEntry>("placeableobjsnds.2da")!; // arrays.m_pPlaceableSoundsTable does not exist in nwserver.
        PlaceableTable = GetTable<PlaceableTableEntry>(arrays.m_pPlaceablesTable);
        VisualEffectTable = GetTable<VisualEffectTableEntry>(arrays.m_pVisualEffectTable);
        ProgrammedEffectTable = GetTable<ProgrammedEffectTableEntry>("progfx.2da")!; // arrays.m_pProgFxTable does not exist in nwserver.
        PersistentEffectTable = GetTable<PersistentVfxTableEntry>(arrays.m_pPersistentVisualEffectTable); // arrays.m_pProgFxTable does not exist in nwserver.
        DamageLevelTable = GetTable<DamageLevelEntry>("damagelevels.2da")!; // arrays.m_pDamageLevelTable does not exist in nwserver.
        ExpTable = GetTable<ExpTableEntry>("exptable.2da")!;
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
