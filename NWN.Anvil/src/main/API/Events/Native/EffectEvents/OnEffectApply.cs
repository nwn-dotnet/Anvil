using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using NWN.Native.API;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when an effect is about to be applied to a creature.
  /// </summary>
  public sealed class OnEffectApply : IEvent
  {
    /// <summary>
    /// Gets the effect being applied.
    /// </summary>
    public Effect Effect { get; init; }

    /// <summary>
    /// Gets the object that the effect is being applied to.
    /// </summary>
    public NwObject Object { get; private init; }

    /// <summary>
    /// Gets or sets whether this effect should be prevented from being applied.
    /// </summary>
    public bool PreventApply { get; set; }

    NwObject IEvent.Context
    {
      get => Object;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.EffectAppliedHook>
    {
      internal delegate int EffectAppliedHook(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame);

      protected override FunctionHook<EffectAppliedHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnEffectApplied;
        return HookService.RequestHook<EffectAppliedHook>(pHook, FunctionsLinux._ZN21CNWSEffectListHandler15OnEffectAppliedEP10CNWSObjectP11CGameEffecti, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static int OnEffectApplied(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame)
      {
        CNWSObject gameObject = CNWSObject.FromPointer(pObject);
        CGameEffect gameEffect = CGameEffect.FromPointer(pEffect);

        if (gameObject == null || gameEffect == null)
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
        }

        EffectDurationType durationType = (EffectDurationType)(gameEffect.m_nSubType & Effect.DurationMask);
        if (durationType != EffectDurationType.Temporary && durationType != EffectDurationType.Permanent)
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
        }

        OnEffectApply eventData = ProcessEvent(new OnEffectApply
        {
          Object = gameObject.ToNwObject(),
          Effect = gameEffect.ToEffect(),
        });

        return eventData.PreventApply ? false.ToInt() : Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
      }
    }
  }
}

namespace Anvil.API
{
  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="Events.OnEffectApply"/>
    public event Action<OnEffectApply> OnEffectApply
    {
      add => EventService.Subscribe<OnEffectApply, OnEffectApply.Factory>(this, value);
      remove => EventService.Unsubscribe<OnEffectApply, OnEffectApply.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnEffectApply"/>
    public event Action<OnEffectApply> OnEffectApply
    {
      add => EventService.SubscribeAll<OnEffectApply, OnEffectApply.Factory>(value);
      remove => EventService.UnsubscribeAll<OnEffectApply, OnEffectApply.Factory>(value);
    }
  }
}
