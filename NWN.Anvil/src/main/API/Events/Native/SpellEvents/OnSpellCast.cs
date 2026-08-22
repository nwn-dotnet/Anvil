using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a game object casts a spell.
  /// </summary>
  public sealed class OnSpellCast : IEvent
  {
    /// <summary>
    /// Gets the object casting the spell.
    /// </summary>
    public NwObject Caster { get; private init; } = null!;

    /// <summary>
    /// Gets the caster's class index used for this spell.
    /// </summary>
    public int ClassIndex { get; private init; }

    /// <summary>
    /// Gets whether this cast is countering another spell.
    /// </summary>
    public bool CounteringSpell { get; private init; }

    /// <summary>
    /// Gets whether the spell is an instant spell.
    /// </summary>
    public bool IsInstantSpell { get; private init; }

    /// <summary>
    /// Gets the item used to cast the spell, if any.
    /// </summary>
    public NwItem? Item { get; private init; }

    /// <summary>
    /// Gets the metamagic applied to the spell.
    /// </summary>
    public MetaMagic MetaMagic { get; private init; }

    /// <summary>
    /// Set to true to prevent the spell from being cast.
    /// </summary>
    public bool PreventSpellCast { get; set; }

    /// <summary>
    /// Gets the projectile path type used by the spell, if applicable.
    /// </summary>
    public ProjectilePathType ProjectilePathType { get; private init; }

    /// <summary>
    /// Gets the spell being cast.
    /// </summary>
    public NwSpell? Spell { get; private init; }

    /// <summary>
    /// Gets whether the spell was countered.
    /// </summary>
    public bool SpellCountered { get; private init; }

    /// <summary>
    /// Gets the target object of the spell, if any.
    /// </summary>
    public NwObject? TargetObject { get; private init; }

    /// <summary>
    /// Gets the targeted position of the spell.
    /// </summary>
    public Vector3 TargetPosition { get; private init; }

    NwObject IEvent.Context => Caster;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSObject.SpellCastAndImpact> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, Vector3, uint, byte, uint, int, int, byte, int, void> pHook = &OnSpellCastAndImpact;
        Hook = HookService.RequestHook<Functions.CNWSObject.SpellCastAndImpact>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static void OnSpellCastAndImpact(void* pObject, int nSpellId, Vector3 targetPosition, uint oidTarget,
        byte nMultiClass, uint itemObj, int bSpellCountered, int bCounteringSpell, byte projectilePathType, int bInstantSpell)
      {
        CNWSObject gameObject = CNWSObject.FromPointer(pObject);

        OnSpellCast eventData = null!;
        VirtualMachine.ExecuteInScriptContext(() =>
        {
          eventData = ProcessEvent(EventCallbackType.Before, new OnSpellCast
          {
            Caster = gameObject.ToNwObject()!,
            Spell = NwSpell.FromSpellId(nSpellId)!,
            TargetPosition = targetPosition,
            TargetObject = oidTarget.ToNwObject()!,
            ClassIndex = nMultiClass,
            Item = itemObj.ToNwObject<NwItem>()!,
            SpellCountered = bSpellCountered.ToBool(),
            CounteringSpell = bCounteringSpell.ToBool(),
            ProjectilePathType = (ProjectilePathType)projectilePathType,
            IsInstantSpell = bInstantSpell.ToBool(),
            MetaMagic = (MetaMagic)NWScript.GetMetaMagicFeat(),
          }, false);
        });

        if (!eventData.PreventSpellCast)
        {
          Hook.CallOriginal(pObject, nSpellId, targetPosition, oidTarget, nMultiClass, itemObj, bSpellCountered, bCounteringSpell, projectilePathType, bInstantSpell);
        }
        else
        {
          gameObject.m_bLastSpellCast = true.ToInt();
        }

        ProcessEvent(EventCallbackType.After, eventData);
      }
    }
  }
}

namespace Anvil.API
{
  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="Events.OnSpellCast"/>
    public event Action<OnSpellCast> OnSpellCast
    {
      add => EventService.Subscribe<OnSpellCast, OnSpellCast.Factory>(this, value);
      remove => EventService.Unsubscribe<OnSpellCast, OnSpellCast.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnSpellCast"/>
    public event Action<OnSpellCast> OnSpellCast
    {
      add => EventService.SubscribeAll<OnSpellCast, OnSpellCast.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellCast, OnSpellCast.Factory>(value);
    }
  }
}
