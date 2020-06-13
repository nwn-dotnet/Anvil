using NWM.API.Constants;
using NWMX.API.Constants;
using NWN.Core;

namespace NWM.API
{
  [NativeObjectInfo(ObjectType.Store, InternalObjectType.Store)]
  public sealed class NwStore : NwGameObject
  {
    internal NwStore(uint objectId) : base(objectId) {}

    public static NwStore Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return NwObjectFactory.CreateInternal<NwStore>(template, location, useAppearAnim, newTag);
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

    public void Open(NwPlayer player, int bonusMarkup = 0, int bonusMarkDown = 0)
    {
      NWScript.OpenStore(this, player, bonusMarkup, bonusMarkDown);
    }
  }
}