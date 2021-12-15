using System.Collections.Generic;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  public sealed class Inventory
  {
    private readonly NwGameObject owner;
    private readonly CItemRepository repo;

    internal Inventory(NwGameObject owner, CItemRepository repo)
    {
      this.owner = owner;
      this.repo = repo;
    }

    /// <summary>
    /// Gets all items belonging to this inventory.
    /// </summary>
    public IEnumerable<NwItem> Items
    {
      get
      {
        for (uint item = NWScript.GetFirstItemInInventory(owner); item != NwObject.Invalid; item = NWScript.GetNextItemInInventory(owner))
        {
          yield return item.ToNwObject<NwItem>();
        }
      }
    }

    /// <summary>
    /// Gets if the specified item will fit in this inventory.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the item will fit, otherwise false.</returns>
    public bool CheckFit(NwItem item)
    {
      return CheckFit(item.BaseItem);
    }

    /// <summary>
    /// Gets if the specified base item type will fit in this inventory.
    /// </summary>
    /// <param name="baseItem">The base item type to check.</param>
    /// <returns>True if the item will fit, otherwise false.</returns>
    public bool CheckFit(NwBaseItem baseItem)
    {
      Vector2Int itemSize = baseItem.InventorySlotSize;

      for (byte y = 0; y < repo.m_nHeight - itemSize.Y + 1; y++)
      {
        for (byte x = 0; x < repo.m_nWidth - itemSize.X + 1; x++)
        {
          if (repo.CheckBaseItemFits((uint)baseItem.Id, x, y).ToBool())
          {
            return true;
          }
        }
      }

      return false;
    }
  }
}
