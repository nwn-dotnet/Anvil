using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCreatureDamage : IEvent
  {
    public DamageData<int> DamageData { get; private init; }
    public NwObject DamagedBy { get; private init; }

    public NwGameObject Target { get; private init; }

    NwObject IEvent.Context => DamagedBy;

    internal unsafe class Factory : HookEventFactory
    {
      private delegate int OnApplyDamageHook(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame);

      private static FunctionHook<OnApplyDamageHook> Hook { get; set; }

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnApplyDamage;
        Hook = HookService.RequestHook<OnApplyDamageHook>(pHook, FunctionsLinux._ZN21CNWSEffectListHandler13OnApplyDamageEP10CNWSObjectP11CGameEffecti, HookOrder.Late);
        return new IDisposable[] { Hook };
      }

      private static bool IsValidObjectTarget(ObjectType objectType)
      {
        return objectType == ObjectType.Creature || objectType == ObjectType.Placeable;
      }

      [UnmanagedCallersOnly]
      private static int OnApplyDamage(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame)
      {
        CNWSObject gameObject = CNWSObject.FromPointer(pObject);
        CGameEffect effect = CGameEffect.FromPointer(pEffect);
        if (gameObject == null || effect == null)
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
        }

        if (IsValidObjectTarget((ObjectType)gameObject.m_nObjectType))
        {
          ProcessEvent(new OnCreatureDamage
          {
            DamagedBy = effect.m_oidCreator.ToNwObject<NwObject>(),
            Target = gameObject.ToNwObject<NwGameObject>(),
            DamageData = new DamageData<int>(effect.m_nParamInteger),
          });
        }

        return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
      }
    }
  }
}

namespace Anvil.API
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
