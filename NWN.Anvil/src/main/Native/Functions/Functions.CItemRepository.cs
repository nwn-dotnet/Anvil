using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CItemRepository
    {
      [NativeFunction("_ZN15CItemRepository7AddItemEPP8CNWSItemhhii", "")]
      public delegate int AddItem(void* pItemRepository, void** ppItem, byte x, byte y, byte z, int bAllowEncumbrance, int bMergeItem);

      [NativeFunction("_ZN15CItemRepository10RemoveItemEP8CNWSItem", "")]
      public delegate int RemoveItem(void* pItemRepository, void* pItem);
    }
  }
}
