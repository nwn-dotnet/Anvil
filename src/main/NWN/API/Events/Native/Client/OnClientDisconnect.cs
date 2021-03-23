using System;
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
    public NwPlayer Player { get; }

    NwObject IEvent.Context => Player;

    private OnClientDisconnect(CNWSPlayer player)
    {
      Player = player.m_oidPCObject == NwObject.INVALID ? null : new NwPlayer(player);
    }

    [NativeFunction(NWNXLib.Functions._ZN21CServerExoAppInternal17RemovePCFromWorldEP10CNWSPlayer)]
    internal delegate void RemovePCFromWorldHook(IntPtr pServerExoAppInternal, IntPtr pPlayer);

    internal class Factory : NativeEventFactory<RemovePCFromWorldHook>
    {
      public Factory(HookService hookService) : base(hookService) {}

      protected override int FunctionHookOrder { get; } = HookOrder.Earliest;

      protected override RemovePCFromWorldHook Handler => OnRemovePCFromWorld;

      private void OnRemovePCFromWorld(IntPtr pServerExoAppInternal, IntPtr pPlayer)
      {
        ProcessEvent(new OnClientDisconnect(new CNWSPlayer(pPlayer, false)));
        FunctionHook.Original.Invoke(pServerExoAppInternal, pPlayer);
      }
    }
  }
}
