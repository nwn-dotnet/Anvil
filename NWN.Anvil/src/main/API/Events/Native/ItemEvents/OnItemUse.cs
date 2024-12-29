using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnItemUse : IEvent
  {
    public NwItem Item { get; private init; } = null!;

    public int ItemPropertyIndex { get; private init; }

    public int ItemSubPropertyIndex { get; private init; }

    public bool PreventUseItem { get; set; }

    public bool SuppressCannotUseFeedback { get; set; }

    public NwArea TargetArea { get; private init; } = null!;

    public NwGameObject TargetObject { get; private init; } = null!;

    public Vector3 TargetPosition { get; private init; }

    public bool UseCharges { get; set; }
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
