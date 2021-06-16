using System;
using System.Runtime.InteropServices;
using NWN.API.Events;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnClientLevelUpBegin : IEvent
  {
    public NwPlayer Player { get; private init; }

    public bool PreventLevelUp { get; set; }

    public Lazy<bool> Result { get; private set; }

    NwObject IEvent.Context
    {
      get => Player.ControlledCreature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.HandlePlayerToServerLevelUpMessageHook>
    {
      internal delegate int HandlePlayerToServerLevelUpMessageHook(void* pMessage, void* pPlayer, byte nMinor);

      protected override FunctionHook<HandlePlayerToServerLevelUpMessageHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, byte, int> pHook = &OnHandlePlayerToServerLevelUpMessage;
        return HookService.RequestHook<HandlePlayerToServerLevelUpMessageHook>(pHook, FunctionsLinux._ZN11CNWSMessage34HandlePlayerToServerLevelUpMessageEP10CNWSPlayerh, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnHandlePlayerToServerLevelUpMessage(void* pMessage, void* pPlayer, byte nMinor)
      {
        if (nMinor != (byte)MessageLevelUpMinor.Begin)
        {
          return Hook.CallOriginal(pMessage, pPlayer, nMinor);
        }

        OnClientLevelUpBegin eventData = new OnClientLevelUpBegin
        {
          Player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer(),
        };

        eventData.Result = new Lazy<bool>(() => !eventData.PreventLevelUp && Hook.CallOriginal(pMessage, pPlayer, nMinor).ToBool());
        ProcessEvent(eventData);

        return eventData.Result.Value.ToInt();
      }
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnClientLevelUpBegin"/>
    public event Action<OnClientLevelUpBegin> OnClientLevelUpBegin
    {
      add => EventService.Subscribe<OnClientLevelUpBegin, OnClientLevelUpBegin.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnClientLevelUpBegin, OnClientLevelUpBegin.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnClientLevelUpBegin"/>
    public event Action<OnClientLevelUpBegin> OnClientLevelUpBegin
    {
      add => EventService.SubscribeAll<OnClientLevelUpBegin, OnClientLevelUpBegin.Factory>(value);
      remove => EventService.UnsubscribeAll<OnClientLevelUpBegin, OnClientLevelUpBegin.Factory>(value);
    }
  }
}
