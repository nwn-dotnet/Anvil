using System.Collections.Generic;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// Provides inventory access for an object, including item enumeration and fit checks against the repository grid.
  /// </summary>
  /// <remarks>
  /// Use <see cref="Items"/> to iterate the current contents. Fit checks validate whether a base item size can be placed within
  /// the available grid dimensions; they do not move items or account for stacking rules.
  /// </remarks>
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
    /// Gets all items currently contained in this inventory repository.
    /// </summary>
    /// <remarks>
    /// Enumeration is lazy and reflects live state; items added or removed during iteration may affect the sequence.
    /// </remarks>
    public IEnumerable<NwItem> Items
    {
      get
      {
        for (uint item = NWScript.GetFirstItemInInventory(owner); item != NwObject.Invalid; item = NWScript.GetNextItemInInventory(owner))
        {
          yield return item.ToNwObject<NwItem>()!;
        }
      }
    }

    /// <summary>
    /// Returns whether the specified item can fit within this inventory's grid.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <returns>True if the item fits within the available dimensions; otherwise, false.</returns>
    public bool CheckFit(NwItem item)
    {
      return CheckFit(item.BaseItem);
    }

    /// <summary>
    /// Returns whether the specified base item type can fit within this inventory's grid.
    /// </summary>
    /// <param name="baseItem">The base item type to check.</param>
    /// <returns>True if a placement exists that accommodates the item's slot size; otherwise, false.</returns>
    /// <remarks>
    /// This check tests all possible top-left positions against the repository width and height and the base item slot size.
    /// It does not consider weight, stack counts, or scripted placement constraints.
    /// </remarks>
    public bool CheckFit(NwBaseItem baseItem)
    {
      Vector2Int itemSize = baseItem.InventorySlotSize;

      for (byte y = 0; y < repo.m_nHeight - itemSize.Y + 1; y++)
      {
        for (byte x = 0; x < repo.m_nWidth - itemSize.X + 1; x++)
        {
          if (repo.CheckBaseItemFits(baseItem.Id, x, y).ToBool())
          {
            return true;
          }
        }
      }

      return false;
    }
  }
}
