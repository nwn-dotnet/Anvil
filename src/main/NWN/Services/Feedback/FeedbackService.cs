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

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature19SendFeedbackMessageEtP16CNWCCMessageDataP10CNWSPlayer)]
    private delegate void SendFeedbackMessageHook(IntPtr pCreature, ushort nFeedbackId, IntPtr pMessageData, IntPtr pFeedbackPlayer);

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage27SendServerToPlayerCCMessageEjhP16CNWCCMessageDataP20CNWSCombatAttackData)]
    private delegate int SendServerToPlayerCCMessageHook(IntPtr pMessage, uint nPlayerId, byte nMinor, IntPtr pMessageData, IntPtr pAttackData);

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage32SendServerToPlayerJournalUpdatedEP10CNWSPlayerii13CExoLocString)]
    private delegate int SendServerToPlayerJournalUpdatedHook(IntPtr pMessage, IntPtr pPlayer, int bQuest, int bCompleted, CExoLocStringStruct cExoLocString);

    private FunctionHook<SendFeedbackMessageHook> sendFeedbackMessageHook;
    private FunctionHook<SendServerToPlayerCCMessageHook> sendServerToPlayerCCMessageHook;
    private FunctionHook<SendServerToPlayerJournalUpdatedHook> sendServerToPlayerJournalUpdatedHook;

    private readonly HookService hookService;

    private readonly HashSet<CombatLogMessage> globalFilterListCombatMessage = new HashSet<CombatLogMessage>();
    private readonly HashSet<FeedbackMessage> globalFilterListFeedbackMessage = new HashSet<FeedbackMessage>();

    private readonly Dictionary<uint, HashSet<CombatLogMessage>> playerFilterListCombatMessage = new Dictionary<uint, HashSet<CombatLogMessage>>();
    private readonly Dictionary<uint, HashSet<FeedbackMessage>> playerFilterListFeedbackMessage = new Dictionary<uint, HashSet<FeedbackMessage>>();

    private FilterMode filterMode;

    public FilterMode FilterMode
    {
      get => filterMode;
      set
      {
        filterMode = value;
        if (value == FilterMode.Whitelist)
        {
          InitHooks();
        }
      }
    }

    public FeedbackService(HookService hookService)
    {
      this.hookService = hookService;
    }

    public void AddCombatLogMessageFilter(CombatLogMessage message)
    {
      sendServerToPlayerCCMessageHook ??= hookService.RequestHook<SendServerToPlayerCCMessageHook>(OnSendServerToPlayerCCMessage, HookOrder.Late);
      globalFilterListCombatMessage.Add(message);
    }

    public void AddCombatLogMessageFilter(CombatLogMessage message, NwPlayer player)
    {
      sendServerToPlayerCCMessageHook ??= hookService.RequestHook<SendServerToPlayerCCMessageHook>(OnSendServerToPlayerCCMessage, HookOrder.Late);
      playerFilterListCombatMessage.AddElement(player, message);
    }

    public void AddFeedbackMessageFilter(FeedbackMessage message)
    {
      if (message == FeedbackMessage.JournalUpdated)
      {
        sendServerToPlayerJournalUpdatedHook ??= hookService.RequestHook<SendServerToPlayerJournalUpdatedHook>(OnSendServerToPlayerJournalUpdated, HookOrder.Late);
      }
      else
      {
        sendFeedbackMessageHook ??= hookService.RequestHook<SendFeedbackMessageHook>(OnSendFeedbackMessage, HookOrder.Late);
      }

      globalFilterListFeedbackMessage.Add(message);
    }

    public void AddFeedbackMessageFilter(FeedbackMessage message, NwPlayer player)
    {
      if (message == FeedbackMessage.JournalUpdated)
      {
        sendServerToPlayerJournalUpdatedHook ??= hookService.RequestHook<SendServerToPlayerJournalUpdatedHook>(OnSendServerToPlayerJournalUpdated, HookOrder.Late);
      }
      else
      {
        sendFeedbackMessageHook ??= hookService.RequestHook<SendFeedbackMessageHook>(OnSendFeedbackMessage, HookOrder.Late);
      }

      playerFilterListFeedbackMessage.AddElement(player, message);
    }

    public bool IsFeedbackMessageHidden(FeedbackMessage message)
      => IsMessageHidden(globalFilterListFeedbackMessage, message);

    public bool IsFeedbackMessageHidden(FeedbackMessage message, NwPlayer player)
      => IsMessageHidden(globalFilterListFeedbackMessage, playerFilterListFeedbackMessage, message, player);

    public bool IsCombatLogMessageHidden(CombatLogMessage message)
      => IsMessageHidden(globalFilterListCombatMessage, message);

    public bool IsCombatLogMessageHidden(CombatLogMessage message, NwPlayer player)
      => IsMessageHidden(globalFilterListCombatMessage, playerFilterListCombatMessage, message, player);

    private void InitHooks()
    {
      sendFeedbackMessageHook ??= hookService.RequestHook<SendFeedbackMessageHook>(OnSendFeedbackMessage, HookOrder.Late);
      sendServerToPlayerCCMessageHook ??= hookService.RequestHook<SendServerToPlayerCCMessageHook>(OnSendServerToPlayerCCMessage, HookOrder.Late);
      sendServerToPlayerJournalUpdatedHook ??= hookService.RequestHook<SendServerToPlayerJournalUpdatedHook>(OnSendServerToPlayerJournalUpdated, HookOrder.Late);
    }

    private bool IsMessageHidden<T>(HashSet<T> globalFilter, T message)
    {
      bool hasFilter = globalFilter.Contains(message);
      return FilterMode == FilterMode.Blacklist ? hasFilter : !hasFilter;
    }

    private bool IsMessageHidden<T>(HashSet<T> globalFilter, Dictionary<uint, HashSet<T>> playerFilter, T message, uint player)
    {
      bool hasFilter = globalFilter.Contains(message) || playerFilter.ContainsElement(player, message);
      return FilterMode == FilterMode.Blacklist ? hasFilter : !hasFilter;
    }

    private void OnSendFeedbackMessage(IntPtr pCreature, ushort nFeedbackId, IntPtr pMessageData, IntPtr pFeedbackPlayer)
    {
      CNWSCreature creature = new CNWSCreature(pCreature, false);
      if (IsMessageHidden(globalFilterListFeedbackMessage, playerFilterListFeedbackMessage, (FeedbackMessage)nFeedbackId, creature.m_idSelf))
      {
        return;
      }

      sendFeedbackMessageHook.CallOriginal(pCreature, nFeedbackId, pMessageData, pFeedbackPlayer);
    }

    private int OnSendServerToPlayerCCMessage(IntPtr pMessage, uint nPlayerId, byte nMinor, IntPtr pMessageData, IntPtr pAttackData)
    {
      CNWSPlayer player = ServerExoApp.GetClientObjectByPlayerId(nPlayerId, 0).AsNWSPlayer();
      if (IsMessageHidden(globalFilterListCombatMessage, playerFilterListCombatMessage, (CombatLogMessage)nMinor, player.m_oidPCObject))
      {
        return false.ToInt();
      }

      return sendServerToPlayerCCMessageHook.CallOriginal(pMessage, nPlayerId, nMinor, pMessageData, pAttackData);
    }

    private int OnSendServerToPlayerJournalUpdated(IntPtr pMessage, IntPtr pPlayer, int bQuest, int bCompleted, CExoLocStringStruct cExoLocString)
    {
      CNWSPlayer player = new CNWSPlayer(pPlayer, false);
      if (IsMessageHidden(globalFilterListFeedbackMessage, playerFilterListFeedbackMessage, FeedbackMessage.JournalUpdated, player.m_oidPCObject))
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
