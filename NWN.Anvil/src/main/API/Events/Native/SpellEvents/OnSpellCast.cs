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
  public sealed class OnSpellCast : IEvent
  {
    public NwObject Caster { get; private init; } = null!;

    public int ClassIndex { get; private init; }

    public bool CounteringSpell { get; private init; }

    public bool IsInstantSpell { get; private init; }

    public NwItem? Item { get; private init; }

    public MetaMagic MetaMagic { get; private init; }

    public bool PreventSpellCast { get; set; }

    public ProjectilePathType ProjectilePathType { get; private init; }

    public NwSpell? Spell { get; private init; }

    public bool SpellCountered { get; private init; }

    public NwObject? TargetObject { get; private init; }

    public Vector3 TargetPosition { get; private init; }

    NwObject IEvent.Context => Caster;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSObject.SpellCastAndImpact> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, Vector3, uint, byte, uint, int, int, byte, int, void> pHook = &OnSpellCastAndImpact;
        Hook = HookService.RequestHook<Functions.CNWSObject.SpellCastAndImpact>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
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
