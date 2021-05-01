using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnClientLevelUpBegin : IEvent
  {
    public NwPlayer Player { get; private init; }

    public bool PreventLevelUp { get; set; }

    public Lazy<bool> Result { get; private set; }

    NwObject IEvent.Context => Player;

    [NativeFunction(NWNXLib.Functions._ZN11CNWSMessage34HandlePlayerToServerLevelUpMessageEP10CNWSPlayerh)]
    internal delegate int HandlePlayerToServerLevelUpMessageHook(IntPtr pMessage, IntPtr pPlayer, byte nMinor);

    internal class Factory : NativeEventFactory<HandlePlayerToServerLevelUpMessageHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<HandlePlayerToServerLevelUpMessageHook> RequestHook(HookService hookService)
        => hookService.RequestHook<HandlePlayerToServerLevelUpMessageHook>(OnHandlePlayerToServerLevelUpMessage, HookOrder.Early);

      private int OnHandlePlayerToServerLevelUpMessage(IntPtr pMessage, IntPtr pPlayer, byte nMinor)
      {
        if (nMinor != (byte)MessageLevelUpMinor.Begin)
        {
          return Hook.CallOriginal(pMessage, pPlayer, nMinor);
        }

        CNWSPlayer player = new CNWSPlayer(pPlayer, false);

        OnClientLevelUpBegin eventData = new OnClientLevelUpBegin
        {
          Player = new NwPlayer(player),
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventLevelUp && Hook.CallOriginal(pMessage, pPlayer, nMinor).ToBool());
        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
      }
    }
  }
}
