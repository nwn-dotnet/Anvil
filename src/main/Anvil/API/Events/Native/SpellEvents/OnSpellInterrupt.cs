using System;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Services;
using NWN.API.Events;
using NWN.Native.API;
using Feat = Anvil.API.Feat;

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

    NwObject IEvent.Context
    {
      get => InterruptedCaster;
    }

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

        CGameEffect effect = CGameEffect.FromPointer(pEffect);
        if (effect.m_nType != (int)EffectTrueType.VisualEffect || effect.m_nNumIntegers == 0 ||
          effect.m_nParamInteger[0] != 292 && effect.m_nParamInteger[0] != 293)
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
        }

        CNWSObject gameObject = CNWSObject.FromPointer(pObject);

        ProcessEvent(new OnSpellInterrupt
        {
          InterruptedCaster = gameObject.ToNwObject<NwGameObject>(),
          Spell = (Spell)gameObject.m_nLastSpellId,
          ClassIndex = gameObject.m_nLastSpellCastMulticlass,
          Feat = (Feat)gameObject.m_nLastSpellCastFeat,
          Domain = (Domain)gameObject.m_nLastDomainLevel,
          Spontaneous = gameObject.m_bLastSpellCastSpontaneous.ToBool(),
          MetaMagic = (MetaMagic)gameObject.m_nLastSpellCastMetaType,
        });

        return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
      }
    }
  }
}

namespace NWN.API
{
  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="Events.OnSpellInterrupt"/>
    public event Action<OnSpellInterrupt> OnSpellInterrupt
    {
      add => EventService.Subscribe<OnSpellInterrupt, OnSpellInterrupt.Factory>(this, value);
      remove => EventService.Unsubscribe<OnSpellInterrupt, OnSpellInterrupt.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnSpellInterrupt"/>
    public event Action<OnSpellInterrupt> OnSpellInterrupt
    {
      add => EventService.SubscribeAll<OnSpellInterrupt, OnSpellInterrupt.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellInterrupt, OnSpellInterrupt.Factory>(value);
    }
  }
}
