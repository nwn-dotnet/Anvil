using NWN;

namespace NWM.API
{
  public class NwStore : NwGameObject
  {
    protected internal NwStore(uint objectId) : base(objectId) {}

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