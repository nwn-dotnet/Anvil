using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
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
    public NwObject Healer { get; private init; }

    /// <summary>
    /// Gets the target being healed.
    /// </summary>
    public NwGameObject Target { get; private init; }

    NwObject IEvent.Context => Healer;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private delegate int OnApplyHealHook(void* pEffectListHandler, void* pObject, void* pGameEffect, int bLoadingGame);

      private static FunctionHook<OnApplyHealHook> Hook { get; set; }

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnApplyHeal;
        Hook = HookService.RequestHook<OnApplyHealHook>(pHook, FunctionsLinux._ZN21CNWSEffectListHandler11OnApplyHealEP10CNWSObjectP11CGameEffecti, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnApplyHeal(void* pEffectListHandler, void* pObject, void* pGameEffect, int bLoadingGame)
      {
        CGameEffect gameEffect = CGameEffect.FromPointer(pGameEffect);
        CNWSObject target = CNWSObject.FromPointer(pObject);

        OnHeal eventData = ProcessEvent(new OnHeal
        {
          Healer = gameEffect.m_oidCreator.ToNwObject<NwObject>(),
          Target = target.ToNwObject<NwGameObject>(),
          HealAmount = gameEffect.GetInteger(0),
        });

        gameEffect.SetInteger(0, eventData.HealAmount);
        return Hook.CallOriginal(pEffectListHandler, pObject, pGameEffect, bLoadingGame);
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
