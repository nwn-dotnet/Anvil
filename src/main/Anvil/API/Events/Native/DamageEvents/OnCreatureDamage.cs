using System;
using System.Runtime.InteropServices;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;

namespace NWN.API.Events
{
  public sealed class OnCreatureDamage : IEvent
  {
    public NwGameObject DamagedBy { get; private init; }

    public NwGameObject Target { get; private init; }

    public DamageData<int> DamageData { get; private init; }

    NwObject IEvent.Context
    {
      get => DamagedBy;
    }

    internal unsafe class Factory : SingleHookEventFactory<Factory.OnApplyDamageHook>
    {
      internal delegate void OnApplyDamageHook(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame);

      protected override FunctionHook<OnApplyDamageHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void*, int, void> pHook = &OnApplyDamage;
        return HookService.RequestHook<OnApplyDamageHook>(pHook, FunctionsLinux._ZN21CNWSEffectListHandler13OnApplyDamageEP10CNWSObjectP11CGameEffecti, HookOrder.Late);
      }

      [UnmanagedCallersOnly]
      private static void OnApplyDamage(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame)
      {
        CNWSObject gameObject = CNWSObject.FromPointer(pObject);
        if (IsValidObjectTarget((ObjectType)gameObject.m_nObjectType))
        {
          CGameEffect effect = CGameEffect.FromPointer(pEffect);

          ProcessEvent(new OnCreatureDamage
          {
            DamagedBy = effect.m_oidCreator.ToNwObject<NwGameObject>(),
            Target = gameObject.ToNwObject<NwGameObject>(),
            DamageData = new DamageData<int>(effect.m_nParamInteger),
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

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnCreatureDamage"/>
    public event Action<OnCreatureDamage> OnCreatureDamage
    {
      add => EventService.Subscribe<OnCreatureDamage, OnCreatureDamage.Factory>(this, value);
      remove => EventService.Unsubscribe<OnCreatureDamage, OnCreatureDamage.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnCreatureDamage"/>
    public event Action<OnCreatureDamage> OnCreatureDamage
    {
      add => EventService.SubscribeAll<OnCreatureDamage, OnCreatureDamage.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCreatureDamage, OnCreatureDamage.Factory>(value);
    }
  }
}
