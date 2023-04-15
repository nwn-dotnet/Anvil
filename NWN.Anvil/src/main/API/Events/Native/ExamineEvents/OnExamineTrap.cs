using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnExamineTrap : IEvent
  {
    public NwPlayer ExaminedBy { get; private init; } = null!;

    public NwGameObject ExaminedObject { get; private init; } = null!;

    public bool Success { get; private init; }

    NwObject? IEvent.Context => ExaminedBy.ControlledCreature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_TrapData> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint, void*, int, int> pHook = &OnExamineTrap;
        Hook = HookService.RequestHook<Functions.CNWSMessage.SendServerToPlayerExamineGui_TrapData>(pHook, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnExamineTrap(void* pMessage, void* pPlayer, uint oidTrap, void* pCreature, int bSuccess)
      {
        OnExamineTrap eventData = ProcessEvent(EventCallbackType.Before, new OnExamineTrap
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer()!,
          ExaminedObject = oidTrap.ToNwObject<NwGameObject>()!,
          Success = bSuccess.ToBool(),
        });

        int retVal = Hook.CallOriginal(pMessage, pPlayer, oidTrap, pCreature, bSuccess);
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnExamineTrap"/>
    public event Action<OnExamineTrap> OnExamineTrap
    {
      add => EventService.Subscribe<OnExamineTrap, OnExamineTrap.Factory>(ControlledCreature, value);
      remove => EventService.Unsubscribe<OnExamineTrap, OnExamineTrap.Factory>(ControlledCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnExamineTrap"/>
    public event Action<OnExamineTrap> OnExamineTrap
    {
      add => EventService.SubscribeAll<OnExamineTrap, OnExamineTrap.Factory>(value);
      remove => EventService.UnsubscribeAll<OnExamineTrap, OnExamineTrap.Factory>(value);
    }
  }
}
