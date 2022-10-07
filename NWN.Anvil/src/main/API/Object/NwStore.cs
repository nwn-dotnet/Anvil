using System;
using System.Collections.Generic;
using Anvil.Native;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A store object for bartering, and items to purchase.
  /// </summary>
  [NativeObjectInfo(ObjectTypes.Store, ObjectType.Store)]
  public sealed partial class NwStore : NwGameObject
  {
    private readonly CNWSStore store;

    internal CNWSStore Store
    {
      get
      {
        AssertObjectValid();
        return store;
      }
    }

    internal NwStore(CNWSStore store) : base(store)
    {
      this.store = store;
    }

    /// <summary>
    /// Gets or sets if this store purchases stolen goods.
    /// </summary>
    public bool BuyStolenGoods
    {
      get => Store.m_bBlackMarket.ToBool();
      set => Store.m_bBlackMarket = value.ToInt();
    }

    /// <summary>
    /// Gets the current customers of this store.
    /// </summary>
    public IReadOnlyList<NwCreature> CurrentCustomers
    {
      get
      {
        List<NwCreature> customers = new List<NwCreature>();
        CExoArrayListCStoreCustomerPtr customersPtr = Store.m_aCurrentCustomers;

        foreach (CStoreCustomer storeCustomer in customersPtr)
        {
          NwCreature? customer = storeCustomer.m_oidObject.ToNwObjectSafe<NwCreature>();
          if (customer != null)
          {
            customers.Add(customer);
          }
        }

        return customers.AsReadOnly();
      }
    }

    /// <summary>
    /// Gets the number of current customers using this store.
    /// </summary>
    public int CustomerCount => Store.m_aCurrentCustomers.Count;

    /// <summary>
    /// Gets or sets the amount this store charges to identify an item.<br/>
    /// Returns -1 if the store does not identify items.
    /// </summary>
    public int IdentifyCost
    {
      get => Store.m_iIdentifyCost;
      set => Store.m_iIdentifyCost = value;
    }

    /// <summary>
    /// Gets all items belonging to this store's inventory.
    /// </summary>
    public IEnumerable<NwItem> Items
    {
      get
      {
        for (uint item = NWScript.GetFirstItemInInventory(this); item != Invalid; item = NWScript.GetNextItemInInventory(this))
        {
          yield return item.ToNwObject<NwItem>()!;
        }
      }
    }

    /// <summary>
    /// Gets or sets the base markdown price for items sold to this store.
    /// </summary>
    public int MarkDown
    {
      get => Store.m_nMarkDown;
      set => Store.m_nMarkDown = value;
    }

    /// <summary>
    /// Gets or sets the base markdown price for stolen items sold to this store.
    /// </summary>
    public int MarkDownStolen
    {
      get => Store.m_nBlackMarketMarkDown;
      set => Store.m_nBlackMarketMarkDown = value;
    }

    /// <summary>
    /// Gets or sets the base markup price for items in the store's inventory.
    /// </summary>
    public int MarkUp
    {
      get => Store.m_nMarkUp;
      set => Store.m_nMarkUp = value;
    }

    /// <summary>
    /// Gets or sets the maximum price this store will pay for an item.<br/>
    /// Returns -1 if the store has no limit.
    /// </summary>
    public int MaxBuyPrice
    {
      get => Store.m_iMaxBuyPrice;
      set => Store.m_iMaxBuyPrice = value;
    }

    public int StoreGold
    {
      get => Store.m_iGold;
      set => Store.m_iGold = value;
    }

    /// <summary>
    /// Gets the list of base item types that this store will not buy.<br/>
    /// Has precedence over <see cref="WillOnlyBuyItems"/>.
    /// </summary>
    public IList<NwBaseItem?> WillNotBuyItems
    {
      get
      {
        return new ListWrapper<int, NwBaseItem?>(Store.m_lstWillNotBuy, NwBaseItem.FromItemId, item => (int)(item?.Id ?? 0));
      }
    }

    /// <summary>
    /// Gets the list of base item types that this store will only buy.<br/>
    /// Does nothing if <see cref="WillNotBuyItems"/> is populated.
    /// </summary>
    public IList<NwBaseItem?> WillOnlyBuyItems
    {
      get
      {
        return new ListWrapper<int, NwBaseItem?>(Store.m_lstWillOnlyBuy, NwBaseItem.FromItemId, item => (int)(item?.Id ?? 0));
      }
    }

    public static NwStore? Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return CreateInternal<NwStore>(template, location, useAppearAnim, newTag);
    }

    public static NwStore? Deserialize(byte[] serialized)
    {
      CNWSStore? store = null;

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTM"))
        {
          return false;
        }

        store = new CNWSStore(Invalid);
        if (store.LoadStore(resGff, resStruct, false.ToInt(), null).ToBool())
        {
          store.LoadObjectState(resGff, resStruct);
          store.m_oidArea = Invalid;
          GC.SuppressFinalize(store);
          return true;
        }

        store.Dispose();
        return false;
      });

      return result && store != null ? store.ToNwObject<NwStore>() : null;
    }

    public static implicit operator CNWSStore?(NwStore? store)
    {
      return store?.Store;
    }

    /// <summary>
    /// Adds the specified item to this store's inventory.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void AcquireItem(NwItem item)
    {
      Store.AcquireItem(item.Item, true.ToInt(), 0xFF, 0xFF);
    }

    public override NwStore Clone(Location location, string? newTag = null, bool copyLocalState = true)
    {
      return CloneInternal<NwStore>(location, newTag, copyLocalState);
    }

    /// <summary>
    /// Open oStore for oPC.<br/>
    /// You can mark up or down the prices with the optional parameters.<br/>
    /// </summary>
    /// <param name="player">The player to present the store.</param>
    /// <param name="bonusMarkup">A number in percent to mark up prices. (Default: 0).</param>
    /// <param name="bonusMarkDown">A number in percent to mark down prices. (Default: 0).</param>
    /// <remarks>If bonusMarkup is given a value of 10, prices will be 110% of the normal prices.</remarks>
    public void Open(NwPlayer player, int bonusMarkup = 0, int bonusMarkDown = 0)
    {
      NWScript.OpenStore(this, player.ControlledCreature, bonusMarkup, bonusMarkDown);
    }

    public override byte[]? Serialize()
    {
      return NativeUtils.SerializeGff("UTM", (resGff, resStruct) =>
      {
        Store.SaveObjectState(resGff, resStruct);
        return Store.SaveStore(resGff, resStruct, 0).ToBool();
      });
    }

    internal override void RemoveFromArea()
    {
      Store.RemoveFromArea();
    }

    private protected override void AddToArea(CNWSArea area, float x, float y, float z)
    {
      Store.AddToArea(area, x, y, z, true.ToInt());
    }
  }
}
