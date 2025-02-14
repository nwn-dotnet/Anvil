using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSItem
    {
      [NativeFunction("_ZN8CNWSItem14CloseInventoryEji", "?CloseInventory@CNWSItem@@QEAAXIH@Z")]
      public delegate void CloseInventory(void* pItem, uint oidCloser, int bUpdatePlayer);

      [NativeFunction("_ZN8CNWSItem16GetMinEquipLevelEv", "?GetMinEquipLevel@CNWSItem@@QEAAEXZ")]
      public delegate byte GetMinEquipLevel(void* pItem);

      [NativeFunction("_ZN8CNWSItem13OpenInventoryEj", "?OpenInventory@CNWSItem@@QEAAXI@Z")]
      public delegate void OpenInventory(void* pItem, uint oidOpener);
    }
  }
}
