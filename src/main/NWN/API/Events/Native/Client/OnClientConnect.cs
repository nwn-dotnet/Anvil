using System;
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
      public string PlayerName { get; }

      /// <summary>
      /// Gets the public CD Key of the connecting client.
      /// </summary>
      public string CDKey { get; }

      /// <summary>
      /// Gets a value indicating whether the client is connecting as a DM (true) or player (false).
      /// </summary>
      public bool DM { get; }

      /// <summary>
      /// Gets the IP address of the connecting client.
      /// </summary>
      public string IP { get; }

      /// <summary>
      /// Gets or sets a value indicating whether this client connection should be prevented.
      /// </summary>
      public bool BlockConnection { get; set; }

      /// <summary>
      /// Gets or sets the kick message to send to the client if BlockConnection is set to true.
      /// </summary>
      public string KickMessage { get; set; }

      NwObject IEvent.Context => null;

      private OnClientConnect(CNetLayerPlayerInfo playerInfo, string ipAddress)
      {
        PlayerName = playerInfo.m_sPlayerName.ToString();
        CDKey = playerInfo.m_lstKeys._OpIndex(0).ToString();
        DM = playerInfo.m_bGameMasterPrivileges.ToBool();
        IP = ipAddress;
      }

      [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage26SendServerToPlayerCharListEP10CNWSPlayer)]
      internal delegate int SendServerToPlayerCharListHook(IntPtr pThis, IntPtr pPlayer);

      internal class Factory : NativeEventFactory<SendServerToPlayerCharListHook>
      {
        private static readonly CNetLayer NetLayer = LowLevel.ServerExoApp.GetNetLayer();

        public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

        protected override FunctionHook<SendServerToPlayerCharListHook> RequestHook(HookService hookService)
          => hookService.RequestHook<SendServerToPlayerCharListHook>(OnSendServerToPlayerCharList, HookOrder.Early);

        private int OnSendServerToPlayerCharList(IntPtr pThis, IntPtr pPlayer)
        {
          CNWSPlayer player = new CNWSPlayer(pPlayer, false);
          uint playerId = player.m_nPlayerID;

          OnClientConnect eventData = new OnClientConnect(NetLayer.GetPlayerInfo(playerId), NetLayer.GetPlayerAddress(playerId).ToString());
          ProcessEvent(eventData);

          if (!eventData.BlockConnection)
          {
            return Hook.Original.Invoke(pThis, pPlayer);
          }

          string kickMessage = eventData.KickMessage ?? string.Empty;
          NetLayer.DisconnectPlayer(playerId, 5838, true.ToInt(), kickMessage.ToExoString());
          return false.ToInt();
        }
      }
    }
}
