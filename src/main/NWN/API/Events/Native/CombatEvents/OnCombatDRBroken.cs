using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnCombatDRBroken : IEvent
  {
    public NwCreature Creature { get; private init; }

    public DRType Type { get; private init; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.SendFeedbackMessageHook>
    {
      internal delegate void SendFeedbackMessageHook(void* pCreature, ushort nFeedbackId, void* pMessageData, void* pFeedbackPlayer);

      protected override FunctionHook<SendFeedbackMessageHook> RequestHook()
      {
        delegate* unmanaged<void*, ushort, void*, void*, void> pHook = &OnSendFeedbackMessage;
        return HookService.RequestHook<SendFeedbackMessageHook>(pHook, FunctionsLinux._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer, HookOrder.Earliest);
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
          Creature = creature.ToNwObject<NwCreature>(),
          Type = nFeedbackId == resistanceId ? DRType.DamageResistance : DRType.DamageReduction,
        });

        Hook.CallOriginal(pCreature, nFeedbackId, pMessageData, pFeedbackPlayer);
      }
    }
  }
}
