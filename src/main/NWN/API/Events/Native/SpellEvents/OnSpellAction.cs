using System;
using System.Numerics;
using System.Runtime.InteropServices;
using NWN.API.Constants;
using NWN.Native.API;
using NWN.Services;
using Feat = NWN.API.Constants.Feat;

namespace NWN.API.Events
{
  public sealed class OnSpellAction : IEvent
  {
    public bool PreventSpellCast { get; set; }

    public Lazy<bool> Result { get; private set; }

    public NwCreature Caster { get; private init; }

    public Spell Spell { get; private init; }

    public int ClassIndex { get; private init; }

    public Domain Domain { get; private init; }

    public bool IsSpontaneous { get; private init; }

    public Vector3 TargetPosition { get; private init; }

    public NwGameObject TargetObject { get; private init; }

    public bool IsAreaTarget { get; private init; }

    public bool IsFake { get; private init; }

    public ProjectilePathType ProjectilePath { get; private init; }

    public bool IsInstant { get; private init; }

    public Feat Feat { get; private init; }

    public MetaMagic MetaMagic { get; private init; }

    public int CasterLevel { get; private init; }

    NwObject IEvent.Context
    {
      get => Caster;
    }

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
          Spell = (Spell)nSpellId,
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
          Feat = (Feat)nFeat,
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
