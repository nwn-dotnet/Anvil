using System.Collections.Generic;
using System.Linq;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A base item type definition.
  /// </summary>
  public sealed unsafe class NwBaseItem
  {
    [Inject]
    private static TlkTable TlkTable { get; set; }

    internal readonly CNWBaseItem BaseItemInfo;

    internal NwBaseItem(uint baseItemId, CNWBaseItem baseItemInfo)
    {
      Id = baseItemId;
      BaseItemInfo = baseItemInfo;
    }

    public ACBonus ACBonusType
    {
      get => (ACBonus)BaseItemInfo.m_nACEnchantmentType;
    }

    public BaseItemAmmunitionType AmmunitionType
    {
      get => (BaseItemAmmunitionType)BaseItemInfo.m_nAmmunitionType;
    }

    public byte AnimSlashL
    {
      get => BaseItemInfo.m_nPercentageSlashL;
    }

    public byte AnimSlashR
    {
      get => BaseItemInfo.m_nPercentageSlashR;
    }

    public byte AnimSlashS
    {
      get => BaseItemInfo.m_nPercentageSlashS;
    }

    public byte ArcaneSpellFailure
    {
      get => BaseItemInfo.m_nArcaneSpellFailure;
    }

    public byte ArmorCheckPenalty
    {
      get => BaseItemInfo.m_nArmorCheckPenalty;
    }

    public byte BaseAC
    {
      get => BaseItemInfo.m_nBaseAC;
    }

    public float BaseCost
    {
      get => BaseItemInfo.m_nBaseCost;
    }

    public string BaseItemStatsText
    {
      get => TlkTable.GetSimpleString(BaseItemInfo.m_nStatsString);
    }

    public BaseItemCategory Category
    {
      get => (BaseItemCategory)BaseItemInfo.m_nCategory;
    }

    public float CostMultiplier
    {
      get => BaseItemInfo.m_nCostMultiplier;
    }

    public byte CritMultiplier
    {
      get => BaseItemInfo.m_nCritMult;
    }

    public byte CritThreat
    {
      get => BaseItemInfo.m_nCritThreat;
    }

    public string DefaultIcon
    {
      get => BaseItemInfo.m_DefaultIconResRef.ToString();
    }

    public string DefaultModel
    {
      get => BaseItemInfo.m_DefaultModelResRef.ToString();
    }

    public string Description
    {
      get => TlkTable.GetSimpleString(BaseItemInfo.m_nDescription);
    }

    public byte DieToRoll
    {
      get => BaseItemInfo.m_nDieToRoll;
    }

    public NwFeat EpicDevastatingCriticalFeat
    {
      get => NwFeat.FromFeatId(BaseItemInfo.m_nEpicWeaponDevastatingCriticalFeat);
    }

    public NwFeat EpicOverwhelmingCriticalFeat
    {
      get => NwFeat.FromFeatId(BaseItemInfo.m_nEpicWeaponOverwhelmingCriticalFeat);
    }

    public NwFeat EpicWeaponFocusFeat
    {
      get => NwFeat.FromFeatId(BaseItemInfo.m_nEpicWeaponFocusFeat);
    }

    public NwFeat EpicWeaponSpecializationFeat
    {
      get => NwFeat.FromFeatId(BaseItemInfo.m_nEpicWeaponSpecializationFeat);
    }

    /// <summary>
    /// Gets the id of this base item.
    /// </summary>
    public uint Id { get; }

    public byte ILRStackSize
    {
      get => BaseItemInfo.m_nILRStackSize;
    }

    public NwFeat ImprovedCriticalFeat
    {
      get => NwFeat.FromFeatId(BaseItemInfo.m_nWeaponImprovedCriticalFeat);
    }

    /// <summary>
    /// Gets the size/number of inventory slots that this base item occupies.
    /// </summary>
    public Vector2Int InventorySlotSize
    {
      get => new Vector2Int(BaseItemInfo.m_nInvSlotWidth, BaseItemInfo.m_nInvSlotHeight);
    }

    public byte InvSoundTypeIndex
    {
      get => BaseItemInfo.m_InventorySoundType;
    }

    public bool IsContainer
    {
      get => BaseItemInfo.m_bContainer.ToBool();
    }

    public bool IsGenderSpecific
    {
      get => BaseItemInfo.m_bGenderSpecific.ToBool();
    }

    /// <summary>
    /// Gets if this base item is a monk weapon.
    /// </summary>
    public bool IsMonkWeapon
    {
      get => BaseItemInfo.m_bIsMonkWeapon.ToBool();
    }

    /// <summary>
    /// Gets if this base item is a ranged weapon.
    /// </summary>
    public bool IsRangedWeapon
    {
      get => BaseItemInfo.m_nWeaponRanged > 0;
    }

    /// <summary>
    /// Gets if this item type can be stacked.
    /// </summary>
    public bool IsStackable
    {
      get => MaxStackSize > 1;
    }

    public string ItemClass
    {
      get => BaseItemInfo.m_ItemClassResRefChunk.ToString();
    }

    public byte ItemPropertiesMax
    {
      get => BaseItemInfo.m_nMaxProperties;
    }

    public byte ItemPropertiesMin
    {
      get => BaseItemInfo.m_nMinProperties;
    }

    public byte ItemPropertyColumnId
    {
      get => BaseItemInfo.m_nPropColumn;
    }

    /// <summary>
    /// Gets the associated <see cref="BaseItemType"/> for this base item.
    /// </summary>
    public BaseItemType ItemType
    {
      get => (BaseItemType)Id;
    }

    /// <summary>
    /// Gets the maximum stack size for this base item.
    /// </summary>
    public int MaxStackSize
    {
      get => BaseItemInfo.m_nStackSize;
    }

    public float ModelRangeMax
    {
      get => BaseItemInfo.m_nMaxRange;
    }

    public float ModelRangeMin
    {
      get => BaseItemInfo.m_nMinRange;
    }

    public BaseItemModelType ModelType
    {
      get => (BaseItemModelType)BaseItemInfo.m_nModelType;
    }

    public string Name
    {
      get => TlkTable.GetSimpleString(BaseItemInfo.m_nName);
    }

    public byte NumDamageDice
    {
      get => BaseItemInfo.m_nNumDice;
    }

    /// <summary>
    /// Gets or sets the preferred attack distance for this base item.
    /// </summary>
    public float PreferredAttackDistance
    {
      get => BaseItemInfo.m_fPreferredAttackDist;
      set => BaseItemInfo.m_fPreferredAttackDist = value;
    }

    public BaseItemQBBehaviour QBBehaviour
    {
      get => (BaseItemQBBehaviour)BaseItemInfo.m_nQBBehaviourType;
    }

    public IEnumerable<NwFeat> RequiredFeats
    {
      get
      {
        NwFeat[] retVal = new NwFeat[BaseItemInfo.m_nRequiredFeatCount];
        for (int i = 0; i < retVal.Length; i++)
        {
          retVal[i] = NwFeat.FromFeatId(BaseItemInfo.m_pRequiredFeats[i]);
        }

        return retVal;
      }
    }

    public bool RotateIcon
    {
      get => BaseItemInfo.m_bCanRotateIcon.ToBool();
    }

    public BaseItemRotation RotateOnGround
    {
      get => (BaseItemRotation)BaseItemInfo.m_nRotateOnGround;
    }

    public byte StartingCharges
    {
      get => BaseItemInfo.m_nStartingCharges;
    }

    public StoreCategory StoreCategory
    {
      get => (StoreCategory)BaseItemInfo.m_nStorePanel;
    }

    public byte StoreSortOrder
    {
      get => BaseItemInfo.m_nStorePanelSort;
    }

    public CreatureSize WeaponFinesseMinimumCreatureSize
    {
      get => (CreatureSize)BaseItemInfo.m_nWeaponFinesseMinimumCreatureSize;
    }

    public NwFeat WeaponFocusFeat
    {
      get => NwFeat.FromFeatId(BaseItemInfo.m_nWeaponFocusFeat);
    }

    public byte WeaponMaterialTypeIndex
    {
      get => BaseItemInfo.m_nWeaponMaterialType;
    }

    public NwFeat WeaponOfChoiceFeat
    {
      get => NwFeat.FromFeatId(BaseItemInfo.m_nWeaponOfChoiceFeat);
    }

    /// <summary>
    /// Gets the <see cref="WeaponSize"/> of this base item if the item is a weapon.
    /// </summary>
    public BaseItemWeaponSize WeaponSize
    {
      get => (BaseItemWeaponSize)BaseItemInfo.m_nWeaponSize;
    }

    public NwFeat WeaponSpecializationFeat
    {
      get => NwFeat.FromFeatId(BaseItemInfo.m_nWeaponSpecializationFeat);
    }

    public IEnumerable<DamageType> WeaponType
    {
      get
      {
        return BaseItemInfo.m_nWeaponType switch
        {
          1 => DamageType.Piercing.Yield(),
          2 => DamageType.Bludgeoning.Yield(),
          3 => DamageType.Slashing.Yield(),
          4 => new[]
          {
            DamageType.Slashing, DamageType.Piercing,
          },
          5 => new[]
          {
            DamageType.Piercing, DamageType.Bludgeoning,
          },
          _ => Enumerable.Empty<DamageType>(),
        };
      }
    }

    public BaseItemWeaponWieldType WeaponWieldType
    {
      get => (BaseItemWeaponWieldType)BaseItemInfo.m_nWeaponWield;
    }

    public decimal Weight
    {
      get => BaseItemInfo.m_nWeight / 10.0m;
    }

    /// <summary>
    /// Resolves a <see cref="NwBaseItem"/> from a base item id.
    /// </summary>
    /// <param name="itemId">The item id to resolve.</param>
    /// <returns>The associated <see cref="NwBaseItem"/> instance. Null if the base item id is invalid.</returns>
    public static NwBaseItem FromItemId(int itemId)
    {
      return NwRuleset.BaseItems.ElementAtOrDefault(itemId);
    }

    /// <summary>
    /// Resolves a <see cref="NwBaseItem"/> from a <see cref="BaseItemType"/>.
    /// </summary>
    /// <param name="itemType">The item type to resolve.</param>
    /// <returns>The associated <see cref="NwBaseItem"/> instance. Null if the base item type is invalid.</returns>
    public static NwBaseItem FromItemType(BaseItemType itemType)
    {
      return NwRuleset.BaseItems.ElementAtOrDefault((int)itemType);
    }

    public static implicit operator NwBaseItem(BaseItemType itemType)
    {
      return NwRuleset.BaseItems.ElementAtOrDefault((int)itemType);
    }

    public bool IsPartUsingEnvMap(int partIndex)
    {
      return BaseItemInfo.m_bPartEnvMap[partIndex].ToBool();
    }
  }
}
