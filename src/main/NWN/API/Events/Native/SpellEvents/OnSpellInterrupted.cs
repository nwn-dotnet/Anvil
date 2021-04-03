using System;
using NWN.API.Constants;
using NWN.Native.API;
using NWN.Services;
using Feat = NWN.API.Constants.Feat;

namespace NWN.API.Events
{
  public class OnSpellInterrupted : IEvent
  {
    public NwGameObject InterruptedCaster { get; private init; }

    public Spell Spell { get; private init; }

    public int ClassIndex { get; private init; }

    public Feat Feat { get; private init; }

    public Domain Domain { get; private init; }

    public bool Spontaneous { get; private init; }

    public MetaMagic MetaMagic { get; private init; }

    NwObject IEvent.Context => InterruptedCaster;

    [NativeFunction(NWNXLib.Functions._ZN21CNWSEffectListHandler15OnEffectAppliedEP10CNWSObjectP11CGameEffecti)]
    internal delegate int OnEffectAppliedHook(IntPtr pEffectListHandler, IntPtr pObject, IntPtr pEffect, int bLoadingGame);

    internal class Factory : NativeEventFactory<OnEffectAppliedHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<OnEffectAppliedHook> RequestHook(HookService hookService)
        => hookService.RequestHook<OnEffectAppliedHook>(OnEffectApplied, HookOrder.Earliest);

      private unsafe int OnEffectApplied(IntPtr pEffectListHandler, IntPtr pObject, IntPtr pEffect, int bLoadingGame)
      {
        if (pEffect == IntPtr.Zero)
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
        }

        CGameEffect effect = new CGameEffect(pEffect, false);
        if (effect.m_nType != (int)EffectTrueType.VisualEffect || effect.m_nNumIntegers == 0 ||
          (effect.m_nParamInteger[0] != 292 && effect.m_nParamInteger[0] != 293))
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
        }

        CNWSObject gameObject = new CNWSObject(pObject, false);

        ProcessEvent(new OnSpellInterrupted
        {
          InterruptedCaster = gameObject.m_idSelf.ToNwObject<NwGameObject>(),
          Spell = (Spell)gameObject.m_nLastSpellId,
          ClassIndex = gameObject.m_nLastSpellCastMulticlass,
          Feat = (Feat)gameObject.m_nLastSpellCastFeat,
          Domain = (Domain)gameObject.m_nLastDomainLevel,
          Spontaneous = gameObject.m_bLastSpellCastSpontaneous.ToBool(),
          MetaMagic = (MetaMagic)gameObject.m_nLastSpellCastMetaType
        });

        return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
      }
    }
  }
}
