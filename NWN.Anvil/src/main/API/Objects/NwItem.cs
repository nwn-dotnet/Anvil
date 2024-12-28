using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anvil.Native;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// An item object that can be carried by creatures, stored in placeable containers, or dropped in an anrea.
  /// </summary>
  [ObjectType(ObjectTypes.Item)]
  [ObjectFilter(ObjectTypes.Item)]
  public sealed partial class NwItem : NwGameObject
  {
    private readonly CNWSItem item;

    internal CNWSItem Item
    {
      get
      {
        AssertObjectValid();
        return item;
      }
    }

    internal NwItem(CNWSItem item) : base(item)
    {
      this.item = item;
      Inventory = new Inventory(this, item.m_pItemRepository);
      Appearance = new ItemAppearance(this);
    }

    /// <summary>
    /// Gets the armor class of this item.
    /// <remarks>
    /// This will return the full AC value of this item, taking into account all modifiers in regards to bonus AC.<br/>
    /// Unlike the standard ruleset, it will stack multiple AC bonuses instead of taking the highest.<br/>
    /// This value does not take into account AC bonuses vs certain conditions nor the <see cref="ItemProperty.DecreaseAC"/> Item Property.<br/>
    /// It will also not take into account ability changes, nor if there is an existing amount of that bonus type. For example, wearing +1 armor, thus using a +1 Armor AC modifier, will not stack with Epic Mage Armor, which gives +5 in Armor AC bonuses.
    /// </remarks>
    /// </summary>
    public int ACValue => NWScript.GetItemACValue(this);

    /// <summary>
    /// Gets or sets the additional GP value of this item.<br/>
    /// Does not persist through saving.
    /// </summary>
    public int AddGoldValue
    {
      get => Item.m_nAdditionalCost;
      set => Item.m_nAdditionalCost = value;
    }

    /// <summary>
    /// Gets the appearance properties of this item.
    /// </summary>
    public ItemAppearance Appearance { get; }

    /// <summary>
    /// Gets the base armor class of this item.
    /// </summary>
    public int BaseACValue => Item.m_nArmorValue;

    /// <summary>
    /// Gets or sets the base GP value of this item.<br/>
    /// Does not persist through saving.
    /// </summary>
    public uint BaseGoldValue
    {
      get => Item.m_nBaseUnitCost;
      set => Item.m_nBaseUnitCost = value;
    }

    /// <summary>
    /// Gets or sets the <see cref="NwBaseItem"/> for this item.
    /// </summary>
    public NwBaseItem BaseItem
    {
      get => NwBaseItem.FromItemId((int)Item.m_nBaseItem)!;
      set => Item.m_nBaseItem = value.Id;
    }

    /// <summary>
    /// Gets or sets a value indicating whether this item is considered cursed. Cursed items cannot be dropped.
    /// </summary>
    public bool CursedFlag
    {
      get => NWScript.GetItemCursedFlag(this).ToBool();
      set => NWScript.SetItemCursedFlag(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether this item can be dropped.
    /// <remarks>Droppable items will appear on a creature's remains when the creature is killed.</remarks>
    /// </summary>
    public bool Droppable
    {
      get => NWScript.GetDroppableFlag(this).ToBool();
      set => NWScript.SetDroppableFlag(this, value.ToInt());
    }

    /// <summary>
    /// Gets the gp value for this item.
    /// </summary>
    public int GoldValue => NWScript.GetGoldPieceValue(this);

    /// <summary>
    /// Gets a value indicating whether this item has an inventory (container).
    /// </summary>
    public bool HasInventory => NWScript.GetHasInventory(this).ToBool();

    /// <summary>
    /// Gets or sets a value indicating whether this item should be hidden when equipped.
    /// </summary>
    public int HiddenWhenEquipped
    {
      get => NWScript.GetHiddenWhenEquipped(this);
      set => NWScript.SetHiddenWhenEquipped(this, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this item has been identified.
    /// </summary>
    public bool Identified
    {
      get => NWScript.GetIdentified(this).ToBool();
      set => NWScript.SetIdentified(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether this item may be infinitely purchased from stores.<br/>
    /// An infinite item will still be available to purchase from a store after a player buys the item.
    /// </summary>
    public bool Infinite
    {
      get => NWScript.GetInfiniteFlag(this).ToBool();
      set => NWScript.SetInfiniteFlag(this, value.ToInt());
    }

    /// <summary>
    /// Gets the inventory of this item, if it is a container.
    /// </summary>
    public Inventory Inventory { get; }

    /// <summary>
    /// Gets if this item is considered a ranged weapon.
    /// </summary>
    public bool IsRangedWeapon => NWScript.GetWeaponRanged(this).ToBool();

    /// <summary>
    /// Gets or sets the number of charges left on this item.
    /// </summary>
    public int ItemCharges
    {
      get => NWScript.GetItemCharges(this);
      set => NWScript.SetItemCharges(this, value);
    }

    /// <summary>
    /// Gets all active item properties currently applied to this object.
    /// </summary>
    public IEnumerable<ItemProperty> ItemProperties
    {
      get
      {
        for (ItemProperty? itemProperty = NWScript.GetFirstItemProperty(this); itemProperty != null && itemProperty.Valid; itemProperty = NWScript.GetNextItemProperty(this))
        {
          yield return itemProperty;
        }
      }
    }

    /// <summary>
    /// Gets the minimum level required to equip this item.
    /// </summary>
    public byte MinEquipLevel => Item.GetMinEquipLevel();

    /// <summary>
    /// Gets the original unidentified description for this item.
    /// </summary>
    public string OriginalUnidentifiedDescription => NWScript.GetDescription(this, true.ToInt(), false.ToInt());

    /// <summary>
    /// Gets or sets a value indicating whether this item can be pickpocketed.
    /// </summary>
    public bool Pickpocketable
    {
      get => NWScript.GetPickpocketableFlag(this).ToBool();
      set => NWScript.SetPickpocketableFlag(this, value.ToInt());
    }

    /// <summary>
    /// Gets the GameObject that has this item in its inventory. Returns null if it is on the ground, or not in any inventory.
    /// </summary>
    /// <remarks>This can be a creature, a placeable, or an item container (e.g magic bag). Use <see cref="RootPossessor"/> to get the root possessor of this item.</remarks>
    public NwGameObject? Possessor => NWScript.GetItemPossessor(this, true.ToInt()).ToNwObject<NwGameObject>();

    /// <summary>
    /// Gets or sets the number of stacked items attached to this item.
    /// </summary>
    public int StackSize
    {
      get => NWScript.GetItemStackSize(this);
      set => NWScript.SetItemStackSize(this, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this item is considered stolen. Only stores with the "Buys Stolen Goods" will purchase this item.
    /// <remarks>The stolen flag is set automatically on pickpocketed items, and traps crafted with the craft trap skill.</remarks>
    /// </summary>
    public bool Stolen
    {
      get => NWScript.GetStolenFlag(this).ToBool();
      set => NWScript.SetStolenFlag(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets the unidentified description for this item.
    /// </summary>
    public string UnidentifiedDescription
    {
      get => NWScript.GetDescription(this, false.ToInt(), false.ToInt());
      set => NWScript.SetDescription(this, value, false.ToInt());
    }

    /// <summary>
    /// Gets the root possessor of this item.
    /// </summary>
    /// <remarks>If this item is in a container, this is the creature/placeable holding the container that holds this item. Otherwise, this returns the same object as <see cref="Possessor"/>.</remarks>
    public NwGameObject? RootPossessor => NWScript.GetItemPossessor(this, false.ToInt()).ToNwObject<NwGameObject>();

    /// <summary>
    /// Gets or sets the weight of this item, in pounds.
    /// </summary>
    public decimal Weight
    {
      get => NWScript.GetWeight(this) / 10.0m;
      set
      {
        Item.m_nWeight = (int)Math.Round(value * 10.0m, MidpointRounding.ToZero);
        if (RootPossessor is NwCreature creature)
        {
          creature.Creature.m_nEquippedWeight = creature.Creature.ComputeTotalEquippedWeight();
          creature.Creature.m_nTotalWeightCarried = creature.Creature.ComputeTotalWeightCarried();
          creature.Creature.UpdateEncumbranceState();
        }
      }
    }

    /// <summary>
    /// Creates a new item from a template ResRef.
    /// </summary>
    /// <param name="template">The item template (.uti) to use.</param>
    /// <param name="location">The world location for the created item.</param>
    /// <param name="useAppearAnim">If true, whether an appear animation should play for the item.</param>
    /// <param name="stackSize">The stack size of the created item.</param>
    /// <param name="newTag">A new tag for the item, otherwise the value set in the blueprint.</param>
    /// <returns>The created item.</returns>
    public static NwItem? Create(string template, Location location, bool useAppearAnim = false, int stackSize = 1, string? newTag = null)
    {
      NwItem? item = CreateInternal<NwItem>(template, location, useAppearAnim, newTag);
      if (item == null)
      {
        return null;
      }

      item.StackSize = stackSize;
      return item;
    }

    /// <summary>
    /// Creates a new item from a template ResRef.
    /// </summary>
    /// <param name="template">The item template (.uti) to use.</param>
    /// <param name="target">The target inventory for the created item.</param>
    /// <param name="stackSize">The stack size of the created item.</param>
    /// <param name="newTag">A new tag for the item, otherwise the value set in the blueprint.</param>
    /// <returns>The created item.</returns>
    public static async Task<NwItem?> Create(string template, NwGameObject? target = null, int stackSize = 1, string newTag = "")
    {
      await NwModule.Instance.WaitForObjectContext();
      return NWScript.CreateItemOnObject(template, target, stackSize, newTag).ToNwObject<NwItem>();
    }

    public static NwItem? Deserialize(byte[] serialized)
    {
      CNWSItem? item = null;

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTI"))
        {
          return false;
        }

        item = new CNWSItem(Invalid);
        if (item.LoadItem(resGff, resStruct, false.ToInt()).ToBool())
        {
          item.m_oidArea = Invalid;
          GC.SuppressFinalize(item);
          return true;
        }

        item.Dispose();
        return false;
      });

      return result && item != null ? item.ToNwObject<NwItem>() : null;
    }

    public static implicit operator CNWSItem?(NwItem? item)
    {
      return item?.Item;
    }

    public unsafe void AcquireItem(NwItem item, bool displayFeedback = true)
    {
      if (item == null)
      {
        throw new ArgumentNullException(nameof(item), "Item cannot be null.");
      }

      void* itemPtr = item.Item;
      Item.AcquireItem(&itemPtr, Invalid, 0xFF, 0xFF, displayFeedback.ToInt());
    }

    /// <summary>
    /// Add an item property. Optional parameters allow for preventing unwanted stacking by removing the existing one first.
    /// </summary>
    /// <param name="itemProperty">The item property to add.</param>
    /// <param name="durationType">(Permanent/Temporary) - the duration of this item property.</param>
    /// <param name="duration">If DurationType is temporary, how long this item property should stay applied.</param>
    /// <param name="policy">The policy to use when adding this item property.</param>
    /// <param name="ignoreDuration">If set to true, an item property will be considered identical even if the DurationType is different. Be careful when using this with <see cref="AddPropPolicy.ReplaceExisting"/>, as this could lead to a temporary item property removing a permanent one</param>
    /// <param name="ignoreSubType">If set to true an item property will be considered identical even if the SubType is different.</param>
    /// <param name="ignoreTag">If set to true an item property will be considered identical even if the tag is different.</param>
    public void AddItemProperty(ItemProperty itemProperty, EffectDuration durationType, TimeSpan duration = default, AddPropPolicy policy = AddPropPolicy.IgnoreExisting, bool ignoreDuration = false, bool ignoreSubType = false, bool ignoreTag = false)
    {
      switch (policy)
      {
        case AddPropPolicy.IgnoreExisting:
          break;
        case AddPropPolicy.ReplaceExisting:
          RemoveItemProperties(itemProperty.Property, ignoreSubType ? null : itemProperty.SubType, ignoreDuration ? null : durationType, ignoreTag ? null : itemProperty.Tag);
          break;
        case AddPropPolicy.KeepExisting:
          if (HasItemProperty(itemProperty.Property, ignoreSubType ? null : itemProperty.SubType, ignoreDuration ? null : durationType, ignoreTag ? null : itemProperty.Tag))
          {
            return;
          }

          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(policy), policy, null);
      }

      NWScript.AddItemProperty((int)durationType, itemProperty, this, (float)duration.TotalSeconds);
    }

    /// <summary>
    /// Creates a copy of this item.
    /// </summary>
    /// <param name="targetInventory">The target inventory to create the cloned item.</param>
    /// <param name="newTag">A new tag to assign the cloned item.</param>
    /// <param name="copyLocalState">If true, local variables on the item are copied.</param>
    /// <param name="preserveDropFlag">If true, preserves the <see cref="Droppable"/> state of the item.</param>
    /// <returns>The newly cloned copy of the item.</returns>
    public NwItem Clone(NwGameObject targetInventory, string? newTag = null, bool copyLocalState = true, bool preserveDropFlag = true)
    {
      NwItem clone = NWScript.CopyItem(this, targetInventory, copyLocalState.ToInt()).ToNwObject<NwItem>()!;
      if (newTag != null)
      {
        clone.Tag = newTag;
      }

      if (preserveDropFlag)
      {
        clone.Droppable = Droppable;
      }

      return clone;
    }

    public override NwItem Clone(Location location, string? newTag = null, bool copyLocalState = true)
    {
      return Clone(location, true, newTag, copyLocalState);
    }

    /// <summary>
    /// Creates a copy of this item.
    /// </summary>
    /// <param name="location">The location for the copied item.</param>
    /// <param name="preserveDropFlag">If true, preserves the <see cref="Droppable"/> state of the item.</param>
    /// <param name="newTag">A new tag to assign the cloned item.</param>
    /// <param name="copyLocalState">If true, local variables on the item are copied.</param>
    /// <returns>The newly cloned copy of the item.</returns>
    public NwItem Clone(Location location, bool preserveDropFlag, string? newTag = null, bool copyLocalState = true)
    {
      NwItem clone = CloneInternal<NwItem>(location, newTag, copyLocalState);
      if (!copyLocalState)
      {
        CleanLocalVariables(clone);
      }

      if (preserveDropFlag)
      {
        clone.Droppable = Droppable;
      }

      return clone;
    }

    /// <summary>
    /// Compare this item to another item to test if it is stackable.
    /// </summary>
    /// <param name="otherItem">The other item to compare to.</param>
    /// <returns>True if both items can stack with each-other, otherwise false.</returns>
    public bool CompareItem(NwItem otherItem)
    {
      return Item.CompareItem(otherItem.Item).ToBool();
    }

    /// <summary>
    /// Gets the number of uses per day remaining for the specified item property on this item.
    /// </summary>
    /// <param name="property">The item property to test for uses remaining.</param>
    /// <returns>The number of uses per day remaining for the specified item property, or 0 if this item property is not uses/day, or belongs to a different item.</returns>
    public int GetUsesPerDayRemaining(ItemProperty property)
    {
      return NWScript.GetItemPropertyUsesPerDayRemaining(this, property);
    }

    /// <summary>
    /// Gets whether this item has a given item property.
    /// </summary>
    /// <param name="property">Item property to check.</param>
    /// <returns>True if this item has a property of the given type, otherwise false.</returns>
    public bool HasItemProperty(ItemPropertyType property)
    {
      return NWScript.GetItemHasItemProperty(this, (int)property).ToBool();
    }

    /// <summary>
    /// Gets whether this item has a given item property that matches the specified filters.<br/>
    /// If no filters are set, returns if any item property is set on this item.
    /// </summary>
    /// <param name="propertyType">If set, restricts the search of item properties to the specified type.</param>
    /// <param name="subType">If set, restricts the search of item properties to the specified sub-type.</param>
    /// <param name="durationType">If set, restricts the search of item properties to the specified duration type.</param>
    /// <param name="tag">If set, restricts the search of item properties to the specified tag.</param>
    /// <returns>True if this item has a property matching the specified filters, otherwise false.</returns>
    public bool HasItemProperty(ItemPropertyTableEntry? propertyType = null, ItemPropertySubTypeTableEntry? subType = null, EffectDuration? durationType = null, string? tag = null)
    {
      foreach (ItemProperty property in ItemProperties)
      {
        if ((propertyType == null || propertyType == property.Property) &&
          (subType == null || subType == property.SubType) &&
          (durationType == null || durationType == property.DurationType) &&
          (tag == null || tag == property.Tag))
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Removes the specified item property from this item.<br/>
    /// See <see cref="ItemProperties"/> to enumerate item properties on this item.
    /// </summary>
    /// <param name="itemProperty">The item property to remove.</param>
    public void RemoveItemProperty(ItemProperty itemProperty)
    {
      NWScript.RemoveItemProperty(this, itemProperty);
    }

    /// <summary>
    /// Remove all item properties from this item, using the specified filter options.<br/>
    /// If no filters are set, removes all item properties from this item.
    /// </summary>
    /// <param name="propertyType">If set, restricts the deletion of item properties to the specified type.</param>
    /// <param name="subType">If set, restricts the deletion of item properties to the specified sub-type.</param>
    /// <param name="durationType">If set, restricts the deletion of item properties to the specified duration type.</param>
    /// <param name="tag">If set, restricts the deletion of item properties to the specified tag.</param>
    public void RemoveItemProperties(ItemPropertyTableEntry? propertyType = null, ItemPropertySubTypeTableEntry? subType = null, EffectDuration? durationType = null, string? tag = null)
    {
      foreach (ItemProperty property in ItemProperties)
      {
        if ((propertyType == null || propertyType == property.Property) &&
          (subType == null || subType == property.SubType) &&
          (durationType == null || durationType == property.DurationType) &&
          (tag == null || tag == property.Tag))
        {
          RemoveItemProperty(property);
        }
      }
    }

    public override byte[]? Serialize()
    {
      return NativeUtils.SerializeGff("UTI", (resGff, resStruct) => Item.SaveItem(resGff, resStruct, 0).ToBool());
    }

    /// <summary>
    /// Sets the number of uses per day remaining for the specified item property on this item.
    /// </summary>
    /// <param name="property">The item property to be adjusted.</param>
    /// <param name="numUses">The new number of uses per day remaining for the specified item property.</param>
    public void SetUsesPerDayRemaining(ItemProperty property, int numUses)
    {
      NWScript.SetItemPropertyUsesPerDayRemaining(this, property, numUses);
    }

    internal override void RemoveFromArea()
    {
      if (Possessor is NwCreature creature)
      {
        creature.Creature.RemoveItem(this, true.ToInt(), true.ToInt(), false.ToInt(), true.ToInt());
      }
      else if (Possessor is NwPlaceable placeable)
      {
        placeable.Placeable.RemoveItem(this, true.ToInt());
      }
      else if (Possessor is NwStore store)
      {
        store.Store.RemoveItem(this);
      }
      else
      {
        Item.RemoveFromArea();
      }
    }

    private static void CleanLocalVariables(NwItem? clone)
    {
      if (clone == null)
      {
        return;
      }

      List<ObjectVariable> localVariables = clone.LocalVariables.ToList();
      foreach (ObjectVariable localVariable in localVariables)
      {
        localVariable.Delete();
      }
    }

    private protected override void AddToArea(CNWSArea area, float x, float y, float z)
    {
      Item.AddToArea(area, x, y, z, true.ToInt());
    }
  }
}
