using System;
using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Native.API;

namespace NWN.Services
{
  [ServiceBinding(typeof(DialogService))]
  [ServiceBindingOptions(Lazy = true)]
  public sealed unsafe class DialogService : IDisposable
  {
    private Stack<DialogState> stateStack = new Stack<DialogState>();

    private CNWSDialog dialog;

    private CNWSDialogEntryArray dialogueEntries => CNWSDialogEntryArray.FromPointer(dialog.m_pEntries);

    private CNWSDialogReplyArray dialogueReplies => CNWSDialogReplyArray.FromPointer(dialog.m_pReplies);

    private CNWSDialogLinkEntryArray startingEntries => CNWSDialogLinkEntryArray.FromPointer(dialog.m_pStartingEntries);

    private uint indexEntry;
    private uint indexReply;

    private delegate uint GetStartEntryHook(void* pDialog, void* pNWSObjectOwner);

    private delegate int GetStartEntryOneLinerHook(void* pDialog, void* pNWSObjectOwner, void* sOneLiner, void* sSound, void* sScript, void* scriptParams);

    private delegate int SendDialogEntryHook(void* pDialog, void* pNWSObjectOwner, uint nPlayerIdGUIOnly, uint iEntry, int bPlayHelloSound);

    private delegate int SendDialogRepliesHook(void* pDialog, void* pNWSObjectOwner, uint nPlayerIdGUIOnly);

    private delegate int HandleReplyHook(void* pDialog, uint nPlayerID, void* pNWSObjectOwner, uint nReplyIndex, int bEscapeDialog, uint currentEntryIndex);

    private delegate int CheckScriptHook(void* pDialog, void* pNWSObjectOwner, void* sActive, void* scriptParams);

    private delegate void RunScriptHook(void* pDialog, void* pNWSObjectOwner, void* sScript, void* scriptParams);

    private readonly FunctionHook<GetStartEntryHook> getStartEntryHook;
    private readonly FunctionHook<GetStartEntryOneLinerHook> getStartEntryOneLinerHook;
    private readonly FunctionHook<SendDialogEntryHook> sendDialogEntryHook;
    private readonly FunctionHook<SendDialogRepliesHook> sendDialogRepliesHook;
    private readonly FunctionHook<HandleReplyHook> handleReplyHook;
    private readonly FunctionHook<CheckScriptHook> checkScriptHook;
    private readonly FunctionHook<RunScriptHook> runScriptHook;

    public DialogService(HookService hookService)
    {
      getStartEntryHook = hookService.RequestHook<GetStartEntryHook>(NWNXLib.Functions._ZN10CNWSDialog13GetStartEntryEP10CNWSObject, OnGetStartEntry, HookOrder.Early);
      getStartEntryOneLinerHook = hookService.RequestHook<GetStartEntryOneLinerHook>(NWNXLib.Functions._ZN10CNWSDialog21GetStartEntryOneLinerEP10CNWSObjectR13CExoLocStringR7CResRefS5_R13CExoArrayListI11ScriptParamE, OnGetStartEntryOneLiner, HookOrder.Early);
      sendDialogEntryHook = hookService.RequestHook<SendDialogEntryHook>(NWNXLib.Functions._ZN10CNWSDialog15SendDialogEntryEP10CNWSObjectjji, OnSendDialogEntry, HookOrder.Early);
      sendDialogRepliesHook = hookService.RequestHook<SendDialogRepliesHook>(NWNXLib.Functions._ZN10CNWSDialog17SendDialogRepliesEP10CNWSObjectj, OnSendDialogReplies, HookOrder.Early);
      handleReplyHook = hookService.RequestHook<HandleReplyHook>(NWNXLib.Functions._ZN10CNWSDialog11HandleReplyEjP10CNWSObjectjij, OnHandleReply, HookOrder.Early);
      checkScriptHook = hookService.RequestHook<CheckScriptHook>(NWNXLib.Functions._ZN10CNWSDialog11CheckScriptEP10CNWSObjectRK7CResRefRK13CExoArrayListI11ScriptParamE, OnCheckScript, HookOrder.Early);
      runScriptHook = hookService.RequestHook<RunScriptHook>(NWNXLib.Functions._ZN10CNWSDialog9RunScriptEP10CNWSObjectRK7CResRefRK13CExoArrayListI11ScriptParamE, OnRunScript, HookOrder.Early);
    }

    public ScriptType CurrentScriptType { get; private set; }

    public int loopCount { get; private set; }

    public NodeType CurrentNodeType
    {
      get
      {
        switch (stateStack.Peek())
        {
          case DialogState.Start:
            return NodeType.StartingNode;
          case DialogState.SendEntry:
            return NodeType.EntryNode;
          case DialogState.SendReplies:
          case DialogState.HandleReply:
            return NodeType.ReplyNode;
          default:
            return NodeType.Invalid;
        }
      }
    }

    public uint? CurrentNodeId
    {
      get
      {
        switch (stateStack.Peek())
        {
          case DialogState.Start:
            return startingEntries[loopCount].m_nIndex;
          case DialogState.SendEntry:
            return indexEntry;
          case DialogState.HandleReply:
            return dialogueEntries[(int)indexEntry].GetReply(indexReply).m_nIndex;
          case DialogState.SendReplies:
            return dialogueEntries[(int)dialog.m_currentEntryIndex].GetReply(loopCount).m_nIndex;
          default:
            return null;
        }
      }
    }

    public string GetCurrentNodeText(Language language = Language.English, Gender gender = Gender.Male)
    {
      CExoLocString locString = null;

      switch (stateStack.Peek())
      {
        case DialogState.Start:
          locString = dialogueEntries[(int)startingEntries[loopCount].m_nIndex].m_sText;
          break;
        case DialogState.SendEntry:
          locString = dialogueEntries[(int)indexEntry].m_sText;
          break;
        case DialogState.HandleReply:
          int handleIndex = (int)dialogueEntries[(int)indexEntry].GetReply(indexReply).m_nIndex;
          locString = dialogueReplies[handleIndex].m_sText;
          break;
        case DialogState.SendReplies:
          int sendIndex = (int)dialogueEntries[(int)indexEntry].GetReply(indexReply).m_nIndex;
          locString = dialogueReplies[sendIndex].m_sText;
          break;
      }

      if (locString != null)
      {
        CExoString str = new CExoString();
        locString.GetString((int)language, str, (byte)gender, true);
        return str.ToString();
      }

      return null;
    }

    public void SetCurrentNodeText(string text, Language language = Language.English, Gender gender = Gender.Male)
    {
      // TODO
    }

    private uint OnGetStartEntry(void* pDialog, void* pNWSObjectOwner)
    {
      dialog = new CNWSDialog(pDialog, false);
      loopCount = 0;

      stateStack.Push(DialogState.Start);
      uint retVal = getStartEntryHook.CallOriginal(pDialog, pNWSObjectOwner);
      stateStack.Pop();

      return retVal;
    }

    private int OnGetStartEntryOneLiner(void* pDialog, void* pNWSObjectOwner, void* sOneLiner, void* sSound, void* sScript, void* scriptParams)
    {
      dialog = new CNWSDialog(pDialog, false);
      loopCount = 0;

      stateStack.Push(DialogState.Start);
      int retVal = getStartEntryOneLinerHook.CallOriginal(pDialog, pNWSObjectOwner, sOneLiner, sSound, sScript, scriptParams);
      stateStack.Pop();

      return retVal;
    }

    private int OnSendDialogEntry(void* pDialog, void* pNWSObjectOwner, uint nPlayerIdGUIOnly, uint iEntry, int bPlayHelloSound)
    {
      dialog = new CNWSDialog(pDialog, false);
      loopCount = 0;

      stateStack.Push(DialogState.SendEntry);
      indexEntry = iEntry;
      int retVal = sendDialogEntryHook.CallOriginal(pDialog, pNWSObjectOwner, nPlayerIdGUIOnly, iEntry, bPlayHelloSound);
      stateStack.Pop();

      return retVal;
    }

    private int OnSendDialogReplies(void* pDialog, void* pNWSObjectOwner, uint nPlayerIdGUIOnly)
    {
      dialog = new CNWSDialog(pDialog, false);
      loopCount = 0;

      stateStack.Push(DialogState.SendReplies);
      int retVal = sendDialogRepliesHook.CallOriginal(pDialog, pNWSObjectOwner, nPlayerIdGUIOnly);
      stateStack.Pop();

      return retVal;
    }

    private int OnHandleReply(void* pDialog, uint nPlayerID, void* pNWSObjectOwner, uint nReplyIndex, int bEscapeDialog, uint currentEntryIndex)
    {
      dialog = new CNWSDialog(pDialog, false);
      loopCount = 0;

      stateStack.Push(DialogState.HandleReply);
      indexEntry = currentEntryIndex;
      indexReply = nReplyIndex;
      int retVal = handleReplyHook.CallOriginal(pDialog, nPlayerID, pNWSObjectOwner, nReplyIndex, bEscapeDialog, currentEntryIndex);
      stateStack.Pop();

      return retVal;
    }

    private int OnCheckScript(void* pDialog, void* pNWSObjectOwner, void* sActive, void* scriptParams)
    {
      dialog = new CNWSDialog(pDialog, false);

      if (stateStack.Peek() == DialogState.HandleReply)
      {
        stateStack.Pop();
        stateStack.Push(DialogState.SendEntry);

        CNWSDialogEntryArray entries = CNWSDialogEntryArray.FromPointer(dialog.m_pEntries);
        CNWSDialogReplyArray replies = CNWSDialogReplyArray.FromPointer(dialog.m_pReplies);

        indexReply = CNWSDialogLinkReplyArray.FromPointer(entries[(int)indexEntry].m_pReplies)[(int)indexReply].m_nIndex;
        indexEntry = CNWSDialogLinkEntryArray.FromPointer(replies[(int)indexReply].m_pEntries)[loopCount].m_nIndex;
      }

      CurrentScriptType = ScriptType.StartingConditional;
      int retVal = checkScriptHook.CallOriginal(pDialog, pNWSObjectOwner, sActive, scriptParams);
      loopCount++;
      CurrentScriptType = ScriptType.Other;

      return retVal;
    }

    private void OnRunScript(void* pDialog, void* pNWSObjectOwner, void* sScript, void* scriptParams)
    {
      dialog = new CNWSDialog(pDialog, false);

      CurrentScriptType = ScriptType.ActionTaken;
      runScriptHook.CallOriginal(pDialog, pNWSObjectOwner, sScript, scriptParams);
      CurrentScriptType = ScriptType.Other;
    }

    void IDisposable.Dispose()
    {
      getStartEntryHook.Dispose();
      getStartEntryOneLinerHook.Dispose();
      sendDialogEntryHook.Dispose();
      sendDialogRepliesHook.Dispose();
      handleReplyHook.Dispose();
      checkScriptHook.Dispose();
      runScriptHook.Dispose();
    }
  }
}
