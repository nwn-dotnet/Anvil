using System;
using System.Collections.Generic;
using Anvil.Internal;
using NWN.API;
using NWN.Native.API;

namespace NWN.Services
{
  [ServiceBinding(typeof(ChatService))]
  public sealed unsafe partial class ChatService
  {
    private const uint PlayerIdAllServerAdmins = 0x0FFFFFFF5;
    private const uint PlayerIdAllGameMasters = 0x0FFFFFFF6;
    private const uint PlayerIdAllPlayers = 0x0FFFFFFF7;
    private const uint PlayerIdServer = 0x0FFFFFFFD;
    private const uint PlayerIdInvalidId = 0x0FFFFFFFE;
    private const uint PlayerIdAllClients = 0x0FFFFFFFF;

    private readonly FunctionHook<SendServerToPlayerChatMessageHook> sendServerToPlayerChatMessageHook;

    private delegate int SendServerToPlayerChatMessageHook(void* pMessage, ChatChannel nChatMessageType, uint oidSpeaker, void* sSpeakerMessage, uint nTellPlayerId, void* tellName);

    private readonly Dictionary<NwPlayer, Dictionary<ChatChannel, float>> playerHearingDistances = new Dictionary<NwPlayer, Dictionary<ChatChannel, float>>();

    private readonly Dictionary<ChatChannel, float> globalHearingDistances = new Dictionary<ChatChannel, float>
    {
      { ChatChannel.DmTalk, 20.0f },
      { ChatChannel.PlayerTalk, 20.0f },
      { ChatChannel.DmWhisper, 3.0f },
      { ChatChannel.PlayerWhisper, 3.0f },
    };

    private bool customHearingDistances;

    public ChatService(Lazy<EventService> eventService, HookService hookService)
    {
      this.eventService = eventService;
      sendServerToPlayerChatMessageHook = hookService.RequestHook<SendServerToPlayerChatMessageHook>(OnSendServerToPlayerChatMessage, FunctionsLinux._ZN11CNWSMessage29SendServerToPlayerChatMessageEhj10CExoStringjRKS0_, HookOrder.Late);
    }

    public bool SendMessage(ChatChannel chatChannel, string message, NwGameObject speaker, NwPlayer target = null)
    {
      uint playerId = target != null ? target.Player.m_nPlayerID : PlayerIdAllClients;
      if (playerId == PlayerIdInvalidId)
      {
        return false;
      }

      bool sentMessage = false;
      CNWSMessage messageDispatch = LowLevel.ServerExoApp.GetNWSMessage();

      if (target != null)
      {
        // This means we're sending this to one player only.
        // The normal function broadcasts in an area for talk, shout, and whisper, therefore
        // we need to call these functions directly if we are in those categories.
        switch (chatChannel)
        {
          case ChatChannel.PlayerTalk:
            messageDispatch.SendServerToPlayerChat_Talk(playerId, speaker, message.ToExoString());
            sentMessage = true;
            break;
          case ChatChannel.DmTalk:
            messageDispatch.SendServerToPlayerChat_DM_Talk(playerId, speaker, message.ToExoString());
            sentMessage = true;
            break;
          case ChatChannel.DmDm:
          case ChatChannel.PlayerDm:
            messageDispatch.SendServerToPlayerChat_DM_Silent_Shout(playerId, speaker, message.ToExoString());
            sentMessage = true;
            break;
          case ChatChannel.PlayerShout:
          case ChatChannel.DmShout:
            messageDispatch.SendServerToPlayerChat_Shout(playerId, speaker, message.ToExoString());
            sentMessage = true;
            break;
          case ChatChannel.PlayerWhisper:
            messageDispatch.SendServerToPlayerChat_Whisper(playerId, speaker, message.ToExoString());
            sentMessage = true;
            break;
          case ChatChannel.DmWhisper:
            messageDispatch.SendServerToPlayerChat_DM_Whisper(playerId, speaker, message.ToExoString());
            sentMessage = true;
            break;
          case ChatChannel.PlayerParty:
          case ChatChannel.DmParty:
            messageDispatch.SendServerToPlayerChat_Party(playerId, speaker, message.ToExoString());
            sentMessage = true;
            break;
        }
      }

      if (!sentMessage)
      {
        messageDispatch.SendServerToPlayerChatMessage((byte)chatChannel, speaker, message.ToExoString(), playerId, null);
      }

      return true;
    }

    public float GetChatHearingDistance(ChatChannel chatChannel)
    {
      globalHearingDistances.TryGetValue(chatChannel, out float retVal);
      return retVal;
    }

    public float GetChatHearingDistance(NwPlayer player, ChatChannel chatChannel)
    {
      if (playerHearingDistances.TryGetValue(player, out Dictionary<ChatChannel, float> playerHearingDistance))
      {
        if (playerHearingDistance.TryGetValue(chatChannel, out float retVal))
        {
          return retVal;
        }
      }

      return GetChatHearingDistance(chatChannel);
    }

    public void SetChatHearingDistance(ChatChannel chatChannel, float distance)
    {
      globalHearingDistances[chatChannel] = distance;
      customHearingDistances = true;
    }

    public void SetChatHearingDistance(NwPlayer player, ChatChannel chatChannel, float distance)
    {
      if (!playerHearingDistances.TryGetValue(player, out Dictionary<ChatChannel, float> playerHearingDistance))
      {
        playerHearingDistance = new Dictionary<ChatChannel, float>();
        playerHearingDistances[player] = playerHearingDistance;
      }

      playerHearingDistance[chatChannel] = distance;
      customHearingDistances = true;
    }

    private int OnSendServerToPlayerChatMessage(void* pMessage, ChatChannel nChatMessageType, uint oidSpeaker, void* sSpeakerMessage, uint nTellPlayerId, void* tellName)
    {
      if (!isEventHooked && !customHearingDistances)
      {
        return sendServerToPlayerChatMessageHook.CallOriginal(pMessage, nChatMessageType, oidSpeaker, sSpeakerMessage, nTellPlayerId, tellName);
      }

      CNWSMessage message = CNWSMessage.FromPointer(pMessage);
      CExoString speakerMessage = CExoString.FromPointer(sSpeakerMessage);

      bool skipMessage = ProcessEvent(nChatMessageType, speakerMessage.ToString(), oidSpeaker.ToNwObject<NwGameObject>(), nTellPlayerId.ToNwPlayer());
      if (skipMessage)
      {
        return false.ToInt();
      }

      if (!customHearingDistances)
      {
        return sendServerToPlayerChatMessageHook.CallOriginal(pMessage, nChatMessageType, oidSpeaker, sSpeakerMessage, nTellPlayerId, tellName);
      }

      CExoLinkedListInternal playerList = LowLevel.ServerExoApp.m_pcExoAppInternal.m_pNWSPlayerList.m_pcExoLinkedListInternal;
      if (playerList == null)
      {
        return false.ToInt();
      }

      if (nChatMessageType == ChatChannel.PlayerShout && NwServer.Instance.ServerInfo.PlayOptions.DisallowShouting)
      {
        nChatMessageType = ChatChannel.PlayerTalk;
      }

      if (nChatMessageType != ChatChannel.PlayerTalk &&
        nChatMessageType != ChatChannel.PlayerWhisper &&
        nChatMessageType != ChatChannel.DmTalk &&
        nChatMessageType != ChatChannel.DmWhisper)
      {
        return sendServerToPlayerChatMessageHook.CallOriginal(pMessage, nChatMessageType, oidSpeaker, sSpeakerMessage, nTellPlayerId, tellName);
      }

      NwGameObject speaker = oidSpeaker.ToNwObject<NwGameObject>();

      foreach (NwPlayer player in NwModule.Instance.Players)
      {
        NwCreature controlledCreature = player.ControlledCreature;
        if (controlledCreature == null || controlledCreature.Area != speaker.Area)
        {
          continue;
        }

        float hearDistance = GetChatHearingDistance(player, nChatMessageType);
        if (controlledCreature.DistanceSquared(speaker) > hearDistance * hearDistance)
        {
          continue;
        }

        switch (nChatMessageType)
        {
          case ChatChannel.PlayerTalk:
            message.SendServerToPlayerChat_Talk(player.Player.m_nPlayerID, speaker, speakerMessage);
            break;
          case ChatChannel.DmTalk:
            message.SendServerToPlayerChat_DM_Talk(player.Player.m_nPlayerID, speaker, speakerMessage);
            break;
          case ChatChannel.PlayerWhisper:
            message.SendServerToPlayerChat_Whisper(player.Player.m_nPlayerID, speaker, speakerMessage);
            break;
          case ChatChannel.DmWhisper:
            message.SendServerToPlayerChat_DM_Whisper(player.Player.m_nPlayerID, speaker, speakerMessage);
            break;
        }
      }

      return true.ToInt();
    }
  }
}
