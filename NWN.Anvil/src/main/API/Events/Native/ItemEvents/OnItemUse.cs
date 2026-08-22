using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a creature attempts to use an item.
  /// </summary>
  public sealed class OnItemUse : IEvent
  {
    /// <summary>
    /// Gets the item being used.
    /// </summary>
    public NwItem Item { get; private init; } = null!;

    /// <summary>
    /// Gets the active item property index.
    /// </summary>
    public int ItemPropertyIndex { get; private init; }

    /// <summary>
    /// Gets the active item sub-property index.
    /// </summary>
    public int ItemSubPropertyIndex { get; private init; }

    /// <summary>
    /// Set to true to prevent the item from being used.
    /// </summary>
    public bool PreventUseItem { get; set; }

    /// <summary>
    /// Set to true to suppress the "Cannot Use" feedback when use fails.
    /// </summary>
    public bool SuppressCannotUseFeedback { get; set; }

    /// <summary>
    /// Gets the targeted area, if applicable.
    /// </summary>
    public NwArea TargetArea { get; private init; } = null!;

    /// <summary>
    /// Gets the targeted object, if applicable.
    /// </summary>
    public NwGameObject TargetObject { get; private init; } = null!;

    /// <summary>
    /// Gets the targeted position, if applicable.
    /// </summary>
    public Vector3 TargetPosition { get; private init; }

    /// <summary>
    /// Gets or sets whether using the item should consume charges.
    /// </summary>
    public bool UseCharges { get; set; }

    /// <summary>
    /// Gets the creature using the item.
    /// </summary>
    public NwCreature UsedBy { get; private init; } = null!;

    NwObject IEvent.Context => UsedBy;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.UseItem> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, byte, byte, uint, Vector3, uint, int, int> pHook = &OnUseItem;
        Hook = HookService.RequestHook<Functions.CNWSCreature.UseItem>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnUseItem(void* pCreature, uint oidItem, byte nActivePropertyIndex, byte nSubPropertyIndex, uint oidTarget, Vector3 vTargetPosition, uint oidArea, int bUseCharges)
      {
        OnItemUse eventData = ProcessEvent(EventCallbackType.Before, new OnItemUse
        {
          UsedBy = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!,
          Item = oidItem.ToNwObject<NwItem>()!,
          TargetObject = oidTarget.ToNwObject<NwGameObject>()!,
          ItemPropertyIndex = nActivePropertyIndex,
          ItemSubPropertyIndex = nSubPropertyIndex,
          TargetPosition = vTargetPosition,
          TargetArea = oidArea.ToNwObject<NwArea>()!,
          UseCharges = bUseCharges.ToBool(),
        });

        if (eventData.PreventUseItem)
        {
          return eventData.SuppressCannotUseFeedback ? 1 : 0;
        }

        int result = Hook.CallOriginal(pCreature, oidItem, nActivePropertyIndex, nSubPropertyIndex, oidTarget, vTargetPosition, oidArea, eventData.UseCharges.ToInt());
        int retVal = result == 1 || eventData.SuppressCannotUseFeedback ? 1 : 0;
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnItemUse"/>
    public event Action<OnItemUse> OnItemUse
    {
      add => EventService.Subscribe<OnItemUse, OnItemUse.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemUse, OnItemUse.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnItemUse"/>
    public event Action<OnItemUse> OnItemUse
    {
      add => EventService.SubscribeAll<OnItemUse, OnItemUse.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemUse, OnItemUse.Factory>(value);
    }
  }
}
