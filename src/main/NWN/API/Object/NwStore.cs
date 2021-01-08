using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  [NativeObjectInfo(ObjectTypes.Store, ObjectType.Store)]
  public sealed class NwStore : NwGameObject
  {
    internal NwStore(uint objectId) : base(objectId) {}

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
  }
}
