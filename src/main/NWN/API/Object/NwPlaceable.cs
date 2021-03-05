using System;
using System.Numerics;
using System.Threading.Tasks;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Placeable, ObjectType.Placeable)]
  public sealed class NwPlaceable : NwStationary
  {
    internal readonly CNWSPlaceable Placeable;

    internal NwPlaceable(uint objectId, CNWSPlaceable placeable) : base(objectId, placeable)
    {
      this.Placeable = placeable;
      this.Inventory = new Inventory(this, placeable.m_pcItemRepository);
    }

    public static implicit operator CNWSPlaceable(NwPlaceable placeable)
    {
      return placeable?.Placeable;
    }

    public event Action<PlaceableEvents.OnClose> OnClose
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnDamaged> OnDamaged
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnDeath> OnDeath
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnDisarm> OnDisarm
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnHeartbeat> OnHeartbeat
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnDisturbed> OnDisturbed
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnLock> OnLock
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnPhysicalAttacked> OnPhysicalAttacked
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnOpen> OnOpen
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnSpellCastAt> OnSpellCastAt
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnTrapTriggered> OnTrapTriggered
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnUnlock> OnUnlock
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnUsed> OnUsed
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnUserDefined> OnUserDefined
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnDialogue> OnDialogue
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public event Action<PlaceableEvents.OnLeftClick> OnLeftClick
    {
      add => NativeEventService.Subscribe(this, value);
      remove => NativeEventService.Unsubscribe(this, value);
    }

    public override Location Location
    {
      set
      {
        Placeable.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z);
        Rotation = value.Rotation;
      }
    }

    public override float Rotation
    {
      get => (360 - NWScript.GetFacing(this)) % 360;
      set
      {
        float radians = (360 - value % 360) * NwMath.DegToRad;
        Vector3 orientation = new Vector3(MathF.Cos(radians), MathF.Sin(radians), 0.0f);
        Placeable.SetOrientation(orientation.ToNativeVector());
      }
    }

    public override bool KeyAutoRemoved
    {
      get => Placeable.m_bAutoRemoveKey.ToBool();
      set => Placeable.m_bAutoRemoveKey = value.ToInt();
    }

    public bool Occupied => NWScript.GetSittingCreature(this) != INVALID;

    public NwCreature SittingCreature => NWScript.GetSittingCreature(this).ToNwObject<NwCreature>();

    /// <summary>
    /// Gets the inventory of this placeable.
    /// </summary>
    public Inventory Inventory { get; }

    /// <summary>
    /// Gets or sets a value indicating whether this placeable should illuminate.
    /// </summary>
    public bool Illumination
    {
      get => NWScript.GetPlaceableIllumination(this).ToBool();
      set => NWScript.SetPlaceableIllumination(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether this placeable should be useable (clickable).
    /// </summary>
    public bool Useable
    {
      get => NWScript.GetUseableFlag(this).ToBool();
      set => NWScript.SetUseableFlag(this, value.ToInt());
    }

    /// <summary>
    /// Gets or sets a value indicating whether this placeable has an inventory.
    /// </summary>
    public bool HasInventory
    {
      get => NWScript.GetHasInventory(this).ToBool();
      set => Placeable.m_bHasInventory = value.ToInt();
    }

    /// <summary>
    /// Moves the specified item/item stack to this placeable's inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public async Task GiveItem(NwItem item)
    {
      NwObject assignTarget;
      if (item.Possessor != null)
      {
        assignTarget = item.Possessor;
      }
      else
      {
        assignTarget = item.Area;
      }

      if (assignTarget != this)
      {
        await assignTarget.WaitForObjectContext();
        NWScript.ActionGiveItem(item, this);
      }
    }

    /// <summary>
    /// Moves a specified amount of items from an item stack to this placeable's inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="amount">The number of items from the item stack to take.</param>
    public async Task GiveItem(NwItem item, int amount)
    {
      if (amount > item.StackSize)
      {
        amount = item.StackSize;
      }

      if (amount == item.StackSize)
      {
        await GiveItem(item);
        return;
      }

      NwItem clone = item.Clone(this);
      clone.StackSize = amount;
      item.StackSize -= amount;
    }

    public static NwPlaceable Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      location = Location.Create(location.Area, location.Position, location.FlippedRotation);
      return NwObject.CreateInternal<NwPlaceable>(template, location, useAppearAnim, newTag);
    }

    /// <summary>
    /// Determines whether the specified action can be performed on this placeable.
    /// </summary>
    /// <param name="action">The action to check.</param>
    /// <returns>true if the specified action can be performed, otherwise false.</returns>
    public bool IsPlaceableActionPossible(PlaceableAction action)
      => NWScript.GetIsPlaceableObjectActionPossible(this, (int)action).ToBool();

    public unsafe void AcquireItem(NwItem item, bool displayFeedback = true)
    {
      if (item == null)
      {
        throw new ArgumentNullException(nameof(item), "Item cannot be null.");
      }

      void* pItem = item.Item;
      Placeable.AcquireItem(&pItem, INVALID, 0xFF, 0xFF, displayFeedback.ToInt());
    }
  }
}
