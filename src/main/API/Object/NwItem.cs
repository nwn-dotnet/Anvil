using NWM.API.Constants;
using NWN;

namespace NWM.API
{
  public class NwItem : NwGameObject
  {
    /// <summary>
    /// The GameObject that has this item in its inventory, otherwise null if it is on the ground, or not in any inventory.
    /// </summary>
    public NwGameObject Possessor
    {
      get => NWScript.GetItemPossessor(this).ToNwObject<NwGameObject>();
    }

    protected internal NwItem(uint objectId) : base(objectId) {}

    public static NwItem Create(string template, Location location, bool useAppearAnim = false, string newTag = null)
    {
      return CreateInternal<NwItem>(ObjectType.Item, template, location, useAppearAnim, newTag);
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
  }
}