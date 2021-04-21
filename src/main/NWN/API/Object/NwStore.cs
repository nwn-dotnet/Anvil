using System;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Store, ObjectType.Store)]
  public sealed class NwStore : NwGameObject
  {
    internal readonly CNWSStore Store;

    internal NwStore(CNWSStore store) : base(store)
    {
      this.Store = store;
    }

    public static implicit operator CNWSStore(NwStore store)
    {
      return store?.Store;
    }

    /// <inheritdoc cref="NWN.API.Events.StoreEvents.OnOpen"/>
    public event Action<StoreEvents.OnOpen> OnOpen
    {
      add => EventService.Subscribe<StoreEvents.OnOpen, GameEventFactory>(this, value)
        .Register<StoreEvents.OnOpen>(this);
      remove => EventService.Unsubscribe<StoreEvents.OnOpen, GameEventFactory>(this, value);
    }

    /// <inheritdoc cref="NWN.API.Events.StoreEvents.OnClose"/>
    public event Action<StoreEvents.OnClose> OnClose
    {
      add => EventService.Subscribe<StoreEvents.OnClose, GameEventFactory>(this, value)
        .Register<StoreEvents.OnClose>(this);
      remove => EventService.Unsubscribe<StoreEvents.OnClose, GameEventFactory>(this, value);
    }

    public override Location Location
    {
      set
      {
        Store.AddToArea(value.Area, value.Position.X, value.Position.Y, value.Position.Z, true.ToInt());
        Rotation = value.Rotation;
      }
    }

    public static NwStore Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return NwObject.CreateInternal<NwStore>(template, location, useAppearAnim, newTag);
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
    /// Open oStore for oPC.<br/>
    /// You can mark up or down the prices with the optional parameters.<br/>
    /// </summary>
    /// <param name="player">The player to present the store.</param>
    /// <param name="bonusMarkup">A number in percent to mark up prices. (Default: 0).</param>
    /// <param name="bonusMarkDown">A number in percent to mark down prices. (Default: 0).</param>
    /// <remarks>If bonusMarkup is given a value of 10, prices will be 110% of the normal prices.</remarks>
    public void Open(NwPlayer player, int bonusMarkup = 0, int bonusMarkDown = 0)
    {
      NWScript.OpenStore(this, player, bonusMarkup, bonusMarkDown);
    }

    public void AcquireItem(NwItem item, bool displayFeedback = true)
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

      NativeUtils.DeserializeGff(serialized, (resGff, resStruct) =>
      {
        if (!resGff.IsValidGff("UTM"))
        {
          return false;
        }

        store = new CNWSStore(INVALID);
        if (store.LoadStore(resGff, resStruct, null).ToBool())
        {
          store.LoadObjectState(resGff, resStruct);
          return true;
        }

        store.Dispose();
        return false;
      });

      return store != null ? store.m_idSelf.ToNwObject<NwStore>() : null;
    }
  }
}
