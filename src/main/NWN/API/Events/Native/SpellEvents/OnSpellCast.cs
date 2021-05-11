using System;
using System.Numerics;
using NWN.API.Constants;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnSpellCast : IEvent
  {
    public bool PreventSpellCast { get; set; }

    public NwGameObject Caster { get; private init; }

    public Spell Spell { get; private init; }

    public Vector3 TargetPosition { get; private init; }

    public NwGameObject TargetObject { get; private init; }

    public int ClassIndex { get; private init; }

    public NwItem Item { get; private init; }

    public bool SpellCountered { get; private init; }

    public bool CounteringSpell { get; private init; }

    public ProjectilePathType ProjectilePathType { get; private init; }

    public bool IsInstantSpell { get; private init; }

    NwObject IEvent.Context => Caster;

    [NativeFunction(NWNXLib.Functions._ZN10CNWSObject18SpellCastAndImpactEj6Vectorjhjiihi)]
    internal delegate void SpellCastAndImpactHook(IntPtr pObject, int nSpellId, Vector3 targetPosition, uint oidTarget,
      byte nMultiClass, uint itemObj, int bSpellCountered, int bCounteringSpell, byte projectilePathType, int bInstantSpell);

    internal class Factory : NativeEventFactory<SpellCastAndImpactHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<SpellCastAndImpactHook> RequestHook(HookService hookService)
        => hookService.RequestHook<SpellCastAndImpactHook>(OnSpellCastAndImpact, HookOrder.Early);

      private void OnSpellCastAndImpact(IntPtr pObject, int nSpellId, Vector3 targetPosition, uint oidTarget,
        byte nMultiClass, uint itemObj, int bSpellCountered, int bCounteringSpell, byte projectilePathType, int bInstantSpell)
      {
        CNWSObject gameObject = new CNWSObject(pObject, false);

        OnSpellCast eventData = ProcessEvent(new OnSpellCast
        {
          Caster = gameObject.m_idSelf.ToNwObject<NwCreature>(),
          Spell = (Spell)nSpellId,
          TargetPosition = targetPosition,
          TargetObject = oidTarget.ToNwObject<NwGameObject>(),
          ClassIndex = nMultiClass,
          Item = itemObj.ToNwObject<NwItem>(),
          SpellCountered = bSpellCountered.ToBool(),
          CounteringSpell = bCounteringSpell.ToBool(),
          ProjectilePathType = (ProjectilePathType)projectilePathType,
          IsInstantSpell = bInstantSpell.ToBool()
        });

        if (!eventData.PreventSpellCast)
        {
          Hook.CallOriginal(pObject, nSpellId, targetPosition, oidTarget, nMultiClass, itemObj, bSpellCountered, bCounteringSpell, projectilePathType, bInstantSpell);
        }
        else
        {
          gameObject.m_bLastSpellCast = true.ToInt();
        }
      }
    }
  }
}
