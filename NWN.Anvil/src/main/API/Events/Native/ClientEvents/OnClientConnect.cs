using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Internal;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a player attempts to connect to the server (before character select).<br/>
  /// This event can be cancelled to prevent a player from connecting to the server.
  /// </summary>
  public sealed class OnClientConnect : IEvent
  {
    /// <summary>
    /// Gets or sets a value indicating whether this client connection should be prevented.
    /// </summary>
    public bool BlockConnection { get; set; }

    /// <summary>
    /// Gets the public CD Key of the connecting client.
    /// </summary>
    public string CDKey { get; private init; }

    /// <summary>
    /// Gets a value indicating whether the client is connecting as a DM (true) or player (false).
    /// </summary>
    public bool DM { get; private init; }

    /// <summary>
    /// Gets the IP address of the connecting client.
    /// </summary>
    public string IP { get; private init; }

    /// <summary>
    /// Gets or sets the kick message to send to the client if BlockConnection is set to true.
    /// </summary>
    public string KickMessage { get; set; }

    /// <summary>
    /// Gets the player name of the connecting client.
    /// </summary>
    public string PlayerName { get; private init; }

    NwObject IEvent.Context => null;

    internal sealed class Factory : SingleHookEventFactory<Factory.SendServerToPlayerCharListHook>
    {
      private static readonly CNetLayer NetLayer = LowLevel.ServerExoApp.GetNetLayer();

      internal unsafe delegate int SendServerToPlayerCharListHook(void* pMessage, void* pPlayer);

      protected override unsafe FunctionHook<SendServerToPlayerCharListHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, int> pHook = &OnSendServerToPlayerCharList;
        return HookService.RequestHook<SendServerToPlayerCharListHook>(pHook, FunctionsLinux._ZN11CNWSMessage26SendServerToPlayerCharListEP10CNWSPlayer, HookOrder.Early);
      }

      private static async void DelayDisconnectPlayer(uint playerId, string kickMessage)
      {
        await NwTask.NextFrame();

        CNetLayerPlayerInfo playerInfo = NetLayer.GetPlayerInfo(playerId);
        if (playerInfo != null)
        {
          NetLayer.DisconnectPlayer(playerId, 5838, true.ToInt(), kickMessage.ToExoString());
        }
      }

      [UnmanagedCallersOnly]
      private static unsafe int OnSendServerToPlayerCharList(void* pMessage, void* pPlayer)
      {
        CNWSPlayer player = CNWSPlayer.FromPointer(pPlayer);
        if (player == null)
        {
          return Hook.CallOriginal(pMessage, pPlayer);
        }

        uint playerId = player.m_nPlayerID;

        CNetLayerPlayerInfo playerInfo = NetLayer.GetPlayerInfo(playerId);
        string ipAddress = NetLayer.GetPlayerAddress(playerId).ToString();

        OnClientConnect eventData = ProcessEvent(new OnClientConnect
        {
          PlayerName = playerInfo.m_sPlayerName.ToString(),
          CDKey = playerInfo.m_lstKeys[0].sPublic.ToString(),
          DM = playerInfo.m_bGameMasterPrivileges.ToBool(),
          IP = ipAddress,
        });

        if (!eventData.BlockConnection)
        {
          return Hook.CallOriginal(pMessage, pPlayer);
        }

        string kickMessage = eventData.KickMessage ?? string.Empty;
        DelayDisconnectPlayer(playerId, kickMessage);
        return Hook.CallOriginal(pMessage, pPlayer);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnClientConnect"/>
    public event Action<OnClientConnect> OnClientConnect
    {
      add => EventService.SubscribeAll<OnClientConnect, OnClientConnect.Factory>(value);
      remove => EventService.UnsubscribeAll<OnClientConnect, OnClientConnect.Factory>(value);
    }
  }
}
