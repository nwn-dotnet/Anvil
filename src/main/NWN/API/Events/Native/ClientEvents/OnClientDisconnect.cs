using System.Runtime.InteropServices;
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
    /// Gets the player that disconnected. Returns null if the player disconnected on the character screen.
    /// </summary>
    public NwPlayer Player { get; private init; }

    NwObject IEvent.Context => Player;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.RemovePCFromWorldHook>
    {
      internal delegate void RemovePCFromWorldHook(void* pServerExoAppInternal, void* pPlayer);

      protected override FunctionHook<RemovePCFromWorldHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void> pHook = &OnRemovePCFromWorld;
        return HookService.RequestHook<RemovePCFromWorldHook>(NWNXLib.Functions._ZN21CServerExoAppInternal17RemovePCFromWorldEP10CNWSPlayer, pHook, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnRemovePCFromWorld(void* pServerExoAppInternal, void* pPlayer)
      {
        CNWSPlayer player = new CNWSPlayer(pPlayer, false);

        ProcessEvent(new OnClientDisconnect
        {
          Player = player.m_oidPCObject == NwObject.INVALID ? null : new NwPlayer(player)
        });

        Hook.CallOriginal(pServerExoAppInternal, pPlayer);
      }
    }
  }
}
