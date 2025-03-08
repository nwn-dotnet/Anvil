using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a creature is about to use an object.
  /// </summary>
  public sealed class OnObjectUse : IEvent
  {
    /// <summary>
    /// The object that is being used.
    /// </summary>
    public NwGameObject Object { get; private init; } = null!;

    /// <summary>
    /// The creature using the object.
    /// </summary>
    public NwCreature UsedBy { get; private init; } = null!;

    /// <summary>
    /// Gets or sets if usage of the object should be prevented.
    /// </summary>
    public bool PreventObjectUse { get; set; }

    NwObject IEvent.Context => UsedBy;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSObject.AddUseObjectAction> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, int> pHook = &OnAddUseObjectAction;
        Hook = HookService.RequestHook<Functions.CNWSObject.AddUseObjectAction>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnAddUseObjectAction(void* pObject, uint oidObjectToUse)
      {
        NwCreature? usedBy = CNWSObject.FromPointer(pObject).ToNwObjectSafe<NwCreature>();
        NwGameObject? gameObject = oidObjectToUse.ToNwObjectSafe<NwGameObject>();

        if (usedBy == null || gameObject == null)
        {
          return Hook.CallOriginal(pObject, oidObjectToUse);
        }

        OnObjectUse eventData = ProcessEvent(EventCallbackType.Before, new OnObjectUse
        {
          UsedBy = usedBy,
          Object = gameObject,
        });

        int retVal = eventData.PreventObjectUse ? false.ToInt() : Hook.CallOriginal(pObject, oidObjectToUse);
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
    /// <inheritdoc cref="Events.OnObjectUse"/>
    public event Action<OnObjectUse> OnObjectUse
    {
      add => EventService.Subscribe<OnObjectUse, OnObjectUse.Factory>(this, value);
      remove => EventService.Unsubscribe<OnObjectUse, OnObjectUse.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnObjectUse"/>
    public event Action<OnObjectUse> OnObjectUse
    {
      add => EventService.SubscribeAll<OnObjectUse, OnObjectUse.Factory>(value);
      remove => EventService.UnsubscribeAll<OnObjectUse, OnObjectUse.Factory>(value);
    }
  }
}
