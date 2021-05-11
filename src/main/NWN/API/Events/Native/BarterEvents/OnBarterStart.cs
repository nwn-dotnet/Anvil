using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnBarterStart : IEvent
  {
    public NwPlayer Initiator { get; private init; }

    public NwPlayer Target { get; private init; }

    NwObject IEvent.Context => Initiator;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.StartBarterHook>
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
          Initiator = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          Target = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwObject<NwPlayer>()
        });

        Hook.CallOriginal(pMessage, pPlayer);
      }
    }
  }
}
