using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnSpellInterrupt : IEvent
  {
    public int ClassIndex { get; private init; }

    public Domain Domain { get; private init; }

    public NwFeat Feat { get; private init; } = null!;
    public NwGameObject InterruptedCaster { get; private init; } = null!;

    public MetaMagic MetaMagic { get; private init; }

    public NwSpell Spell { get; private init; } = null!;

    public bool Spontaneous { get; private init; }

    NwObject? IEvent.Context => InterruptedCaster;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<OnEffectAppliedHook> Hook { get; set; } = null!;

      private delegate int OnEffectAppliedHook(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnEffectApplied;
        Hook = HookService.RequestHook<OnEffectAppliedHook>(pHook, FunctionsLinux._ZN21CNWSEffectListHandler15OnEffectAppliedEP10CNWSObjectP11CGameEffecti, HookOrder.Earliest);
        return new IDisposable[] { Hook };
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
          InterruptedCaster = gameObject.ToNwObject<NwGameObject>()!,
          Spell = NwSpell.FromSpellId((int)gameObject.m_nLastSpellId)!,
          ClassIndex = gameObject.m_nLastSpellCastMulticlass,
          Feat = NwFeat.FromFeatId(gameObject.m_nLastSpellCastFeat)!,
          Domain = (Domain)gameObject.m_nLastDomainLevel,
          Spontaneous = gameObject.m_bLastSpellCastSpontaneous.ToBool(),
          MetaMagic = (MetaMagic)gameObject.m_nLastSpellCastMetaType,
        });

        return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
      }
    }
  }
}

namespace Anvil.API
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
