using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnHeal : IEvent
  {
    /// <summary>
    /// Gets or sets how much HP the heal will provide.
    /// </summary>
    public int HealAmount { get; set; }

    /// <summary>
    /// Gets the <see cref="NwObject"/> performing the heal.
    /// </summary>
    public NwObject Healer { get; private init; } = null!;

    /// <summary>
    /// Gets the target being healed.
    /// </summary>
    public NwGameObject Target { get; private init; } = null!;

    NwObject IEvent.Context => Healer;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSEffectListHandler.OnApplyHeal> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnApplyHeal;
        Hook = HookService.RequestHook<Functions.CNWSEffectListHandler.OnApplyHeal>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnApplyHeal(void* pEffectListHandler, void* pObject, void* pGameEffect, int bLoadingGame)
      {
        CGameEffect gameEffect = CGameEffect.FromPointer(pGameEffect);
        CNWSObject target = CNWSObject.FromPointer(pObject);

        OnHeal eventData = ProcessEvent(EventCallbackType.Before, new OnHeal
        {
          Healer = gameEffect.m_oidCreator.ToNwObject<NwObject>()!,
          Target = target.ToNwObject<NwGameObject>()!,
          HealAmount = gameEffect.GetInteger(0),
        });

        gameEffect.SetInteger(0, eventData.HealAmount);

        int retVal = Hook.CallOriginal(pEffectListHandler, pObject, pGameEffect, bLoadingGame);
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
    /// <inheritdoc cref="Events.OnHeal"/>
    public event Action<OnHeal> OnHeal
    {
      add => EventService.Subscribe<OnHeal, OnHeal.Factory>(this, value);
      remove => EventService.Unsubscribe<OnHeal, OnHeal.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnHeal"/>
    public event Action<OnHeal> OnHeal
    {
      add => EventService.SubscribeAll<OnHeal, OnHeal.Factory>(value);
      remove => EventService.UnsubscribeAll<OnHeal, OnHeal.Factory>(value);
    }
  }
}
