using System.Runtime.InteropServices;
using NWN.API.Constants;
using NWN.Native.API;
using NWN.Services;
using Feat = NWN.API.Constants.Feat;

namespace NWN.API.Events
{
  public sealed class OnSpellInterrupt : IEvent
  {
    public NwGameObject InterruptedCaster { get; private init; }

    public Spell Spell { get; private init; }

    public int ClassIndex { get; private init; }

    public Feat Feat { get; private init; }

    public Domain Domain { get; private init; }

    public bool Spontaneous { get; private init; }

    public MetaMagic MetaMagic { get; private init; }

    NwObject IEvent.Context => InterruptedCaster;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.OnEffectAppliedHook>
    {
      internal delegate int OnEffectAppliedHook(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame);

      protected override FunctionHook<OnEffectAppliedHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnEffectApplied;
        return HookService.RequestHook<OnEffectAppliedHook>(pHook, FunctionsLinux._ZN21CNWSEffectListHandler15OnEffectAppliedEP10CNWSObjectP11CGameEffecti, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static int OnEffectApplied(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame)
      {
        if (pEffect == null)
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, null, bLoadingGame);
        }

        CGameEffect effect = new CGameEffect(pEffect, false);
        if (effect.m_nType != (int)EffectTrueType.VisualEffect || effect.m_nNumIntegers == 0 ||
          (effect.m_nParamInteger[0] != 292 && effect.m_nParamInteger[0] != 293))
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
        }

        CNWSObject gameObject = new CNWSObject(pObject, false);

        ProcessEvent(new OnSpellInterrupt
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
