using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a casting creature is interrupted and the spell fails.
  /// </summary>
  public sealed class OnSpellInterrupt : IEvent
  {
    /// <summary>
    /// Gets the caster's class index used for the interrupted spell.
    /// </summary>
    public int ClassIndex { get; private init; }

    /// <summary>
    /// Gets the domain level for the interrupted spell, if applicable.
    /// </summary>
    public NwDomain? Domain { get; private init; }

    /// <summary>
    /// Gets the feat associated with the interrupted spell, if applicable.
    /// </summary>
    public NwFeat? Feat { get; private init; }

    /// <summary>
    /// Gets the game object whose casting was interrupted.
    /// </summary>
    public NwGameObject InterruptedCaster { get; private init; } = null!;

    /// <summary>
    /// Gets the metamagic applied to the interrupted spell.
    /// </summary>
    public MetaMagic MetaMagic { get; private init; }

    /// <summary>
    /// Gets the interrupted spell.
    /// </summary>
    public NwSpell Spell { get; private init; } = null!;

    /// <summary>
    /// Gets whether the interrupted spell was cast spontaneously.
    /// </summary>
    public bool Spontaneous { get; private init; }

    NwObject IEvent.Context => InterruptedCaster;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSEffectListHandler.OnEffectApplied> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnEffectApplied;
        Hook = HookService.RequestHook<Functions.CNWSEffectListHandler.OnEffectApplied>(pHook, HookOrder.Earliest);
        return [Hook];
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

        OnSpellInterrupt eventData = ProcessEvent(EventCallbackType.Before, new OnSpellInterrupt
        {
          InterruptedCaster = gameObject.ToNwObject<NwGameObject>()!,
          Spell = NwSpell.FromSpellId((int)gameObject.m_nLastSpellId)!,
          ClassIndex = gameObject.m_nLastSpellCastMulticlass,
          Feat = NwFeat.FromFeatId(gameObject.m_nLastSpellCastFeat),
          Domain = NwDomain.FromDomainId(gameObject.m_nLastDomainLevel),
          Spontaneous = gameObject.m_bLastSpellCastSpontaneous.ToBool(),
          MetaMagic = (MetaMagic)gameObject.m_nLastSpellCastMetaType,
        });

        int retVal = Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
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
