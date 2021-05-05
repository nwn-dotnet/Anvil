using System;
using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnCombatDRBroken : IEvent
  {
    public NwCreature Creature { get; private init; }

    public DRType Type { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.SendFeedbackMessageHook>
    {
      internal delegate void SendFeedbackMessageHook(void* pCreature, ushort nFeedbackId, void* pMessageData, void* pFeedbackPlayer);

      protected override FunctionHook<SendFeedbackMessageHook> RequestHook()
      {
        delegate* unmanaged<void*, ushort, void*, void*, void> pHook = &OnSendFeedbackMessage;
        return HookService.RequestHook<SendFeedbackMessageHook>(NWNXLib.Functions._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer, pHook, HookOrder.Earliest);
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

        CNWSCreature creature = new CNWSCreature(pCreature, false);
        CNWCCMessageData messageData = new CNWCCMessageData(pMessageData, false);

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
          Creature = creature.m_idSelf.ToNwObject<NwCreature>(),
          Type = nFeedbackId == resistanceId ? DRType.DamageResistance : DRType.DamageReduction
        });

        Hook.CallOriginal(pCreature, nFeedbackId, pMessageData, pFeedbackPlayer);
      }
    }
  }
}
