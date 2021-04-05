using System;
using System.Numerics;
using NWN.API.Constants;
using NWN.Native.API;
using NWN.Services;
using Feat = NWN.API.Constants.Feat;

namespace NWN.API.Events
{
  public class OnSpellAction : IEvent
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

    NwObject IEvent.Context => Caster;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature19AddCastSpellActionsEjiiii6Vectorjiiihiiih)]
    internal delegate int AddCastSpellActionsHook(IntPtr pCreature, uint nSpellId, int nMultiClass, int nDomainLevel,
      int nMetaType, int bSpontaneousCast, Vector3 vTargetLocation, uint oidTarget, int bAreaTarget, int bAddToFront,
      int bFake, byte nProjectilePathType, int bInstant, int bAllowPolymorphedCast, int nFeat, byte nCasterLevel);

    internal class Factory : NativeEventFactory<AddCastSpellActionsHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<AddCastSpellActionsHook> RequestHook(HookService hookService)
        => hookService.RequestHook<AddCastSpellActionsHook>(OnAddCastSpellActions, HookOrder.Early);

      private int OnAddCastSpellActions(IntPtr pCreature, uint nSpellId, int nMultiClass, int nDomainLevel,
        int nMetaType, int bSpontaneousCast, Vector3 vTargetLocation, uint oidTarget, int bAreaTarget, int bAddToFront,
        int bFake, byte nProjectilePathType, int bInstant, int bAllowPolymorphedCast, int nFeat, byte nCasterLevel)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        OnSpellAction eventData = new OnSpellAction
        {
          Caster = creature.m_idSelf.ToNwObject<NwCreature>(),
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
          CasterLevel = nCasterLevel
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
