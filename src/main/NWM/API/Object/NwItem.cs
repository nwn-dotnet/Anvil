using System;
using System.Collections.Generic;
using NWM.API.Constants;
using NWMX.API.Constants;
using NWN;
using ItemProperty = NWN.ItemProperty;

namespace NWM.API
{
  [NativeObjectInfo(ObjectType.Item, InternalObjectType.Item)]
  public sealed class NwItem : NwGameObject
  {
    internal NwItem(uint objectId) : base(objectId) {}

    public string OriginalUnidentifiedDescription => NWScript.GetDescription(this, true.ToInt(), false.ToInt());

    public string UnidentifiedDescription
    {
      get => NWScript.GetDescription(this, false.ToInt(), false.ToInt());
      set => NWScript.SetDescription(this, value, false.ToInt());
    }

    public int StackSize
    {
      get => NWScript.GetItemStackSize(this);
      set => NWScript.SetItemStackSize(this, value);
    }

    public bool Identified
    {
      get => NWScript.GetIdentified(this).ToBool();
      set => NWScript.SetIdentified(this, value.ToInt());
    }

    /// <summary>
    /// The GameObject that has this item in its inventory, otherwise null if it is on the ground, or not in any inventory.
    /// </summary>
    public NwGameObject Possessor
    {
      get => NWScript.GetItemPossessor(this).ToNwObject<NwGameObject>();
    }

    public BaseItemType BaseItemType
    {
      get => (BaseItemType) NWScript.GetBaseItemType(this);
    }

    public int GoldValue
    {
      get => NWScript.GetGoldPieceValue(this);
    }

    public IEnumerable<ItemProperty> ItemProperties
    {
      get
      {
        for (ItemProperty itemProperty = NWScript.GetFirstItemProperty(this); itemProperty.IsValid(); itemProperty = NWScript.GetNextItemProperty(this))
        {
          yield return itemProperty;
        }
      }
    }

    public void AddItemProperty(ItemProperty itemProperty,
      float duration = 0.0f,
      AddPropertyPolicy addPolicy = AddPropertyPolicy.ReplaceExisting,
      bool ignoreDurationType = false,
      bool ignoreSubType = false)
    {
      throw new NotImplementedException();
    }

    public static NwItem Create(string template, Location location, bool useAppearAnim = false, string newTag = null)
    {
      return NwObjectFactory.CreateInternal<NwItem>(template, location, useAppearAnim, newTag);
    }

    public static NwItem Create(string template, NwGameObject target = null, int stackSize = 1, string newTag = null)
    {
      return NWScript.CreateItemOnObject(template, target, stackSize, newTag).ToNwObject<NwItem>();
    }

    public NwItem Clone(NwGameObject targetInventory, string newTag = null)
    {
      return NWScript.CopyObject(this, targetInventory.Location, targetInventory, newTag).ToNwObject<NwItem>();
    }

    public NwItem Clone(Location location, string newTag = null)
    {
      return NWScript.CopyObject(this, location, INVALID, newTag).ToNwObject<NwItem>();
    }

    public static NwItem GetSpellCastItem() => NWScript.GetSpellCastItem().ToNwObject<NwItem>();
  }
}