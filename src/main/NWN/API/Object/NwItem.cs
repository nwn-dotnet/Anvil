﻿using System;
using System.Collections.Generic;
using System.Linq;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Item, ObjectType.Item)]
  public sealed class NwItem : NwGameObject
  {
    internal readonly CNWSItem Item;

    /// <summary>
    /// Gets the inventory of this item, if it is a container.
    /// </summary>
    public Inventory Inventory { get; }

    /// <summary>
    /// Gets the appearance properties of this item.
    /// </summary>
    public ItemAppearance Appearance { get; }

    internal NwItem(CNWSItem item) : base(item)
    {
      this.Item = item;
      this.Inventory = new Inventory(this, item.m_pItemRepository);
      this.Appearance = new ItemAppearance(item);
    }

    public static implicit operator CNWSItem(NwItem item)
    {
      return item?.Item;
    }

    public override Location Location
    {
      set
      {
        Item.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z, true.ToInt());
        Rotation = value.Rotation;
      }
    }

    /// <summary>
    /// Gets the original unidentified description for this item.
    /// </summary>
    public string OriginalUnidentifiedDescription
    {
      get => NWScript.GetDescription(this, true.ToInt(), false.ToInt());
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
    /// Gets or sets the number of stacked items attached to this item.
    /// </summary>
    public int StackSize
    {
      get => NWScript.GetItemStackSize(this);
      set => NWScript.SetItemStackSize(this, value);
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
    /// Gets or sets a value indicating whether this item is considered stolen. Only stores with the "Buys Stolen Goods" will purchase this item.
    /// <remarks>The stolen flag is set automatically on pickpocketed items, and traps crafted with the craft trap skill.</remarks>
    /// </summary>
    public bool Stolen
    {
      get => NWScript.GetStolenFlag(this).ToBool();
      set => NWScript.SetStolenFlag(this, value.ToInt());
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
    /// Gets or sets a value indicating whether this item is considered cursed. Cursed items cannot be dropped.
    /// </summary>
    public bool CursedFlag
    {
      get => NWScript.GetItemCursedFlag(this).ToBool();
      set => NWScript.SetItemCursedFlag(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether this item can be pickpocketed.
    /// </summary>
    public bool Pickpocketable
    {
      get => NWScript.GetPickpocketableFlag(this).ToBool();
      set => NWScript.SetPickpocketableFlag(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets the number of charges left on this item.
    /// </summary>
    public int ItemCharges
    {
      get => NWScript.GetItemCharges(this);
      set => NWScript.SetItemCharges(this, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether this item should be hidden when equipped.
    /// </summary>
    public int HiddenWhenEquipped
    {
      get => NWScript.GetHiddenWhenEquipped(this);
      set => NWScript.SetHiddenWhenEquipped(this, value);
    }

    /// <summary>
    /// Gets a value indicating whether this item has an inventory (container).
    /// </summary>
    public bool HasInventory
    {
      get => NWScript.GetHasInventory(this).ToBool();
    }

    /// <summary>
    /// Gets the GameObject that has this item in its inventory. Returns null if it is on the ground, or not in any inventory.
    /// </summary>
    public NwGameObject Possessor
    {
      get => NWScript.GetItemPossessor(this).ToNwObject<NwGameObject>();
    }

    /// <summary>
    /// Gets the <see cref="NWN.API.Constants.BaseItemType"/> for this item.
    /// </summary>
    public BaseItemType BaseItemType
    {
      get => (BaseItemType)NWScript.GetBaseItemType(this);
    }

    /// <summary>
    /// Gets the gp value for this item.
    /// </summary>
    public int GoldValue
    {
      get => NWScript.GetGoldPieceValue(this);
    }

    /// <summary>
    /// Gets or sets the weight of this item, in pounds.
    /// </summary>
    public decimal Weight
    {
      get => NWScript.GetWeight(this) / 10.0m;
      set
      {
        Item.m_nWeight = (int)Math.Round(value * 10.0m, MidpointRounding.ToZero);
        Item.m_oidPossessor.ToNwObject<NwCreature>()?.Creature.UpdateEncumbranceState();
      }
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
    public int ACValue
    {
      get => NWScript.GetItemACValue(this);
    }

    /// <summary>
    /// Gets the base armor class of this item.
    /// </summary>
    public int BaseACValue
    {
      get => Item.m_nArmorValue;
    }

    /// <summary>
    /// Gets all active item properties currently applied to this object.
    /// </summary>
    public IEnumerable<ItemProperty> ItemProperties
    {
      get
      {
        for (ItemProperty itemProperty = NWScript.GetFirstItemProperty(this); itemProperty.Valid; itemProperty = NWScript.GetNextItemProperty(this))
        {
          yield return itemProperty;
        }
      }
    }

    /// <summary>
    /// Adds the specified item property to this item.
    /// </summary>
    /// <param name="itemProperty">The item property to add.</param>
    /// <param name="durationType">(Permanent/Temporary) - the duration of this item property.</param>
    /// <param name="duration">If DurationType is temporary, how long this item property should stay applied.</param>
    public void AddItemProperty(ItemProperty itemProperty, EffectDuration durationType, TimeSpan duration = default)
      => NWScript.AddItemProperty((int)durationType, itemProperty, this, (float)duration.TotalSeconds);

    /// <summary>
    /// Removes the specified item property from this item.<br/>
    /// See <see cref="ItemProperties"/> to enumerate item properties on this item.
    /// </summary>
    /// <param name="itemProperty">The item property to remove.</param>
    public void RemoveItemProperty(ItemProperty itemProperty)
      => NWScript.RemoveItemProperty(this, itemProperty);

    /// <summary>
    /// Creates a new item from a template ResRef.
    /// </summary>
    /// <param name="template">The item template (.uti) to use.</param>
    /// <param name="location">The world location for the created item.</param>
    /// <param name="useAppearAnim">If true, whether an appear animation should play for the item.</param>
    /// <param name="stackSize">The stack size of the created item.</param>
    /// <param name="newTag">A new tag for the item, otherwise the value set in the blueprint.</param>
    /// <returns>The created item.</returns>
    public static NwItem Create(string template, Location location, bool useAppearAnim = false, int stackSize = 1, string newTag = null)
    {
      NwItem item = NwObject.CreateInternal<NwItem>(template, location, useAppearAnim, newTag);
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
    public static NwItem Create(string template, NwGameObject target = null, int stackSize = 1, string newTag = null)
    {
      return NWScript.CreateItemOnObject(template, target, stackSize, newTag).ToNwObject<NwItem>();
    }

    /// <summary>
    /// Creates a copy of this item.
    /// </summary>
    /// <param name="targetInventory">The target inventory to create the cloned item.</param>
    /// <param name="newTag">A new tag to assign the cloned item.</param>
    /// <param name="copyVars">If true, local variables on the item are copied.</param>
    /// <returns>The newly cloned copy of the item.</returns>
    public NwItem Clone(NwGameObject targetInventory, string newTag = null, bool copyVars = true)
    {
      NwItem clone = NWScript.CopyItem(this, targetInventory, copyVars.ToInt()).ToNwObject<NwItem>();
      if (clone != null && newTag != null)
      {
        clone.Tag = newTag;
      }

      return clone;
    }

    /// <summary>
    /// Creates a copy of this item.
    /// </summary>
    /// <param name="location">The location to create the cloned item.</param>
    /// <param name="newTag">A new tag to assign the cloned item.</param>
    /// <param name="copyVars">If true, local variables on the item are copied.</param>
    /// <returns>The newly cloned copy of the item.</returns>
    public NwItem Clone(Location location, string newTag = null, bool copyVars = true)
    {
      NwItem clone = NWScript.CopyObject(this, location, INVALID, newTag).ToNwObject<NwItem>();
      if (!copyVars)
      {
        CleanLocalVariables(clone);
      }

      return clone;
    }

    private static void CleanLocalVariables(NwItem clone)
    {
      if (clone == null)
      {
        return;
      }

      List<LocalVariable> localVariables = clone.LocalVariables.ToList();
      foreach (LocalVariable localVariable in localVariables)
      {
        localVariable.Delete();
      }
    }

    /// <summary>
    /// Gets whether this item has a given item property.
    /// </summary>
    /// <param name="property">Item property to check.</param>
    /// <returns>True if this item has a property of the given type, otherwise false.</returns>
    public bool HasItemProperty(ItemPropertyType property)
      => NWScript.GetItemHasItemProperty(this, (int)property).ToBool();

    /// <summary>
    /// Gets the number of uses per day remaining for the specified item property on this item.
    /// </summary>
    /// <param name="property">The item property to test for uses remaining.</param>
    /// <returns>The number of uses per day remaining for the specified item property, or 0 if this item property is not uses/day, or belongs to a different item.</returns>
    public int UsesPerDayRemaining(ItemProperty property)
      => NWScript.GetItemPropertyUsesPerDayRemaining(this, property);

    public unsafe void AcquireItem(NwItem item, bool displayFeedback = true)
    {
      if (item == null)
      {
        throw new ArgumentNullException(nameof(item), "Item cannot be null.");
      }

      void* itemPtr = item.Item;
      Item.AcquireItem(&itemPtr, INVALID, 0xFF, 0xFF, displayFeedback.ToInt());
    }

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("UTI", (resGff, resStruct) =>
      {
        return Item.SaveItem(resGff, resStruct, 0).ToBool();
      });
    }

    public static NwItem Deserialize(byte[] serialized)
    {
      CNWSItem item = null;

      NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTI"))
        {
          return false;
        }

        item = new CNWSItem(INVALID);
        if (item.LoadItem(resGff, resStruct).ToBool())
        {
          return true;
        }

        item.Dispose();
        return false;
      });

      return item != null ? item.m_idSelf.ToNwObject<NwItem>() : null;
    }
  }
}
