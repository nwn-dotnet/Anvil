using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSItem
    {
      [NativeFunction("_ZN8CNWSItem14CloseInventoryEji", "")]
      public delegate void CloseInventory(void* pItem, uint oidCloser, int bUpdatePlayer);

      [NativeFunction("_ZN8CNWSItem13OpenInventoryEj", "")]
      public delegate void OpenInventory(void* pItem, uint oidOpener);
    }
  }
}
