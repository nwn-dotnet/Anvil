using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when an effect is about to be removed from a creature.
  /// </summary>
  public sealed class OnEffectRemove : IEvent
  {
    /// <summary>
    /// Gets the effect being removed.
    /// </summary>
    public Effect Effect { get; init; }

    /// <summary>
    /// Gets the object that the effect was removed from.
    /// </summary>
    public NwObject Object { get; private init; }

    /// <summary>
    /// Gets or sets whether this effect should be prevented from being removed.
    /// </summary>
    public bool PreventRemove { get; set; }

    NwObject IEvent.Context => Object;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<EffectRemovedHook> Hook { get; set; }

      private delegate int EffectRemovedHook(void* pEffectListHandler, void* pObject, void* pEffect);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int> pHook = &OnEffectRemoved;
        Hook = HookService.RequestHook<EffectRemovedHook>(pHook, FunctionsLinux._ZN21CNWSEffectListHandler15OnEffectRemovedEP10CNWSObjectP11CGameEffect, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnEffectRemoved(void* pEffectListHandler, void* pObject, void* pEffect)
      {
        CNWSObject gameObject = CNWSObject.FromPointer(pObject);
        CGameEffect gameEffect = CGameEffect.FromPointer(pEffect);

        if (gameObject == null || gameEffect == null)
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect);
        }

        EffectDurationType durationType = (EffectDurationType)gameEffect.GetDurationType();
        if (durationType != EffectDurationType.Temporary && durationType != EffectDurationType.Permanent)
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect);
        }

        OnEffectRemove eventData = ProcessEvent(new OnEffectRemove
        {
          Object = gameObject.ToNwObject(),
          Effect = gameEffect.ToEffect(false),
        });

        return eventData.PreventRemove ? false.ToInt() : Hook.CallOriginal(pEffectListHandler, pObject, pEffect);
      }
    }
  }
}

namespace Anvil.API
{
  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="Events.OnEffectRemove"/>
    public event Action<OnEffectRemove> OnEffectRemove
    {
      add => EventService.Subscribe<OnEffectRemove, OnEffectRemove.Factory>(this, value);
      remove => EventService.Unsubscribe<OnEffectRemove, OnEffectRemove.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnEffectRemove"/>
    public event Action<OnEffectRemove> OnEffectRemove
    {
      add => EventService.SubscribeAll<OnEffectRemove, OnEffectRemove.Factory>(value);
      remove => EventService.UnsubscribeAll<OnEffectRemove, OnEffectRemove.Factory>(value);
    }
  }
}
