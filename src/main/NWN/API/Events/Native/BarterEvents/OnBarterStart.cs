using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnBarterStart : IEvent
  {
    public NwPlayer Initiator { get; private init; }

    public NwPlayer Target { get; private init; }

    NwObject IEvent.Context => Initiator;

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage38HandlePlayerToServerBarter_StartBarterEP10CNWSPlayer)]
    internal delegate void StartBarterHook(IntPtr pMessage, IntPtr pPlayer);

    internal class Factory : NativeEventFactory<StartBarterHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<StartBarterHook> RequestHook(HookService hookService)
        => hookService.RequestHook<StartBarterHook>(OnStartBarter, HookOrder.Earliest);

      private void OnStartBarter(IntPtr pMessage, IntPtr pPlayer)
      {
        CNWSMessage message = new CNWSMessage(pMessage, false);

        ProcessEvent(new OnBarterStart()
        {
          Initiator = new NwPlayer(new CNWSPlayer(pPlayer, false)),
          Target = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwObject<NwPlayer>()
        });

        Hook.CallOriginal(pMessage, pPlayer);
      }
    }
  }
}
