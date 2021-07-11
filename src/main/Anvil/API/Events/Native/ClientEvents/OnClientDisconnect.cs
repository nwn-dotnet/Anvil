using System;
using System.Runtime.InteropServices;
using NWN.API.Events;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  /// <summary>
  /// Called when the player disconnects from the server.<br/>
  /// This event is also called if the player connects, and then disconnects on the character select screen.
  /// </summary>
  public sealed class OnClientDisconnect : IEvent
  {
    /// <summary>
    /// Gets the player that disconnected.
    /// </summary>
    public NwPlayer Player { get; private init; }

    NwObject IEvent.Context
    {
      get => Player.ControlledCreature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.RemovePCFromWorldHook>
    {
      internal delegate void RemovePCFromWorldHook(void* pServerExoAppInternal, void* pPlayer);

      protected override FunctionHook<RemovePCFromWorldHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void> pHook = &OnRemovePCFromWorld;
        return HookService.RequestHook<RemovePCFromWorldHook>(pHook, FunctionsLinux._ZN21CServerExoAppInternal17RemovePCFromWorldEP10CNWSPlayer, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnRemovePCFromWorld(void* pServerExoAppInternal, void* pPlayer)
      {
        ProcessEvent(new OnClientDisconnect
        {
          Player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer(),
        });

        Hook.CallOriginal(pServerExoAppInternal, pPlayer);
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnClientDisconnect"/>
    public event Action<OnClientDisconnect> OnClientDisconnect
    {
      add => EventService.Subscribe<OnClientDisconnect, OnClientDisconnect.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnClientDisconnect, OnClientDisconnect.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnClientDisconnect"/>
    public event Action<OnClientDisconnect> OnClientDisconnect
    {
      add => EventService.SubscribeAll<OnClientDisconnect, OnClientDisconnect.Factory>(value);
      remove => EventService.UnsubscribeAll<OnClientDisconnect, OnClientDisconnect.Factory>(value);
    }
  }
}
