using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnHeal : IEvent
  {
    /// <summary>
    /// Gets the <see cref="NwObject"/> performing the heal.
    /// </summary>
    public NwObject Healer { get; private init; }

    /// <summary>
    /// Gets the target being healed.
    /// </summary>
    public NwGameObject Target { get; private init; }

    /// <summary>
    /// Gets or sets how much HP the heal will provide.
    /// </summary>
    public int HealAmount { get; set; }

    NwObject IEvent.Context => Healer;

    [NativeFunction(NWNXLib.Functions._ZN21CNWSEffectListHandler11OnApplyHealEP10CNWSObjectP11CGameEffecti)]
    internal delegate int OnApplyHealHook(IntPtr pEffectListHandler, IntPtr pObject, IntPtr pGameEffect, int bLoadingGame);

    internal class Factory : NativeEventFactory<OnApplyHealHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<OnApplyHealHook> RequestHook(HookService hookService)
        => hookService.RequestHook<OnApplyHealHook>(OnOnApplyHeal, HookOrder.Early);

      private int OnOnApplyHeal(IntPtr pEffectListHandler, IntPtr pObject, IntPtr pGameEffect, int bLoadingGame)
      {
        CGameEffect gameEffect = new CGameEffect(pGameEffect, false);
        CNWSObject target = new CNWSObject(pObject, false);

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
