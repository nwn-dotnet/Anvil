using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnDisarmWeapon : IEvent
  {
    public NwGameObject DisarmedBy { get; private init; }

    public NwGameObject DisarmedObject { get; private init; }

    public NwFeat Feat { get; private init; }
    public bool PreventDisarm { get; set; }

    public Lazy<bool> Result { get; private set; }

    NwObject IEvent.Context => DisarmedObject;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<ApplyDisarmHook> Hook { get; set; }

      private delegate int ApplyDisarmHook(void* pEffectHandler, void* pObject, void* pEffect, int bLoadingGame);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnApplyDisarm;
        Hook = HookService.RequestHook<ApplyDisarmHook>(pHook, FunctionsLinux._ZN21CNWSEffectListHandler13OnApplyDisarmEP10CNWSObjectP11CGameEffecti, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnApplyDisarm(void* pEffectHandler, void* pObject, void* pEffect, int bLoadingGame)
      {
        CNWSObject gameObject = CNWSObject.FromPointer(pObject);
        CGameEffect gameEffect = CGameEffect.FromPointer(pEffect);

        OnDisarmWeapon eventData = new OnDisarmWeapon
        {
          DisarmedObject = gameObject.ToNwObject<NwGameObject>(),
          DisarmedBy = gameEffect.m_oidCreator.ToNwObject<NwGameObject>(),
          Feat = gameEffect.GetInteger(0) == 1 ? NwFeat.FromFeatType(API.Feat.ImprovedDisarm) : NwFeat.FromFeatType(API.Feat.Disarm),
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventDisarm && Hook.CallOriginal(pEffectHandler, pObject, pEffect, bLoadingGame).ToBool());
        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
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
