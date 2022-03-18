using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnExamineTrap : IEvent
  {
    public NwPlayer ExaminedBy { get; private init; }

    public NwGameObject ExaminedObject { get; private init; }

    public bool Success { get; private init; }

    NwObject IEvent.Context => ExaminedBy.ControlledCreature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private delegate void TrapExamineHook(void* pMessage, void* pPlayer, uint oidTrap, void* pCreature, int bSuccess);

      private static FunctionHook<TrapExamineHook> Hook { get; set; }

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint, void*, int, void> pHook = &OnExamineTrap;
        Hook = HookService.RequestHook<TrapExamineHook>(pHook, FunctionsLinux._ZN11CNWSMessage37SendServerToPlayerExamineGui_TrapDataEP10CNWSPlayerjP12CNWSCreaturei, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnExamineTrap(void* pMessage, void* pPlayer, uint oidTrap, void* pCreature, int bSuccess)
      {
        ProcessEvent(new OnExamineTrap
        {
          ExaminedBy = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer(),
          ExaminedObject = oidTrap.ToNwObject<NwGameObject>(),
          Success = bSuccess.ToBool(),
        });

        Hook.CallOriginal(pMessage, pPlayer, oidTrap, pCreature, bSuccess);
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
