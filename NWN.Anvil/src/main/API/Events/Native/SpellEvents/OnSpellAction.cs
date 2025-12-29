using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a creature queues the spell casting action.
  /// </summary>
  public sealed class OnSpellAction : IEvent
  {
    /// <summary>
    /// Gets the creature casting the spell.
    /// </summary>
    public NwCreature Caster { get; private init; } = null!;

    /// <summary>
    /// Gets the caster level used for the spell.
    /// </summary>
    public int CasterLevel { get; private init; }

    /// <summary>
    /// Gets the caster's class index used for the spell.
    /// </summary>
    public int ClassIndex { get; private init; }

    /// <summary>
    /// Gets the domain level used, if applicable.
    /// </summary>
    public NwDomain? Domain { get; private init; }

    /// <summary>
    /// Gets the feat used to cast the spell, if applicable.
    /// </summary>
    public NwFeat? Feat { get; private init; }

    /// <summary>
    /// Gets whether the target is an area location.
    /// </summary>
    public bool IsAreaTarget { get; private init; }

    /// <summary>
    /// Gets whether the spell action is a fake cast.
    /// </summary>
    public bool IsFake { get; private init; }

    /// <summary>
    /// Gets whether the spell is instant.
    /// </summary>
    public bool IsInstant { get; private init; }

    /// <summary>
    /// Gets whether the spell is cast spontaneously.
    /// </summary>
    public bool IsSpontaneous { get; private init; }

    /// <summary>
    /// Gets the metamagic applied to the spell.
    /// </summary>
    public MetaMagic MetaMagic { get; private init; }

    /// <summary>
    /// Set to true to prevent the spell action from being queued.
    /// </summary>
    public bool PreventSpellCast { get; set; }

    /// <summary>
    /// Gets the projectile path type used, if applicable.
    /// </summary>
    public ProjectilePathType ProjectilePath { get; private init; }

    /// <summary>
    /// Gets the result of queuing the spell action.
    /// </summary>
    public Lazy<bool> Result { get; private set; } = null!;

    /// <summary>
    /// Gets the spell to be cast.
    /// </summary>
    public NwSpell Spell { get; private init; } = null!;

    /// <summary>
    /// Gets the target object of the spell action, if applicable.
    /// </summary>
    public NwGameObject TargetObject { get; private init; } = null!;

    /// <summary>
    /// Gets the targeted position.
    /// </summary>
    public Vector3 TargetPosition { get; private init; }

    NwObject IEvent.Context => Caster;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.AddCastSpellActions> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, int, int, int, int, Vector3, uint, int, int, int, byte, int, int, int, byte, int> pHook = &OnAddCastSpellActions;
        Hook = HookService.RequestHook<Functions.CNWSCreature.AddCastSpellActions>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnAddCastSpellActions(void* pCreature, uint nSpellId, int nMultiClass, int nDomainLevel,
        int nMetaType, int bSpontaneousCast, Vector3 vTargetLocation, uint oidTarget, int bAreaTarget, int bAddToFront,
        int bFake, byte nProjectilePathType, int bInstant, int bAllowPolymorphedCast, int nFeat, byte nCasterLevel)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnSpellAction eventData = new OnSpellAction
        {
          Caster = creature.ToNwObject<NwCreature>()!,
          Spell = NwSpell.FromSpellId((int)nSpellId)!,
          ClassIndex = nMultiClass,
          Domain = NwDomain.FromDomainId(nDomainLevel),
          MetaMagic = (MetaMagic)nMetaType,
          IsSpontaneous = bSpontaneousCast.ToBool(),
          TargetPosition = vTargetLocation,
          TargetObject = oidTarget.ToNwObject<NwGameObject>()!,
          IsAreaTarget = bAreaTarget.ToBool(),
          IsFake = bFake.ToBool(),
          ProjectilePath = (ProjectilePathType)nProjectilePathType,
          IsInstant = bInstant.ToBool(),
          Feat = NwFeat.FromFeatId(nFeat),
          CasterLevel = nCasterLevel,
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventSpellCast &&
          Hook.CallOriginal(pCreature, nSpellId, nMultiClass, nDomainLevel,
            nMetaType, bSpontaneousCast, vTargetLocation, oidTarget, bAreaTarget, bAddToFront,
            bFake, nProjectilePathType, bInstant, bAllowPolymorphedCast, nFeat, nCasterLevel).ToBool());

        ProcessEvent(EventCallbackType.Before, eventData);
        int retVal = eventData.Result.Value.ToInt();
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
    /// <inheritdoc cref="Events.OnSpellAction"/>
    public event Action<OnSpellAction> OnSpellAction
    {
      add => EventService.Subscribe<OnSpellAction, OnSpellAction.Factory>(this, value);
      remove => EventService.Unsubscribe<OnSpellAction, OnSpellAction.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnSpellAction"/>
    public event Action<OnSpellAction> OnSpellAction
    {
      add => EventService.SubscribeAll<OnSpellAction, OnSpellAction.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellAction, OnSpellAction.Factory>(value);
    }
  }
}
