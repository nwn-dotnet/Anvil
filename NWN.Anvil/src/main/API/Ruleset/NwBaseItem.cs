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

    /// <summary>
    /// The type of armor class bonus granted, if this item grants one.
    /// </summary>
    public ACBonus ACBonusType => (ACBonus)BaseItemInfo.m_nACEnchantmentType;

    /// <summary>
    /// If this item is a ranged weapon, gets the ammunition type used.
    /// </summary>
    public BaseItemAmmunitionType AmmunitionType => (BaseItemAmmunitionType)BaseItemInfo.m_nAmmunitionType;

    /// <summary>
    /// Gets the chance (expressed as a percentage) that a character wielding this weapon (item) will be animated as making a slash from the left during an attack.
    /// </summary>
    public byte AnimSlashL => BaseItemInfo.m_nPercentageSlashL;

    /// <summary>
    /// Gets the chance (expressed as a percentage) that a character wielding this weapon (item) will be animated as making a slash from the right during an attack.
    /// </summary>
    public byte AnimSlashR => BaseItemInfo.m_nPercentageSlashR;

    /// <summary>
    /// Gets the chance (expressed as a percentage) that a character wielding this weapon (item) will be animated as making a straight slash (from the center) during an attack.
    /// </summary>
    public byte AnimSlashS => BaseItemInfo.m_nPercentageSlashS;

    /// <summary>
    /// Gets the percentage this item adds to the chance for arcane spell failure. Not used for armor which uses armor.2da.
    /// </summary>
    public byte ArcaneSpellFailure => BaseItemInfo.m_nArcaneSpellFailure;

    /// <summary>
    /// Gets the armor check penalty inherent to this item. Not used for armor which uses armor.2da
    /// </summary>
    public byte ArmorCheckPenalty => BaseItemInfo.m_nArmorCheckPenalty;

    /// <summary>
    /// Gets the armor class value of this shield (before enchantments). Not used for armor.
    /// </summary>
    public byte BaseAC => BaseItemInfo.m_nBaseAC;

    /// <summary>
    /// Gets the base value (in gold pieces) of an unenchanted item of this type.
    /// </summary>
    public float BaseCost => BaseItemInfo.m_nBaseCost;

    /// <summary>
    /// Gets the string containing basic statistics of this item. This is displayed under the description when an item is examined in the game.
    /// </summary>
    public string BaseItemStatsText => TlkTable.GetSimpleString(BaseItemInfo.m_nStatsString);

    /// <summary>
    /// Gets if the inventory icon for this item may be rotated 90 degrees clockwise, such as when placed on a player's quickbar.
    /// </summary>
    public bool CanRotateIcon => BaseItemInfo.m_bCanRotateIcon.ToBool();

    public BaseItemCategory Category => (BaseItemCategory)BaseItemInfo.m_nCategory;

    /// <summary>
    /// Gets the cost multiplier of the item.<br/>
    /// Used in the cost calculation of magical items, see itemprops.2da, itempropdef.2da, iprp_costtable.2da and iprp_paramtable.2da.
    /// </summary>
    public float CostMultiplier => BaseItemInfo.m_nCostMultiplier;

    /// <summary>
    /// The critical multiplier for this weapon (item).
    /// </summary>
    public byte CritMultiplier => BaseItemInfo.m_nCritMult;

    /// <summary>
    /// Gets the chance (out of 20) that this weapon (item) will threaten a critical hit. For example, "2" would mean two chances out of 20, for a threat range of 19-20.
    /// </summary>
    public byte CritThreat => BaseItemInfo.m_nCritThreat;

    /// <summary>
    /// Gets the ResRef of the default icon that is used when the item's icon doesn't exist.
    /// </summary>
    public string DefaultIcon => BaseItemInfo.m_DefaultIconResRef.ToString();

    /// <summary>
    /// Gets the default model used when the item is lying on the ground.
    /// </summary>
    public string DefaultModel => BaseItemInfo.m_DefaultModelResRef.ToString();

    /// <summary>
    /// Gets the default description for this item type. This is also used in game for items that lack specific descriptions.
    /// </summary>
    public string Description => TlkTable.GetSimpleString(BaseItemInfo.m_nDescription);

    /// <summary>
    /// Gets the number of sides of the dice rolled for this weapon's (item's) damage.
    /// </summary>
    public byte DieToRoll => BaseItemInfo.m_nDieToRoll;

    /// <summary>
    /// Gets the character feat that allows devastating criticals to be performed with this item.
    /// </summary>
    public NwFeat EpicDevastatingCriticalFeat => NwFeat.FromFeatId(BaseItemInfo.m_nEpicWeaponDevastatingCriticalFeat);

    /// <summary>
    /// Gets the character feat that allows overwhelming criticals to be performed with this item.
    /// </summary>
    public NwFeat EpicOverwhelmingCriticalFeat => NwFeat.FromFeatId(BaseItemInfo.m_nEpicWeaponOverwhelmingCriticalFeat);

    /// <summary>
    /// Gets the character feat that provides epic weapon focus for this item.
    /// </summary>
    public NwFeat EpicWeaponFocusFeat => NwFeat.FromFeatId(BaseItemInfo.m_nEpicWeaponFocusFeat);

    /// <summary>
    /// Gets the character feat that provides epic weapon specialization for this item.
    /// </summary>
    public NwFeat EpicWeaponSpecializationFeat => NwFeat.FromFeatId(BaseItemInfo.m_nEpicWeaponSpecializationFeat);

    /// <summary>
    /// Gets the id of this base item.
    /// </summary>
    public uint Id { get; }

    /// <summary>
    /// Gets the stack size used when calculating this item's value in regards to item level restrictions.
    /// </summary>
    public byte ILRStackSize => BaseItemInfo.m_nILRStackSize;

    /// <summary>
    /// Gets the character feat that allows improved criticals to be performed with this item.
    /// </summary>
    public NwFeat ImprovedCriticalFeat => NwFeat.FromFeatId(BaseItemInfo.m_nWeaponImprovedCriticalFeat);

    /// <summary>
    /// Gets the size/number of inventory slots that this base item occupies.
    /// </summary>
    public Vector2Int InventorySlotSize => new Vector2Int(BaseItemInfo.m_nInvSlotWidth, BaseItemInfo.m_nInvSlotHeight);

    /// <summary>
    /// Gets the index in inventorysnds.2da indicating the sound used when this item is placed (or moved) in a character's inventory.
    /// </summary>
    public byte InvSoundTypeIndex => BaseItemInfo.m_InventorySoundType;

    /// <summary>
    /// Gets if this item is considered a container, and can contain other items, like a bag.
    /// </summary>
    public bool IsContainer => BaseItemInfo.m_bContainer.ToBool();

    /// <summary>
    /// Gets if the item changes appearance based on the gender of the possessor.
    /// </summary>
    public bool IsGenderSpecific => BaseItemInfo.m_bGenderSpecific.ToBool();

    /// <summary>
    /// Gets if this base item is a monk weapon.
    /// </summary>
    public bool IsMonkWeapon => BaseItemInfo.m_bIsMonkWeapon.ToBool();

    /// <summary>
    /// Gets if this base item is a ranged weapon.
    /// </summary>
    public bool IsRangedWeapon => BaseItemInfo.m_nWeaponRanged > 0;

    /// <summary>
    /// Gets if this item type can be stacked.
    /// </summary>
    public bool IsStackable => MaxStackSize > 1;

    /// <summary>
    /// Gets the ResRef of the item's model, or the base part of the resref.<br/>
    /// This property is dependent on <see cref="ModelType"/>. See https://nwn.wiki/display/NWN1/baseitems.2da for more info.
    /// </summary>
    public string ItemClass => BaseItemInfo.m_ItemClassResRefChunk.ToString();

    /// <summary>
    /// Gets the maximum number of "cast spell" properties items of this type can be given when designed in the Toolset.
    /// </summary>
    public byte ItemPropertiesMax => BaseItemInfo.m_nMaxProperties;

    /// <summary>
    /// Gets the minimum number of "cast spell" properties items of this type must be given when designed in the Toolset.
    /// </summary>
    public byte ItemPropertiesMin => BaseItemInfo.m_nMinProperties;

    public byte ItemPropertyColumnId => BaseItemInfo.m_nPropColumn;

    /// <summary>
    /// Gets the associated <see cref="BaseItemType"/> for this base item.
    /// </summary>
    public BaseItemType ItemType => (BaseItemType)Id;

    /// <summary>
    /// Gets the maximum stack size for this base item.
    /// </summary>
    public int MaxStackSize => BaseItemInfo.m_nStackSize;

    /// <summary>
    /// Gets the maximum allowed number of models of this type.
    /// </summary>
    public float ModelRangeMax => BaseItemInfo.m_nMaxRange;

    /// <summary>
    /// Gets the minimum number of models of this type.
    /// </summary>
    public float ModelRangeMin => BaseItemInfo.m_nMinRange;

    /// <summary>
    /// Gets a value indicating how much the look of this item can be customized.
    /// </summary>
    public BaseItemModelType ModelType => (BaseItemModelType)BaseItemInfo.m_nModelType;

    /// <summary>
    /// Gets the base name of this item. Used for unidentified items.
    /// </summary>
    public string Name => TlkTable.GetSimpleString(BaseItemInfo.m_nName);

    /// <summary>
    /// Gets the number of dice rolled for this weapon's (item's) damage.
    /// </summary>
    public byte NumDamageDice => BaseItemInfo.m_nNumDice;

    /// <summary>
    /// Gets or sets the preferred attack distance for this base item.
    /// </summary>
    public float PreferredAttackDistance
    {
      get => BaseItemInfo.m_fPreferredAttackDist;
      set => BaseItemInfo.m_fPreferredAttackDist = value;
    }

    /// <summary>
    /// Gets a list of feats that grants proficiency with this item. A character must be proficient with an item in order to equip or use it.
    /// </summary>
    public IEnumerable<NwFeat> PrerequisiteFeats
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

    /// <summary>
    /// Gets the behaviour when this property appears on the player's quick bar.
    /// </summary>
    public BaseItemQBBehaviour QBBehaviour => (BaseItemQBBehaviour)BaseItemInfo.m_nQBBehaviourType;

    /// <summary>
    /// Gets the rotation of the item model when laid on the ground.
    /// </summary>
    public BaseItemRotation RotateOnGround => (BaseItemRotation)BaseItemInfo.m_nRotateOnGround;

    /// <summary>
    /// Gets the starting number of charges this item has by default.
    /// </summary>
    public byte StartingCharges => BaseItemInfo.m_nStartingCharges;

    /// <summary>
    /// Gets where this item can be found in a store.
    /// </summary>
    public StoreCategory StoreCategory => (StoreCategory)BaseItemInfo.m_nStorePanel;

    /// <summary>
    /// Gets a number indicating the order in which this item will be listed in a store.
    /// </summary>
    public byte StoreSortOrder => BaseItemInfo.m_nStorePanelSort;

    /// <summary>
    /// Gets the minimum creature size required for this weapon (item) to be considered a finesse weapon, for the <see cref="Feat.WeaponFinesse"/> feat.
    /// </summary>
    public CreatureSize WeaponFinesseMinimumCreatureSize => (CreatureSize)BaseItemInfo.m_nWeaponFinesseMinimumCreatureSize;

    /// <summary>
    /// Gets the character feat that provides weapon focus for this item.
    /// </summary>
    public NwFeat WeaponFocusFeat => NwFeat.FromFeatId(BaseItemInfo.m_nWeaponFocusFeat);

    /// <summary>
    /// Gets an index into weaponsounds.2da indicating what sounds this weapon (item) makes when it hits an opponent.
    /// </summary>
    public byte WeaponMaterialTypeIndex => BaseItemInfo.m_nWeaponMaterialType;

    /// <summary>
    /// Gets the character feat that identifies this item as a weapon of choice.
    /// </summary>
    public NwFeat WeaponOfChoiceFeat => NwFeat.FromFeatId(BaseItemInfo.m_nWeaponOfChoiceFeat);

    /// <summary>
    /// Gets the <see cref="WeaponSize"/> of this base item if the item is a weapon.
    /// </summary>
    public BaseItemWeaponSize WeaponSize => (BaseItemWeaponSize)BaseItemInfo.m_nWeaponSize;

    /// <summary>
    /// Gets the character feat that provides weapon specialization for this item.
    /// </summary>
    public NwFeat WeaponSpecializationFeat => NwFeat.FromFeatId(BaseItemInfo.m_nWeaponSpecializationFeat);

    /// <summary>
    /// Gets the type/s of damage inflicted by this weapon (item).
    /// </summary>
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

    /// <summary>
    /// Gets the animation set is used when this item is wielded (equipped) in one of the weapon slots.<br/>
    /// May also affect which slots it can work in (eg; double sided weapons take both slots, and small creatures wielding larger weapons do so two handed).
    /// </summary>
    public BaseItemWeaponWieldType WeaponWieldType => (BaseItemWeaponWieldType)BaseItemInfo.m_nWeaponWield;

    /// <summary>
    /// Gets the base weight of this item in pounds.
    /// </summary>
    public decimal Weight => BaseItemInfo.m_nWeight / 10.0m;

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

    /// <summary>
    /// Gets if the specified part is using environment mapping.
    /// </summary>
    /// <param name="partIndex">The part index to query, 0, 1 or 2)</param>
    /// <returns>True if the specified part uses environment mapping, false if it does not and uses transparency instead.</returns>
    public bool IsPartUsingEnvMap(int partIndex)
    {
      return BaseItemInfo.m_bPartEnvMap[partIndex].ToBool();
    }
  }
}
