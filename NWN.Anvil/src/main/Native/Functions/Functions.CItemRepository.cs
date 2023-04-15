using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CItemRepository
    {
      [NativeFunction("_ZN15CItemRepository7AddItemEPP8CNWSItemhhii", "?AddItem@CItemRepository@@QEAAHPEAPEAVCNWSItem@@EEHH@Z")]
      public delegate int AddItem(void* pItemRepository, void** ppItem, byte x, byte y, byte z, int bAllowEncumbrance, int bMergeItem);

      [NativeFunction("_ZN15CItemRepository10RemoveItemEP8CNWSItem", "?RemoveItem@CItemRepository@@QEAAHPEAVCNWSItem@@@Z")]
      public delegate int RemoveItem(void* pItemRepository, void* pItem);
    }
  }
}
