using System;
using System.Collections.Generic;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  [NativeObjectInfo(ObjectTypes.Store, ObjectType.Store)]
  public sealed partial class NwStore : NwGameObject
  {
    internal readonly CNWSStore Store;

    internal NwStore(CNWSStore store) : base(store)
    {
      Store = store;
    }

    public static implicit operator CNWSStore(NwStore store)
    {
      return store?.Store;
    }

    public static NwStore Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return CreateInternal<NwStore>(template, location, useAppearAnim, newTag);
    }

    public int StoreGold
    {
      get => NWScript.GetStoreGold(this);
      set => NWScript.SetStoreGold(this, value);
    }

    public int IdentifyCost
    {
      get => NWScript.GetStoreIdentifyCost(this);
      set => NWScript.SetStoreIdentifyCost(this, value);
    }

    public int MaxBuyPrice
    {
      get => NWScript.GetStoreMaxBuyPrice(this);
      set => NWScript.SetStoreMaxBuyPrice(this, value);
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
          yield return item.ToNwObject<NwItem>();
        }
      }
    }

    /// <summary>
    /// Gets the current customers of this store.
    /// </summary>
    public unsafe IEnumerable<NwCreature> CurrentCustomers
    {
      get
      {
        List<NwCreature> customers = new List<NwCreature>();
        CExoArrayListCStoreCustomerPtr customersPtr = Store.m_aCurrentCustomers;

        for (int i = 0; i < customersPtr.num; i++)
        {
          void** customerPtr = customersPtr._OpIndex(i);
          NwCreature customer = CStoreCustomer.FromPointer(*customerPtr).m_oidObject.ToNwObjectSafe<NwCreature>();
          if (customer != null)
          {
            customers.Add(customer);
          }
        }

        return customers;
      }
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

    public void AcquireItem(NwItem item)
    {
      if (item == null)
      {
        throw new ArgumentNullException(nameof(item), "Item cannot be null.");
      }

      Store.AcquireItem(item.Item, true.ToInt(), 0xFF, 0xFF);
    }

    public override byte[] Serialize()
    {
      return NativeUtils.SerializeGff("UTM", (resGff, resStruct) =>
      {
        Store.SaveObjectState(resGff, resStruct);
        return Store.SaveStore(resGff, resStruct, 0).ToBool();
      });
    }

    public static NwStore Deserialize(byte[] serialized)
    {
      CNWSStore store = null;

      bool result = NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTM"))
        {
          return false;
        }

        store = new CNWSStore(Invalid);
        if (store.LoadStore(resGff, resStruct, null).ToBool())
        {
          store.LoadObjectState(resGff, resStruct);
          GC.SuppressFinalize(store);
          return true;
        }

        store.Dispose();
        return false;
      });

      return result && store != null ? store.ToNwObject<NwStore>() : null;
    }

    private protected override void AddToArea(CNWSArea area, float x, float y, float z)
    {
      Store.AddToArea(area, x, y, z, true.ToInt());
    }
  }
}
