using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Native;
using NWN.Native.API;

namespace Anvil.Services
{
  /// <summary>
  /// Allows combat log, feedback and journal update messages to be hidden globally or per player.
  /// </summary>
  [ServiceBinding(typeof(FeedbackService))]
  public sealed class FeedbackService : IDisposable
  {
    private static readonly CServerExoApp ServerExoApp = NWNXLib.AppManager().m_pServerExoApp;

    private readonly HashSet<CombatLogMessage> globalFilterListCombatMessage = new HashSet<CombatLogMessage>();
    private readonly HashSet<FeedbackMessage> globalFilterListFeedbackMessage = new HashSet<FeedbackMessage>();

    private readonly HookService hookService;

    private readonly Dictionary<uint, HashSet<CombatLogMessage>> playerFilterListCombatMessage = new Dictionary<uint, HashSet<CombatLogMessage>>();
    private readonly Dictionary<uint, HashSet<FeedbackMessage>> playerFilterListFeedbackMessage = new Dictionary<uint, HashSet<FeedbackMessage>>();

    private FilterMode combatMessageFilterMode;

    private FilterMode feedbackMessageFilterMode;

    private FunctionHook<SendFeedbackMessageHook> sendFeedbackMessageHook;
    private FunctionHook<SendServerToPlayerCCMessageHook> sendServerToPlayerCCMessageHook;
    private FunctionHook<SendServerToPlayerJournalUpdatedHook> sendServerToPlayerJournalUpdatedHook;

    public FeedbackService(HookService hookService)
    {
      this.hookService = hookService;
    }

    private delegate void SendFeedbackMessageHook(IntPtr pCreature, ushort nFeedbackId, IntPtr pMessageData, IntPtr pFeedbackPlayer);

    private delegate int SendServerToPlayerCCMessageHook(IntPtr pMessage, uint nPlayerId, byte nMinor, IntPtr pMessageData, IntPtr pAttackData);

    private delegate int SendServerToPlayerJournalUpdatedHook(IntPtr pMessage, IntPtr pPlayer, int bQuest, int bCompleted, CExoLocStringStruct cExoLocString);

    public FilterMode CombatMessageFilterMode
    {
      get => combatMessageFilterMode;
      set
      {
        combatMessageFilterMode = value;
        if (value == FilterMode.Whitelist)
        {
          sendServerToPlayerCCMessageHook ??= hookService.RequestHook<SendServerToPlayerCCMessageHook>(OnSendServerToPlayerCCMessage,
            FunctionsLinux._ZN11CNWSMessage27SendServerToPlayerCCMessageEjhP16CNWCCMessageDataP20CNWSCombatAttackData, HookOrder.Late);
        }
      }
    }

    public FilterMode FeedbackMessageFilterMode
    {
      get => feedbackMessageFilterMode;
      set
      {
        feedbackMessageFilterMode = value;
        if (value == FilterMode.Whitelist)
        {
          sendFeedbackMessageHook ??= hookService.RequestHook<SendFeedbackMessageHook>(OnSendFeedbackMessage,
            FunctionsLinux._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer, HookOrder.Late);

          sendServerToPlayerJournalUpdatedHook ??= hookService.RequestHook<SendServerToPlayerJournalUpdatedHook>(OnSendServerToPlayerJournalUpdated,
            FunctionsLinux._ZN11CNWSMessage32SendServerToPlayerJournalUpdatedEP10CNWSPlayerii13CExoLocString, HookOrder.Late);
        }
      }
    }

    public void AddCombatLogMessageFilter(CombatLogMessage message)
    {
      sendServerToPlayerCCMessageHook ??= hookService.RequestHook<SendServerToPlayerCCMessageHook>(OnSendServerToPlayerCCMessage,
        FunctionsLinux._ZN11CNWSMessage27SendServerToPlayerCCMessageEjhP16CNWCCMessageDataP20CNWSCombatAttackData, HookOrder.Late);

      globalFilterListCombatMessage.Add(message);
    }

    public void AddCombatLogMessageFilter(CombatLogMessage message, NwPlayer player)
    {
      sendServerToPlayerCCMessageHook ??= hookService.RequestHook<SendServerToPlayerCCMessageHook>(OnSendServerToPlayerCCMessage,
        FunctionsLinux._ZN11CNWSMessage27SendServerToPlayerCCMessageEjhP16CNWCCMessageDataP20CNWSCombatAttackData, HookOrder.Late);

      playerFilterListCombatMessage.AddElement(player.ControlledCreature, message);
    }

    public void AddFeedbackMessageFilter(FeedbackMessage message)
    {
      if (message == FeedbackMessage.JournalUpdated)
      {
        sendServerToPlayerJournalUpdatedHook ??= hookService.RequestHook<SendServerToPlayerJournalUpdatedHook>(OnSendServerToPlayerJournalUpdated,
          FunctionsLinux._ZN11CNWSMessage32SendServerToPlayerJournalUpdatedEP10CNWSPlayerii13CExoLocString, HookOrder.Late);
      }
      else
      {
        sendFeedbackMessageHook ??= hookService.RequestHook<SendFeedbackMessageHook>(OnSendFeedbackMessage,
          FunctionsLinux._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer, HookOrder.Late);
      }

      globalFilterListFeedbackMessage.Add(message);
    }

    public void AddFeedbackMessageFilter(FeedbackMessage message, NwPlayer player)
    {
      if (message == FeedbackMessage.JournalUpdated)
      {
        sendServerToPlayerJournalUpdatedHook ??= hookService.RequestHook<SendServerToPlayerJournalUpdatedHook>(OnSendServerToPlayerJournalUpdated,
          FunctionsLinux._ZN11CNWSMessage32SendServerToPlayerJournalUpdatedEP10CNWSPlayerii13CExoLocString, HookOrder.Late);
      }
      else
      {
        sendFeedbackMessageHook ??= hookService.RequestHook<SendFeedbackMessageHook>(OnSendFeedbackMessage,
          FunctionsLinux._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer, HookOrder.Late);
      }

      playerFilterListFeedbackMessage.AddElement(player.ControlledCreature, message);
    }

    public bool IsCombatLogMessageHidden(CombatLogMessage message)
    {
      return IsMessageHidden(globalFilterListCombatMessage, message, CombatMessageFilterMode);
    }

    public bool IsCombatLogMessageHidden(CombatLogMessage message, NwPlayer player)
    {
      return IsMessageHidden(globalFilterListCombatMessage, playerFilterListCombatMessage, player.ControlledCreature, message, CombatMessageFilterMode);
    }

    public bool IsFeedbackMessageHidden(FeedbackMessage message)
    {
      return IsMessageHidden(globalFilterListFeedbackMessage, message, FeedbackMessageFilterMode);
    }

    public bool IsFeedbackMessageHidden(FeedbackMessage message, NwPlayer player)
    {
      return IsMessageHidden(globalFilterListFeedbackMessage, playerFilterListFeedbackMessage, player.ControlledCreature, message, FeedbackMessageFilterMode);
    }

    public void RemoveCombatMessageFilter(CombatLogMessage message)
    {
      globalFilterListCombatMessage.Remove(message);
    }

    public void RemoveCombatMessageFilter(CombatLogMessage message, NwPlayer player)
    {
      playerFilterListCombatMessage.RemoveElement(player.ControlledCreature, message);
    }

    public void RemoveFeedbackMessageFilter(FeedbackMessage message)
    {
      globalFilterListFeedbackMessage.Remove(message);
    }

    public void RemoveFeedbackMessageFilter(FeedbackMessage message, NwPlayer player)
    {
      playerFilterListFeedbackMessage.RemoveElement(player.ControlledCreature, message);
    }

    void IDisposable.Dispose()
    {
      sendFeedbackMessageHook?.Dispose();
      sendServerToPlayerCCMessageHook?.Dispose();
      sendServerToPlayerJournalUpdatedHook?.Dispose();
    }

    private bool IsMessageHidden<T>(HashSet<T> globalFilter, T message, FilterMode filterMode)
    {
      bool hasFilter = globalFilter.Contains(message);
      return filterMode == FilterMode.Blacklist ? hasFilter : !hasFilter;
    }

    private bool IsMessageHidden<T>(HashSet<T> globalFilter, Dictionary<uint, HashSet<T>> playerFilter, uint player, T message, FilterMode filterMode)
    {
      bool hasFilter = globalFilter.Contains(message) || playerFilter.ContainsElement(player, message);
      return filterMode == FilterMode.Blacklist ? hasFilter : !hasFilter;
    }

    private void OnSendFeedbackMessage(IntPtr pCreature, ushort nFeedbackId, IntPtr pMessageData, IntPtr pFeedbackPlayer)
    {
      CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
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
      CNWSPlayer player = CNWSPlayer.FromPointer(pPlayer);
      if (IsMessageHidden(globalFilterListFeedbackMessage, playerFilterListFeedbackMessage, player.m_oidPCObject, FeedbackMessage.JournalUpdated, FeedbackMessageFilterMode))
      {
        return false.ToInt();
      }

      return sendServerToPlayerJournalUpdatedHook.CallOriginal(pMessage, pPlayer, bQuest, bCompleted, cExoLocString);
    }
  }
}
