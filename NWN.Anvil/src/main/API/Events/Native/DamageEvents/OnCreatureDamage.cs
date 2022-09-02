using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCreatureDamage : IEvent
  {
    public DamageData<int> DamageData { get; private init; } = null!;
    public NwObject DamagedBy { get; private init; } = null!;
    public NwGameObject Target { get; private init; } = null!;

    NwObject IEvent.Context => DamagedBy;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<OnApplyDamageHook> Hook { get; set; } = null!;

      private delegate int OnApplyDamageHook(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnApplyDamage;
        Hook = HookService.RequestHook<OnApplyDamageHook>(pHook, FunctionsLinux._ZN21CNWSEffectListHandler13OnApplyDamageEP10CNWSObjectP11CGameEffecti, HookOrder.Late);
        return new IDisposable[] { Hook };
      }

      private static bool IsValidObjectTarget(ObjectType objectType)
      {
        return objectType is ObjectType.Creature or ObjectType.Placeable;
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

        OnCreatureDamage? eventData = null;
        if (IsValidObjectTarget((ObjectType)gameObject.m_nObjectType))
        {
          eventData = ProcessEvent(EventCallbackType.Before, new OnCreatureDamage
          {
            DamagedBy = effect.m_oidCreator.ToNwObject<NwObject>()!,
            Target = gameObject.ToNwObject<NwGameObject>()!,
            DamageData = new DamageData<int>(effect.m_nParamInteger),
          });
        }

        int retVal = Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
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
