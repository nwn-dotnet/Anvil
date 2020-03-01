using NWN;

namespace NWM.API
{
  public class NwItem : NwGameObject
  {
    protected internal NwItem(uint objectId) : base(objectId) {}
    public static NwItem Create(string template, Location location, bool useAppearAnim = false, string newTag = "")
    {
      return CreateInternal<NwItem>(ObjectType.Item, template, location, useAppearAnim, newTag);
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