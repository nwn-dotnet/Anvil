using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnSpellAction : IEvent
  {
    public NwCreature Caster { get; private init; } = null!;

    public int CasterLevel { get; private init; }

    public int ClassIndex { get; private init; }

    public NwDomain? Domain { get; private init; }

    public NwFeat? Feat { get; private init; }

    public bool IsAreaTarget { get; private init; }

    public bool IsFake { get; private init; }

    public bool IsInstant { get; private init; }

    public bool IsSpontaneous { get; private init; }

    public MetaMagic MetaMagic { get; private init; }
    public bool PreventSpellCast { get; set; }

    public ProjectilePathType ProjectilePath { get; private init; }

    public Lazy<bool> Result { get; private set; } = null!;

    public NwSpell Spell { get; private init; } = null!;

    public NwGameObject TargetObject { get; private init; } = null!;

    public Vector3 TargetPosition { get; private init; }

    NwObject IEvent.Context => Caster;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<AddCastSpellActionsHook> Hook { get; set; } = null!;

      [NativeFunction("_ZN12CNWSCreature19AddCastSpellActionsEjiiii6Vectorjiiihiiih", "")]
      private delegate int AddCastSpellActionsHook(void* pCreature, uint nSpellId, int nMultiClass, int nDomainLevel,
        int nMetaType, int bSpontaneousCast, Vector3 vTargetLocation, uint oidTarget, int bAreaTarget, int bAddToFront,
        int bFake, byte nProjectilePathType, int bInstant, int bAllowPolymorphedCast, int nFeat, byte nCasterLevel);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, int, int, int, int, Vector3, uint, int, int, int, byte, int, int, int, byte, int> pHook = &OnAddCastSpellActions;
        Hook = HookService.RequestHook<AddCastSpellActionsHook>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
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
