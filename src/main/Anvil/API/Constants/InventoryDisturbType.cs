using NWN.Core;

namespace Anvil.API
{
  public enum InventoryDisturbType
  {
    Added = NWScript.INVENTORY_DISTURB_TYPE_ADDED,
    Removed = NWScript.INVENTORY_DISTURB_TYPE_REMOVED,
    Stolen = NWScript.INVENTORY_DISTURB_TYPE_STOLEN,
  }
}
