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
    public NwCreature Caster { get; private init; }

    public int CasterLevel { get; private init; }

    public int ClassIndex { get; private init; }

    public Domain Domain { get; private init; }

    public NwFeat Feat { get; private init; }

    public bool IsAreaTarget { get; private init; }

    public bool IsFake { get; private init; }

    public bool IsInstant { get; private init; }

    public bool IsSpontaneous { get; private init; }

    public MetaMagic MetaMagic { get; private init; }
    public bool PreventSpellCast { get; set; }

    public ProjectilePathType ProjectilePath { get; private init; }

    public Lazy<bool> Result { get; private set; }

    public NwSpell Spell { get; private init; }

    public NwGameObject TargetObject { get; private init; }

    public Vector3 TargetPosition { get; private init; }

    NwObject IEvent.Context => Caster;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.AddCastSpellActionsHook>
    {
      internal delegate int AddCastSpellActionsHook(void* pCreature, uint nSpellId, int nMultiClass, int nDomainLevel,
        int nMetaType, int bSpontaneousCast, Vector3 vTargetLocation, uint oidTarget, int bAreaTarget, int bAddToFront,
        int bFake, byte nProjectilePathType, int bInstant, int bAllowPolymorphedCast, int nFeat, byte nCasterLevel);

      protected override FunctionHook<AddCastSpellActionsHook> RequestHook()
      {
        delegate* unmanaged<void*, uint, int, int, int, int, Vector3, uint, int, int, int, byte, int, int, int, byte, int> pHook = &OnAddCastSpellActions;
        return HookService.RequestHook<AddCastSpellActionsHook>(pHook, FunctionsLinux._ZN12CNWSCreature19AddCastSpellActionsEjiiii6Vectorjiiihiiih, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnAddCastSpellActions(void* pCreature, uint nSpellId, int nMultiClass, int nDomainLevel,
        int nMetaType, int bSpontaneousCast, Vector3 vTargetLocation, uint oidTarget, int bAreaTarget, int bAddToFront,
        int bFake, byte nProjectilePathType, int bInstant, int bAllowPolymorphedCast, int nFeat, byte nCasterLevel)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnSpellAction eventData = new OnSpellAction
        {
          Caster = creature.ToNwObject<NwCreature>(),
          Spell = NwSpell.FromSpellId((int)nSpellId),
          ClassIndex = nMultiClass,
          Domain = (Domain)nDomainLevel,
          MetaMagic = (MetaMagic)nMetaType,
          IsSpontaneous = bSpontaneousCast.ToBool(),
          TargetPosition = vTargetLocation,
          TargetObject = oidTarget.ToNwObject<NwGameObject>(),
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

        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
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
