using System.Runtime.InteropServices;
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

    internal sealed unsafe class Factory : NativeEventFactory<Factory.OnApplyHealHook>
    {
      internal delegate int OnApplyHealHook(void* pEffectListHandler, void* pObject, void* pGameEffect, int bLoadingGame);

      protected override FunctionHook<OnApplyHealHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnApplyHeal;
        return HookService.RequestHook<OnApplyHealHook>(pHook, NWNXLib.Functions._ZN21CNWSEffectListHandler11OnApplyHealEP10CNWSObjectP11CGameEffecti, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnApplyHeal(void* pEffectListHandler, void* pObject, void* pGameEffect, int bLoadingGame)
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
