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
    public string CDKey { get; private init; } = null!;

    /// <summary>
    /// Gets the platform of the connecting client.
    /// </summary>
    public PlayerPlatform ClientPlatform { get; private init; }

    /// <summary>
    /// Gets the version of the connecting client.
    /// </summary>
    public Version ClientVersion { get; private init; } = null!;

    /// <summary>
    /// Gets a value indicating whether the client is connecting as a DM (true) or player (false).
    /// </summary>
    public bool DM { get; private init; }

    /// <summary>
    /// Gets the IP address of the connecting client.
    /// </summary>
    public string IP { get; private init; } = null!;

    /// <summary>
    /// Gets or sets the kick message to send to the client if BlockConnection is set to true.
    /// </summary>
    public string? KickMessage { get; set; }

    /// <summary>
    /// Gets the player name of the connecting client.
    /// </summary>
    public string PlayerName { get; private init; } = null!;

    NwObject? IEvent.Context => null;

    internal sealed class Factory : HookEventFactory
    {
      private static readonly CNetLayer NetLayer = LowLevel.ServerExoApp.GetNetLayer();

      private static FunctionHook<SendServerToPlayerCharListHook> Hook { get; set; } = null!;

      internal unsafe delegate int SendServerToPlayerCharListHook(void* pMessage, void* pPlayer);

      protected override unsafe IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int> pHook = &OnSendServerToPlayerCharList;
        Hook = HookService.RequestHook<SendServerToPlayerCharListHook>(pHook, FunctionsLinux._ZN11CNWSMessage26SendServerToPlayerCharListEP10CNWSPlayer, HookOrder.Early);
        return new IDisposable[] { Hook };
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

        OnClientConnect eventData = ProcessEvent(EventCallbackType.Before, new OnClientConnect
        {
          PlayerName = playerInfo.m_sPlayerName.ToString(),
          ClientVersion = new Version(playerInfo.m_nBuildVersion, playerInfo.m_nPatchRevision),
          ClientPlatform = (PlayerPlatform)playerInfo.m_nPlatformId,
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

        int retVal = Hook.CallOriginal(pMessage, pPlayer);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
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
