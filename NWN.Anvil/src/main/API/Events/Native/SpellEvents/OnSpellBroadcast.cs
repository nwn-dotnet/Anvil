using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnSpellBroadcast : IEvent
  {
    public NwCreature Caster { get; private init; } = null!;

    public int ClassIndex { get; private init; }

    public NwFeat Feat { get; private init; } = null!;
    public bool PreventSpellCast { get; set; }

    public NwSpell Spell { get; private init; } = null!;
    public NwGameObject? TargetObject { get; private init; }
    public Vector3 TargetPosition { get; private init; }

    NwObject IEvent.Context => Caster;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.BroadcastSpellCast> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, byte, ushort, void> pHook = &OnBroadcastSpellCast;
        Hook = HookService.RequestHook<Functions.CNWSCreature.BroadcastSpellCast>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static void OnBroadcastSpellCast(void* pCreature, uint nSpellId, byte nMultiClass, ushort nFeat)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
        NwObject? targetObject = null;
        Vector3 targetPosition = Vector3.Zero;

        CNWSObjectActionNode? actionNode = creature.m_pExecutingAIAction;
        if (actionNode != null && actionNode.m_nActionId == ActionIdCastSpell)
        {
          NativeArray<long>? nodeParams = actionNode.m_pParameter;
          targetObject = ((uint)nodeParams[5]).ToNwObject();
          targetPosition = targetObject is NwGameObject gameObject ? gameObject.Position : new Vector3(BitConverter.Int32BitsToSingle((int)nodeParams[6]), BitConverter.Int32BitsToSingle((int)nodeParams[7]), BitConverter.Int32BitsToSingle((int)nodeParams[8]));
        }

        targetObject ??= creature.m_oidArea.ToNwObject()!;

        OnSpellBroadcast eventData = ProcessEvent(EventCallbackType.Before, new OnSpellBroadcast
        {
          Caster = creature.ToNwObject<NwCreature>()!,
          Spell = NwSpell.FromSpellId((int)nSpellId)!,
          ClassIndex = nMultiClass,
          Feat = NwFeat.FromFeatId(nFeat)!,
          TargetObject = targetObject,
          TargetPosition = targetPosition,
        });

        if (!eventData.PreventSpellCast)
        {
          Hook.CallOriginal(pCreature, nSpellId, nMultiClass, nFeat);
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
    /// <inheritdoc cref="Events.OnSpellBroadcast"/>
    public event Action<OnSpellBroadcast> OnSpellBroadcast
    {
      add => EventService.Subscribe<OnSpellBroadcast, OnSpellBroadcast.Factory>(this, value);
      remove => EventService.Unsubscribe<OnSpellBroadcast, OnSpellBroadcast.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnSpellBroadcast"/>
    public event Action<OnSpellBroadcast> OnSpellBroadcast
    {
      add => EventService.SubscribeAll<OnSpellBroadcast, OnSpellBroadcast.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellBroadcast, OnSpellBroadcast.Factory>(value);
    }
  }
}
