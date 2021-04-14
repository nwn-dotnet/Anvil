using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnCreatureDamage : IEvent
  {
    public NwGameObject DamagedBy { get; private init; }

    public NwGameObject Target { get; private init; }

    public DamageData<int> DamageData { get; private init; }

    NwObject IEvent.Context => DamagedBy;

    [NativeFunction(NWNXLib.Functions._ZN21CNWSEffectListHandler13OnApplyDamageEP10CNWSObjectP11CGameEffecti)]
    internal delegate void OnApplyDamageHook(IntPtr pEffectListHandler, IntPtr pObject, IntPtr pEffect, int bLoadingGame);

    internal class Factory : NativeEventFactory<OnApplyDamageHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<OnApplyDamageHook> RequestHook(HookService hookService)
        => hookService.RequestHook<OnApplyDamageHook>(OnApplyDamage, HookOrder.Late);

      private unsafe void OnApplyDamage(IntPtr pEffectListHandler, IntPtr pObject, IntPtr pEffect, int bLoadingGame)
      {
        CNWSObject gameObject = new CNWSObject(pObject, false);
        if (IsValidObjectTarget((ObjectType)gameObject.m_nObjectType))
        {
          CGameEffect effect = new CGameEffect(pEffect, false);

          ProcessEvent(new OnCreatureDamage
          {
            DamagedBy = effect.m_oidCreator.ToNwObject<NwGameObject>(),
            Target = gameObject.m_idSelf.ToNwObject<NwGameObject>(),
            DamageData = new DamageData<int>(effect.m_nParamInteger)
          });
        }

        Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
      }

      private bool IsValidObjectTarget(ObjectType objectType)
      {
        return objectType == ObjectType.Creature || objectType == ObjectType.Placeable;
      }
    }
  }
}
