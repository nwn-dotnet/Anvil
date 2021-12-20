using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnItemUse : IEvent
  {
    public NwItem Item { get; private init; }

    public int ItemPropertyIndex { get; private init; }

    public int ItemSubPropertyIndex { get; private init; }

    public bool PreventUseItem { get; set; }

    public bool SuppressCannotUseFeedback { get; set; }

    public NwArea TargetArea { get; private init; }

    public NwGameObject TargetObject { get; private init; }

    public Vector3 TargetPosition { get; private init; }

    public bool UseCharges { get; set; }
    public NwCreature UsedBy { get; private init; }

    NwObject IEvent.Context => UsedBy;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.UseItemHook>
    {
      internal delegate int UseItemHook(void* pCreature, uint oidItem, byte nActivePropertyIndex, byte nSubPropertyIndex, uint oidTarget, Vector3 vTargetPosition, uint oidArea, int bUseCharges);

      protected override FunctionHook<UseItemHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, byte, byte, uint, Vector3, uint, int, int> pHook = &OnUseItem;
        return HookService.RequestHook<UseItemHook>(pHook, FunctionsLinux._ZN12CNWSCreature7UseItemEjhhj6Vectorji, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnUseItem(void* pCreature, uint oidItem, byte nActivePropertyIndex, byte nSubPropertyIndex, uint oidTarget, Vector3 vTargetPosition, uint oidArea, int bUseCharges)
      {
        OnItemUse eventData = ProcessEvent(new OnItemUse
        {
          UsedBy = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>(),
          Item = oidItem.ToNwObject<NwItem>(),
          TargetObject = oidTarget.ToNwObject<NwGameObject>(),
          ItemPropertyIndex = nActivePropertyIndex,
          ItemSubPropertyIndex = nSubPropertyIndex,
          TargetPosition = vTargetPosition,
          TargetArea = oidArea.ToNwObject<NwArea>(),
          UseCharges = bUseCharges.ToBool(),
        });

        if (eventData.PreventUseItem)
        {
          return eventData.SuppressCannotUseFeedback ? 1 : 0;
        }

        int result = Hook.CallOriginal(pCreature, oidItem, nActivePropertyIndex, nSubPropertyIndex, oidTarget, vTargetPosition, oidArea, eventData.UseCharges.ToInt());
        return result == 1 || eventData.SuppressCannotUseFeedback ? 1 : 0;
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
