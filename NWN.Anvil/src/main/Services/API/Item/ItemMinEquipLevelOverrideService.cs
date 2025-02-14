using Anvil.API;
using Anvil.Native;
using NLog;
using NWN.Native.API;

namespace Anvil.Services.Item
{
  [ServiceBinding(typeof(ItemMinEquipLevelOverrideService))]
  [ServiceBindingOptions(InternalBindingPriority.API, Lazy = true)]
  internal sealed unsafe class ItemMinEquipLevelOverrideService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly FunctionHook<Functions.CNWSItem.GetMinEquipLevel> minEquipLevelHook;

    public ItemMinEquipLevelOverrideService(HookService hookService)
    {
      Log.Info($"Initialising optional service {nameof(ItemMinEquipLevelOverrideService)}");
      minEquipLevelHook = hookService.RequestHook<Functions.CNWSItem.GetMinEquipLevel>(OnGetMinEquipLevel, HookOrder.Late);
    }

    public byte? GetMinEquipLevelOverride(NwItem item)
    {
      InternalVariableInt overrideValue = InternalVariables.MinEquipLevelOverride(item);
      return overrideValue.HasValue ? (byte)overrideValue.Value : null;
    }

    public void SetMinEquipLevelOverride(NwItem item, byte value)
    {
      InternalVariables.MinEquipLevelOverride(item).Value = value;
    }

    public void ClearMinEquipLevelOverride(NwItem item)
    {
      InternalVariables.MinEquipLevelOverride(item).Delete();
    }

    private byte OnGetMinEquipLevel(void* pItem)
    {
      NwItem? item = CNWSObject.FromPointer(pItem).ToNwObject<NwItem>();
      if (item != null)
      {
        InternalVariableInt overrideValue = InternalVariables.MinEquipLevelOverride(item);
        if (overrideValue.HasValue)
        {
          return (byte)overrideValue.Value;
        }
      }

      return minEquipLevelHook.CallOriginal(pItem);
    }
  }
}
