using System.Linq;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A base item type definition.
  /// </summary>
  public sealed class NwBaseItem
  {
    private readonly CNWBaseItem baseItem;

    internal NwBaseItem(CNWBaseItem baseItem, BaseItemType itemType)
    {
      this.baseItem = baseItem;
      ItemType = itemType;
    }

    /// <summary>
    /// Gets the size/number of inventory slots that this base item occupies.
    /// </summary>
    public Vector2Int InventorySlotSize
    {
      get => new Vector2Int(baseItem.m_nInvSlotWidth, baseItem.m_nInvSlotHeight);
    }

    /// <summary>
    /// Gets the associated <see cref="BaseItemType"/> for this base item.
    /// </summary>
    public BaseItemType ItemType { get; }

    /// <summary>
    /// Gets the maximum stack size for this base item.
    /// </summary>
    public int MaxStackSize
    {
      get => baseItem.m_nStackSize;
    }

    /// <summary>
    /// Gets if this item type can be stacked.
    /// </summary>
    public bool IsStackable
    {
      get => MaxStackSize > 1;
    }

    /// <summary>
    /// Resolves a <see cref="NwBaseItem"/> from a base item id.
    /// </summary>
    /// <param name="itemId">The item id to resolve.</param>
    /// <returns>The associated <see cref="NwBaseItem"/> instance. Null if the base item id is invalid.</returns>
    public static NwBaseItem FromItemId(int itemId)
    {
      return NwRuleset.BaseItems.ElementAtOrDefault(itemId);
    }

    /// <summary>
    /// Resolves a <see cref="NwBaseItem"/> from a <see cref="BaseItemType"/>.
    /// </summary>
    /// <param name="itemType">The item type to resolve.</param>
    /// <returns>The associated <see cref="NwBaseItem"/> instance. Null if the base item type is invalid.</returns>
    public static NwBaseItem FromItemType(BaseItemType itemType)
    {
      return NwRuleset.BaseItems.ElementAtOrDefault((int)itemType);
    }
  }
}
