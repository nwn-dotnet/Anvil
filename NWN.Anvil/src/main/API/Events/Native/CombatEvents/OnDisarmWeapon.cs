using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a disarm effect is applied to an object.
  /// </summary>
  public sealed class OnDisarmWeapon : IEvent
  {
    /// <summary>
    /// Gets the object that initiated the disarm.
    /// </summary>
    public NwGameObject DisarmedBy { get; private init; } = null!;

    /// <summary>
    /// Gets the object being disarmed.
    /// </summary>
    public NwGameObject DisarmedObject { get; private init; } = null!;

    /// <summary>
    /// Gets the feat used to perform the disarm.
    /// </summary>
    public NwFeat Feat { get; private init; } = null!;

    /// <summary>
    /// Gets or sets if the disarm should be prevented.
    /// </summary>
    public bool PreventDisarm { get; set; }

    /// <summary>
    /// Completes the event, and gets the final disarm result.
    /// </summary>
    public Lazy<bool> Result { get; private set; } = null!;

    NwObject IEvent.Context => DisarmedObject;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSEffectListHandler.OnApplyDisarm> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnApplyDisarm;
        Hook = HookService.RequestHook<Functions.CNWSEffectListHandler.OnApplyDisarm>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnApplyDisarm(void* pEffectHandler, void* pObject, void* pEffect, int bLoadingGame)
      {
        CNWSObject gameObject = CNWSObject.FromPointer(pObject);
        CGameEffect gameEffect = CGameEffect.FromPointer(pEffect);

        OnDisarmWeapon eventData = new OnDisarmWeapon
        {
          DisarmedObject = gameObject.ToNwObject<NwGameObject>()!,
          DisarmedBy = gameEffect.m_oidCreator.ToNwObject<NwGameObject>()!,
          Feat = gameEffect.GetInteger(0) == 1 ? NwFeat.FromFeatType(API.Feat.ImprovedDisarm) : NwFeat.FromFeatType(API.Feat.Disarm),
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventDisarm && Hook.CallOriginal(pEffectHandler, pObject, pEffect, bLoadingGame).ToBool());
        ProcessEvent(EventCallbackType.Before, eventData);

        int retVal = eventData.Result.Value.ToInt();
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }
    }
  }
}

namespace Anvil.API
{
  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="Events.OnDisarmWeapon"/>
    public event Action<OnDisarmWeapon> OnDisarmWeapon
    {
      add => EventService.Subscribe<OnDisarmWeapon, OnDisarmWeapon.Factory>(this, value);
      remove => EventService.Unsubscribe<OnDisarmWeapon, OnDisarmWeapon.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDisarmWeapon"/>
    public event Action<OnDisarmWeapon> OnDisarmWeapon
    {
      add => EventService.SubscribeAll<OnDisarmWeapon, OnDisarmWeapon.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDisarmWeapon, OnDisarmWeapon.Factory>(value);
    }
  }
}
