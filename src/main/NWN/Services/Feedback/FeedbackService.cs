using System;
using System.Collections.Generic;
using NWN.API;
using NWN.Native;
using NWN.Native.API;

namespace NWN.Services
{
  /// <summary>
  /// Allows combat log, feedback and journal update messages to be hidden globally or per player.
  /// </summary>
  [ServiceBinding(typeof(FeedbackService))]
  public sealed class FeedbackService : IDisposable
  {
    private static readonly CServerExoApp ServerExoApp = NWNXLib.AppManager().m_pServerExoApp;

    private delegate void SendFeedbackMessageHook(IntPtr pCreature, ushort nFeedbackId, IntPtr pMessageData, IntPtr pFeedbackPlayer);

    private delegate int SendServerToPlayerCCMessageHook(IntPtr pMessage, uint nPlayerId, byte nMinor, IntPtr pMessageData, IntPtr pAttackData);

    private delegate int SendServerToPlayerJournalUpdatedHook(IntPtr pMessage, IntPtr pPlayer, int bQuest, int bCompleted, CExoLocStringStruct cExoLocString);

    private readonly HookService hookService;

    private readonly HashSet<CombatLogMessage> globalFilterListCombatMessage = new HashSet<CombatLogMessage>();
    private readonly HashSet<FeedbackMessage> globalFilterListFeedbackMessage = new HashSet<FeedbackMessage>();

    private readonly Dictionary<uint, HashSet<CombatLogMessage>> playerFilterListCombatMessage = new Dictionary<uint, HashSet<CombatLogMessage>>();
    private readonly Dictionary<uint, HashSet<FeedbackMessage>> playerFilterListFeedbackMessage = new Dictionary<uint, HashSet<FeedbackMessage>>();

    private FunctionHook<SendFeedbackMessageHook> sendFeedbackMessageHook;
    private FunctionHook<SendServerToPlayerCCMessageHook> sendServerToPlayerCCMessageHook;
    private FunctionHook<SendServerToPlayerJournalUpdatedHook> sendServerToPlayerJournalUpdatedHook;

    private FilterMode combatMessageFilterMode;

    public FilterMode CombatMessageFilterMode
    {
      get => combatMessageFilterMode;
      set
      {
        combatMessageFilterMode = value;
        if (value == FilterMode.Whitelist)
        {
          sendServerToPlayerCCMessageHook ??= hookService.RequestHook<SendServerToPlayerCCMessageHook>(
            NWNXLib.Functions._ZN11CNWSMessage27SendServerToPlayerCCMessageEjhP16CNWCCMessageDataP20CNWSCombatAttackData,
            OnSendServerToPlayerCCMessage,
            HookOrder.Late);
        }
      }
    }

    private FilterMode feedbackMessageFilterMode;

    public FilterMode FeedbackMessageFilterMode
    {
      get => feedbackMessageFilterMode;
      set
      {
        feedbackMessageFilterMode = value;
        if (value == FilterMode.Whitelist)
        {
          sendFeedbackMessageHook ??= hookService.RequestHook<SendFeedbackMessageHook>(
            NWNXLib.Functions._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer,
            OnSendFeedbackMessage,
            HookOrder.Late);

          sendServerToPlayerJournalUpdatedHook ??= hookService.RequestHook<SendServerToPlayerJournalUpdatedHook>(
            NWNXLib.Functions._ZN11CNWSMessage32SendServerToPlayerJournalUpdatedEP10CNWSPlayerii13CExoLocString,
            OnSendServerToPlayerJournalUpdated,
            HookOrder.Late);
        }
      }
    }

    public FeedbackService(HookService hookService)
    {
      this.hookService = hookService;
    }

    public void AddCombatLogMessageFilter(CombatLogMessage message)
    {
      sendServerToPlayerCCMessageHook ??= hookService.RequestHook<SendServerToPlayerCCMessageHook>(
        NWNXLib.Functions._ZN11CNWSMessage27SendServerToPlayerCCMessageEjhP16CNWCCMessageDataP20CNWSCombatAttackData,
        OnSendServerToPlayerCCMessage,
        HookOrder.Late);

      globalFilterListCombatMessage.Add(message);
    }

    public void AddCombatLogMessageFilter(CombatLogMessage message, NwPlayer player)
    {
      sendServerToPlayerCCMessageHook ??= hookService.RequestHook<SendServerToPlayerCCMessageHook>(
        NWNXLib.Functions._ZN11CNWSMessage27SendServerToPlayerCCMessageEjhP16CNWCCMessageDataP20CNWSCombatAttackData,
        OnSendServerToPlayerCCMessage,
        HookOrder.Late);

      playerFilterListCombatMessage.AddElement(player, message);
    }

    public void AddFeedbackMessageFilter(FeedbackMessage message)
    {
      if (message == FeedbackMessage.JournalUpdated)
      {
        sendServerToPlayerJournalUpdatedHook ??= hookService.RequestHook<SendServerToPlayerJournalUpdatedHook>(
          NWNXLib.Functions._ZN11CNWSMessage32SendServerToPlayerJournalUpdatedEP10CNWSPlayerii13CExoLocString,
          OnSendServerToPlayerJournalUpdated,
          HookOrder.Late);
      }
      else
      {
        sendFeedbackMessageHook ??= hookService.RequestHook<SendFeedbackMessageHook>(
          NWNXLib.Functions._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer,
          OnSendFeedbackMessage,
          HookOrder.Late);
      }

      globalFilterListFeedbackMessage.Add(message);
    }

    public void AddFeedbackMessageFilter(FeedbackMessage message, NwPlayer player)
    {
      if (message == FeedbackMessage.JournalUpdated)
      {
        sendServerToPlayerJournalUpdatedHook ??= hookService.RequestHook<SendServerToPlayerJournalUpdatedHook>(
          NWNXLib.Functions._ZN11CNWSMessage32SendServerToPlayerJournalUpdatedEP10CNWSPlayerii13CExoLocString,
          OnSendServerToPlayerJournalUpdated,
          HookOrder.Late);
      }
      else
      {
        sendFeedbackMessageHook ??= hookService.RequestHook<SendFeedbackMessageHook>(
          NWNXLib.Functions._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer,
          OnSendFeedbackMessage,
          HookOrder.Late);
      }

      playerFilterListFeedbackMessage.AddElement(player, message);
    }

    public void RemoveFeedbackMessageFilter(FeedbackMessage message)
    {
      globalFilterListFeedbackMessage.Remove(message);
    }

    public void RemoveFeedbackMessageFilter(FeedbackMessage message, NwPlayer player)
    {
      playerFilterListFeedbackMessage.RemoveElement(player, message);
    }

    public void RemoveCombatMessageFilter(CombatLogMessage message)
    {
      globalFilterListCombatMessage.Remove(message);
    }

    public void RemoveCombatMessageFilter(CombatLogMessage message, NwPlayer player)
    {
      playerFilterListCombatMessage.RemoveElement(player, message);
    }

    public bool IsFeedbackMessageHidden(FeedbackMessage message)
      => IsMessageHidden(globalFilterListFeedbackMessage, message, FeedbackMessageFilterMode);

    public bool IsFeedbackMessageHidden(FeedbackMessage message, NwPlayer player)
      => IsMessageHidden(globalFilterListFeedbackMessage, playerFilterListFeedbackMessage, player, message, FeedbackMessageFilterMode);

    public bool IsCombatLogMessageHidden(CombatLogMessage message)
      => IsMessageHidden(globalFilterListCombatMessage, message, CombatMessageFilterMode);

    public bool IsCombatLogMessageHidden(CombatLogMessage message, NwPlayer player)
      => IsMessageHidden(globalFilterListCombatMessage, playerFilterListCombatMessage, player, message, CombatMessageFilterMode);

    private bool IsMessageHidden<T>(HashSet<T> globalFilter, T message, FilterMode filterMode)
    {
      bool hasFilter = globalFilter.Contains(message);
      return filterMode == FilterMode.Blacklist ? hasFilter : !hasFilter;
    }

    private bool IsMessageHidden<T>(HashSet<T> globalFilter, Dictionary<uint, HashSet<T>> playerFilter, uint player, T message, FilterMode filterMode)
    {
      bool hasFilter = globalFilter.Contains(message) || playerFilter.ContainsElement(player, message);
      return CombatMessageFilterMode == FilterMode.Blacklist ? hasFilter : !hasFilter;
    }

    private void OnSendFeedbackMessage(IntPtr pCreature, ushort nFeedbackId, IntPtr pMessageData, IntPtr pFeedbackPlayer)
    {
      CNWSCreature creature = new CNWSCreature(pCreature, false);
      if (IsMessageHidden(globalFilterListFeedbackMessage, playerFilterListFeedbackMessage, creature.m_idSelf, (FeedbackMessage)nFeedbackId, FeedbackMessageFilterMode))
      {
        return;
      }

      sendFeedbackMessageHook.CallOriginal(pCreature, nFeedbackId, pMessageData, pFeedbackPlayer);
    }

    private int OnSendServerToPlayerCCMessage(IntPtr pMessage, uint nPlayerId, byte nMinor, IntPtr pMessageData, IntPtr pAttackData)
    {
      CNWSPlayer player = ServerExoApp.GetClientObjectByPlayerId(nPlayerId, 0).AsNWSPlayer();
      if (IsMessageHidden(globalFilterListCombatMessage, playerFilterListCombatMessage, player.m_oidPCObject, (CombatLogMessage)nMinor, CombatMessageFilterMode))
      {
        return false.ToInt();
      }

      return sendServerToPlayerCCMessageHook.CallOriginal(pMessage, nPlayerId, nMinor, pMessageData, pAttackData);
    }

    private int OnSendServerToPlayerJournalUpdated(IntPtr pMessage, IntPtr pPlayer, int bQuest, int bCompleted, CExoLocStringStruct cExoLocString)
    {
      CNWSPlayer player = new CNWSPlayer(pPlayer, false);
      if (IsMessageHidden(globalFilterListFeedbackMessage, playerFilterListFeedbackMessage, player.m_oidPCObject, FeedbackMessage.JournalUpdated, FeedbackMessageFilterMode))
      {
        return false.ToInt();
      }

      return sendServerToPlayerJournalUpdatedHook.CallOriginal(pMessage, pPlayer, bQuest, bCompleted, cExoLocString);
    }

    void IDisposable.Dispose()
    {
      sendFeedbackMessageHook?.Dispose();
      sendServerToPlayerCCMessageHook?.Dispose();
      sendServerToPlayerJournalUpdatedHook?.Dispose();
    }
  }
}
