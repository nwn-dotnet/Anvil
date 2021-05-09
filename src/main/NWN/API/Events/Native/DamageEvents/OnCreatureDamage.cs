using System.Runtime.InteropServices;
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

    internal unsafe class Factory : NativeEventFactory<Factory.OnApplyDamageHook>
    {
      internal delegate void OnApplyDamageHook(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame);

      protected override FunctionHook<OnApplyDamageHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void*, int, void> pHook = &OnApplyDamage;
        return HookService.RequestHook<OnApplyDamageHook>(pHook, NWNXLib.Functions._ZN21CNWSEffectListHandler13OnApplyDamageEP10CNWSObjectP11CGameEffecti, HookOrder.Late);
      }

      [UnmanagedCallersOnly]
      private static void OnApplyDamage(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame)
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

      private static bool IsValidObjectTarget(ObjectType objectType)
      {
        return objectType == ObjectType.Creature || objectType == ObjectType.Placeable;
      }
    }
  }
}
