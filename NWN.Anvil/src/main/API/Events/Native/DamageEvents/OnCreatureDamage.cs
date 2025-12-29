using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when damage is applied to a creature or placeable.
  /// </summary>
  public sealed class OnCreatureDamage : IEvent
  {
    /// <summary>
    /// Gets the damage data associated with this event.
    /// </summary>
    public DamageData<int> DamageData { get; private init; } = null!;

    /// <summary>
    /// Gets the object that applied the damage.
    /// </summary>
    public NwObject DamagedBy { get; private init; } = null!;

    /// <summary>
    /// Gets the damaged creature or placeable.
    /// </summary>
    public NwGameObject Target { get; private init; } = null!;

    /// <summary>
    /// Gets the spell responsible for the damage, if any.
    /// </summary>
    public NwSpell? Spell { get; private init; }

    NwObject IEvent.Context => DamagedBy;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSEffectListHandler.OnApplyDamage> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnApplyDamage;
        Hook = HookService.RequestHook<Functions.CNWSEffectListHandler.OnApplyDamage>(pHook, HookOrder.Late);
        return [Hook];
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
            Spell = NwSpell.FromSpellId((int)effect.m_nSpellId),
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
