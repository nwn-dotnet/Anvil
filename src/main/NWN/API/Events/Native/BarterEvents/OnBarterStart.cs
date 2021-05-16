using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnBarterStart : IEvent
  {
    public NwPlayer Initiator { get; private init; }

    public NwPlayer Target { get; private init; }

    NwObject IEvent.Context => Initiator.ControlledCreature;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.StartBarterHook>
    {
      internal delegate void StartBarterHook(void* pMessage, void* pPlayer);

      protected override FunctionHook<StartBarterHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, void> pHook = &OnStartBarter;
        return HookService.RequestHook<StartBarterHook>(pHook, FunctionsLinux._ZN11CNWSMessage38HandlePlayerToServerBarter_StartBarterEP10CNWSPlayer, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnStartBarter(void* pMessage, void* pPlayer)
      {
        CNWSMessage message = new CNWSMessage(pMessage, false);

        ProcessEvent(new OnBarterStart
        {
          Initiator = new CNWSPlayer(pPlayer, false).ToNwPlayer(),
          Target = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwPlayer(),
        });

        Hook.CallOriginal(pMessage, pPlayer);
      }
    }
  }
}
