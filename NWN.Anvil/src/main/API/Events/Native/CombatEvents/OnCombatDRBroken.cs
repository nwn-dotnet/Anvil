using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnCombatDRBroken : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    public DRType Type { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<SendFeedbackMessageHook> Hook { get; set; } = null!;

      private delegate void SendFeedbackMessageHook(void* pCreature, ushort nFeedbackId, void* pMessageData, void* pFeedbackPlayer);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, ushort, void*, void*, void> pHook = &OnSendFeedbackMessage;
        Hook = HookService.RequestHook<SendFeedbackMessageHook>(pHook, FunctionsLinux._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnSendFeedbackMessage(void* pCreature, ushort nFeedbackId, void* pMessageData, void* pFeedbackPlayer)
      {
        const ushort resistanceId = 66;
        const ushort reductionId = 67;
        const int remainingDRIndex = 2;

        if (pMessageData == null)
        {
          Hook.CallOriginal(pCreature, nFeedbackId, null, pFeedbackPlayer);
          return;
        }

        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
        CNWCCMessageData messageData = CNWCCMessageData.FromPointer(pMessageData);

        if (nFeedbackId != resistanceId &&
          nFeedbackId != reductionId ||
          creature.m_idSelf != messageData.GetObjectID(0) ||
          messageData.GetInteger(remainingDRIndex) != 0)
        {
          Hook.CallOriginal(pCreature, nFeedbackId, pMessageData, pFeedbackPlayer);
          return;
        }

        ProcessEvent(new OnCombatDRBroken
        {
          Creature = creature.ToNwObject<NwCreature>()!,
          Type = nFeedbackId == resistanceId ? DRType.DamageResistance : DRType.DamageReduction,
        });

        Hook.CallOriginal(pCreature, nFeedbackId, pMessageData, pFeedbackPlayer);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnCombatDRBroken"/>
    public event Action<OnCombatDRBroken> OnCombatDRBroken
    {
      add => EventService.Subscribe<OnCombatDRBroken, OnCombatDRBroken.Factory>(this, value);
      remove => EventService.Unsubscribe<OnCombatDRBroken, OnCombatDRBroken.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnCombatDRBroken"/>
    public event Action<OnCombatDRBroken> OnCombatDRBroken
    {
      add => EventService.SubscribeAll<OnCombatDRBroken, OnCombatDRBroken.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCombatDRBroken, OnCombatDRBroken.Factory>(value);
    }
  }
}
