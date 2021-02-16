using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  public class Inventory
  {
    private static readonly CNWBaseItemArray baseItemArray = NWNXLib.Rules().m_pBaseItemArray;

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
        for (uint item = NWScript.GetFirstItemInInventory(owner); item != NwObject.INVALID; item = NWScript.GetNextItemInInventory(owner))
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
      return CheckFit(item.BaseItemType);
    }

    /// <summary>
    /// Gets if the specified base item type will fit in this inventory.
    /// </summary>
    /// <param name="baseItem">The base item type to check.</param>
    /// <returns>True if the item will fit, otherwise false.</returns>
    public bool CheckFit(BaseItemType baseItem)
    {
      CNWSItem tmp = new CNWSItem(NwObject.INVALID);
      tmp.m_nBaseItem = (uint)baseItem;

      byte width = baseItemArray.GetBaseItem((int)baseItem).m_nInvSlotWidth;
      byte height = baseItemArray.GetBaseItem((int)baseItem).m_nInvSlotHeight;

      for (byte y = 0; y < (repo.m_nHeight - height + 1); y++)
      {
        for (byte x = 0; x < (repo.m_nWidth - width + 1); x++)
        {
          if (repo.CheckFit(tmp, x, y).ToBool())
          {
            return true;
          }
        }
      }

      return false;
    }
  }
}
