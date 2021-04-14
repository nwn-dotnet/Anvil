using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnCombatDRBroken : IEvent
  {
    public NwCreature Creature { get; private init; }

    public DRType Type { get; private init; }

    NwObject IEvent.Context => Creature;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer)]
    internal delegate void SendFeedbackMessageHook(IntPtr pCreature, ushort nFeedbackId, IntPtr pMessageData, IntPtr pFeedbackPlayer);

    internal class Factory : NativeEventFactory<SendFeedbackMessageHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<SendFeedbackMessageHook> RequestHook(HookService hookService)
        => hookService.RequestHook<SendFeedbackMessageHook>(OnSendFeedbackMessage, HookOrder.Earliest);

      private void OnSendFeedbackMessage(IntPtr pCreature, ushort nFeedbackId, IntPtr pMessageData, IntPtr pFeedbackPlayer)
      {
        const ushort resistanceId = 66;
        const ushort reductionId = 67;
        const int remainingDRIndex = 2;

        if (pMessageData == IntPtr.Zero)
        {
          Hook.CallOriginal(pCreature, nFeedbackId, pMessageData, pFeedbackPlayer);
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
