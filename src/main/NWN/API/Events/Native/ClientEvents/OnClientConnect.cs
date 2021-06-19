using System;
using System.Runtime.InteropServices;
using Anvil.Internal;
using NWN.API.Events;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  /// <summary>
  /// Triggered when a player attempts to connect to the server (before character select).<br/>
  /// This event can be cancelled to prevent a player from connecting to the server.
  /// </summary>
  public sealed class OnClientConnect : IEvent
  {
    /// <summary>
    /// Gets the player name of the connecting client.
    /// </summary>
    public string PlayerName { get; private init; }

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
    /// Gets or sets a value indicating whether this client connection should be prevented.
    /// </summary>
    public bool BlockConnection { get; set; }

    /// <summary>
    /// Gets or sets the kick message to send to the client if BlockConnection is set to true.
    /// </summary>
    public string KickMessage { get; set; }

    NwObject IEvent.Context
    {
      get => null;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.SendServerToPlayerCharListHook>
    {
      internal delegate int SendServerToPlayerCharListHook(void* pMessage, void* pPlayer);

      private static readonly CNetLayer NetLayer = LowLevel.ServerExoApp.GetNetLayer();

      protected override FunctionHook<SendServerToPlayerCharListHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, int> pHook = &OnSendServerToPlayerCharList;
        return HookService.RequestHook<SendServerToPlayerCharListHook>(pHook, FunctionsLinux._ZN11CNWSMessage26SendServerToPlayerCharListEP10CNWSPlayer, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnSendServerToPlayerCharList(void* pMessage, void* pPlayer)
      {
        CNWSPlayer player = CNWSPlayer.FromPointer(pPlayer);
        uint playerId = player.m_nPlayerID;

        CNetLayerPlayerInfo playerInfo = NetLayer.GetPlayerInfo(playerId);
        string ipAddress = NetLayer.GetPlayerAddress(playerId).ToString();

        OnClientConnect eventData = ProcessEvent(new OnClientConnect
        {
          PlayerName = playerInfo.m_sPlayerName.ToString(),
          CDKey = playerInfo.m_lstKeys._OpIndex(0).ToString(),
          DM = playerInfo.m_bGameMasterPrivileges.ToBool(),
          IP = ipAddress,
        });

        if (!eventData.BlockConnection)
        {
          return Hook.CallOriginal(pMessage, pPlayer);
        }

        string kickMessage = eventData.KickMessage ?? string.Empty;
        NetLayer.DisconnectPlayer(playerId, 5838, true.ToInt(), kickMessage.ToExoString());
        return false.ToInt();
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnClientConnect"/>
    public event Action<OnClientConnect> OnClientConnect
    {
      add => EventService.SubscribeAll<OnClientConnect, OnClientConnect.Factory>(value);
      remove => EventService.UnsubscribeAll<OnClientConnect, OnClientConnect.Factory>(value);
    }
  }
}
