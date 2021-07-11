using System.Collections.Generic;
using Anvil.API;
using Anvil.Internal;
using NWN.Native.API;

namespace Anvil.Services
{
  [ServiceBinding(typeof(ChatService))]
  public sealed unsafe partial class ChatService
  {
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

    public ChatService(HookService hookService)
    {
      sendServerToPlayerChatMessageHook = hookService.RequestHook<SendServerToPlayerChatMessageHook>(OnSendServerToPlayerChatMessage, FunctionsLinux._ZN11CNWSMessage29SendServerToPlayerChatMessageEhj10CExoStringjRKS0_, HookOrder.Late);
    }

    /// <summary>
    /// Sends a message as the specified creature.
    /// </summary>
    /// <param name="chatChannel">The <see cref="ChatChannel"/> to send the message.</param>
    /// <param name="message">The message to send.</param>
    /// <param name="sender">The sender of the message.</param>
    /// <param name="target">The receiver of the message.</param>
    /// <returns>True if the message was sent successfully, otherwise false.</returns>
    public bool SendMessage(ChatChannel chatChannel, string message, NwCreature sender, NwPlayer target = null)
    {
      return SendMessage(chatChannel, message, (NwObject)sender, target);
    }

    /// <summary>
    /// Sends a message from the "Server".
    /// </summary>
    /// <param name="chatChannel">The <see cref="ChatChannel"/> to send the message. Only works with ServerMessage and Player/DMShout</param>
    /// <param name="message">The message to send.</param>
    /// <param name="target">The receiver of the message.</param>
    /// <returns>True if the message was sent successfully, otherwise false.</returns>
    public bool SendServerMessage(ChatChannel chatChannel, string message, NwPlayer target = null)
    {
      return SendMessage(chatChannel, message, NwModule.Instance, target);
    }

    /// <summary>
    /// Gets the global hearing distance for the specified <see cref="ChatChannel"/>.
    /// </summary>
    /// <param name="chatChannel">The <see cref="ChatChannel"/> to query.</param>
    /// <returns>The distance configured for the specified chat channel.</returns>
    public float GetChatHearingDistance(ChatChannel chatChannel)
    {
      globalHearingDistances.TryGetValue(chatChannel, out float retVal);
      return retVal;
    }

    /// <summary>
    /// Gets the hearing distance for the specified <see cref="NwPlayer"/> and <see cref="ChatChannel"/>.
    /// </summary>
    /// <param name="player">The <see cref="NwPlayer"/> to query.</param>
    /// <param name="chatChannel">The <see cref="ChatChannel"/> to query.</param>
    /// <returns>The distance configured for the specified player and chat channel.<br/>
    /// If no override is configured, returns the global configured distance instead.</returns>
    public float GetPlayerChatHearingDistance(NwPlayer player, ChatChannel chatChannel)
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

    /// <summary>
    /// Sets the global hearing distance for the specified <see cref="ChatChannel"/>.
    /// </summary>
    /// <param name="chatChannel">The <see cref="ChatChannel"/> to query.</param>
    /// <param name="distance">The new hearing distance.</param>
    public void SetChatHearingDistance(ChatChannel chatChannel, float distance)
    {
      globalHearingDistances[chatChannel] = distance;
      customHearingDistances = true;
    }

    /// <summary>
    /// Sets the hearing distance override for the specified <see cref="NwPlayer"/> and <see cref="ChatChannel"/>.
    /// </summary>
    /// <param name="player">The <see cref="NwPlayer"/> to query.</param>
    /// <param name="chatChannel">The <see cref="ChatChannel"/> to query.</param>
    /// <param name="distance">The new hearing distance.</param>
    public void SetPlayerChatHearingDistance(NwPlayer player, ChatChannel chatChannel, float distance)
    {
      if (!playerHearingDistances.TryGetValue(player, out Dictionary<ChatChannel, float> playerHearingDistance))
      {
        playerHearingDistance = new Dictionary<ChatChannel, float>();
        playerHearingDistances[player] = playerHearingDistance;
      }

      playerHearingDistance[chatChannel] = distance;
      customHearingDistances = true;
    }

    /// <summary>
    /// Clears the hearing distance override for the specified <see cref="NwPlayer"/> and <see cref="ChatChannel"/>.
    /// </summary>
    /// <param name="player">The <see cref="NwPlayer"/> to query.</param>
    /// <param name="chatChannel">The <see cref="ChatChannel"/> to query.</param>
    public void ClearPlayerChatHearingDistance(NwPlayer player, ChatChannel chatChannel)
    {
      if (playerHearingDistances.TryGetValue(player, out Dictionary<ChatChannel, float> playerHearingDistance))
      {
        playerHearingDistance.Remove(chatChannel);
      }
    }

    /// <summary>
    /// Clears any hearing distance overrides for the specified <see cref="NwPlayer"/>.
    /// </summary>
    /// <param name="player">The <see cref="NwPlayer"/> to query.</param>
    public void ClearPlayerChatHearingDistance(NwPlayer player)
    {
      playerHearingDistances.Remove(player);
    }

    private bool SendMessage(ChatChannel chatChannel, string message, NwObject speaker, NwPlayer target)
    {
      uint playerId = target != null ? target.Player.m_nPlayerID : PlayerIdConstants.AllClients;
      if (playerId == PlayerIdConstants.Invalid)
      {
        return false;
      }

      CNWSMessage messageDispatch = LowLevel.ServerExoApp.GetNWSMessage();

      if (target != null)
      {
        // This means we're sending this to one player only.
        // The normal function broadcasts in an area for talk, shout, and whisper, therefore
        // we need to call these functions directly if we are in those categories.
        switch (chatChannel)
        {
          case ChatChannel.PlayerTalk:
            return messageDispatch.SendServerToPlayerChat_Talk(playerId, speaker, message.ToExoString()).ToBool();
          case ChatChannel.DmTalk:
            return messageDispatch.SendServerToPlayerChat_DM_Talk(playerId, speaker, message.ToExoString()).ToBool();
          case ChatChannel.DmDm:
          case ChatChannel.PlayerDm:
            return messageDispatch.SendServerToPlayerChat_DM_Silent_Shout(playerId, speaker, message.ToExoString()).ToBool();
          case ChatChannel.PlayerShout:
          case ChatChannel.DmShout:
            return messageDispatch.SendServerToPlayerChat_Shout(playerId, speaker, message.ToExoString()).ToBool();
          case ChatChannel.PlayerWhisper:
            return messageDispatch.SendServerToPlayerChat_Whisper(playerId, speaker, message.ToExoString()).ToBool();
          case ChatChannel.DmWhisper:
            return messageDispatch.SendServerToPlayerChat_DM_Whisper(playerId, speaker, message.ToExoString()).ToBool();
          case ChatChannel.PlayerParty:
          case ChatChannel.DmParty:
            return messageDispatch.SendServerToPlayerChat_Party(playerId, speaker, message.ToExoString()).ToBool();
        }
      }

      return messageDispatch.SendServerToPlayerChatMessage((byte)chatChannel, speaker, message.ToExoString(), playerId).ToBool();
    }

    private int OnSendServerToPlayerChatMessage(void* pMessage, ChatChannel nChatMessageType, uint oidSpeaker, void* sSpeakerMessage, uint nTellPlayerId, void* tellName)
    {
      if (!isEventHooked && !customHearingDistances)
      {
        return sendServerToPlayerChatMessageHook.CallOriginal(pMessage, nChatMessageType, oidSpeaker, sSpeakerMessage, nTellPlayerId, tellName);
      }

      CNWSMessage message = CNWSMessage.FromPointer(pMessage);
      CExoString speakerMessage = CExoString.FromPointer(sSpeakerMessage);
      NwObject speaker = oidSpeaker.ToNwObject();

      bool skipMessage = ProcessEvent(nChatMessageType, speakerMessage.ToString(), speaker, nTellPlayerId.ToNwPlayer());
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

      NwGameObject speakerGameObject = speaker as NwGameObject;
      NwArea speakerArea = speakerGameObject?.Area;

      foreach (NwPlayer player in NwModule.Instance.Players)
      {
        NwCreature controlledCreature = player.ControlledCreature;

        if (controlledCreature == null || speakerArea != null && speakerArea != controlledCreature.Area)
        {
          continue;
        }

        float hearDistance = GetPlayerChatHearingDistance(player, nChatMessageType);
        if (speakerGameObject != null && controlledCreature.DistanceSquared(speakerGameObject) > hearDistance * hearDistance)
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
