using System;
using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Core;
using NWNX.API.Constants;

namespace NWN.API
{
  [NativeObjectInfo(ObjectType.Item, InternalObjectType.Item)]
  public sealed class NwItem : NwGameObject
  {
    internal NwItem(uint objectId) : base(objectId) {}

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
    /// Gets or sets the amount of stacked items.
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
      get => (BaseItemType) NWScript.GetBaseItemType(this);
    }

    /// <summary>
    /// Gets the gp value for this item.
    /// </summary>
    public int GoldValue
    {
      get => NWScript.GetGoldPieceValue(this);
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
      => NWScript.AddItemProperty((int) durationType, itemProperty, this, (float) duration.TotalSeconds);

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
    /// <param name="newTag">A new tag for the item, otherwise the value set in the blueprint.</param>
    /// <returns>The created item.</returns>
    public static NwItem Create(string template, Location location, bool useAppearAnim = false, string newTag = null)
    {
      return NwObject.CreateInternal<NwItem>(template, location, useAppearAnim, newTag);
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
    /// <returns>The newly cloned copy of the item.</returns>
    public NwItem Clone(NwGameObject targetInventory, string newTag = null)
    {
      return NWScript.CopyObject(this, targetInventory.Location, targetInventory, newTag).ToNwObject<NwItem>();
    }

    /// <summary>
    /// Creates a copy of this item.
    /// </summary>
    /// <param name="location">The location to create the cloned item.</param>
    /// <param name="newTag">A new tag to assign the cloned item.</param>
    /// <returns>The newly cloned copy of the item.</returns>
    public NwItem Clone(Location location, string newTag = null)
    {
      return NWScript.CopyObject(this, location, INVALID, newTag).ToNwObject<NwItem>();
    }

    public static NwItem GetSpellCastItem() => NWScript.GetSpellCastItem().ToNwObject<NwItem>();

    /// <summary>
    /// Gets or sets a value indicating whether this provided item should be hidden when equipped.
    /// </summary>
    public int HiddenWhenEquipped
    {
      get => NWScript.GetHiddenWhenEquipped(this);
      set => NWScript.SetHiddenWhenEquipped(this, value);
    }
  }
}
