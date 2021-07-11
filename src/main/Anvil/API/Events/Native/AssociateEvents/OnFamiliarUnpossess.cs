using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnFamiliarUnpossess : IEvent
  {
    public NwCreature Owner { get; private init; }

    public NwCreature Familiar { get; private init; }

    NwObject IEvent.Context
    {
      get => Owner;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.UnpossessFamiliarHook>
    {
      internal delegate void UnpossessFamiliarHook(void* pCreature);

      protected override FunctionHook<UnpossessFamiliarHook> RequestHook()
      {
        delegate* unmanaged<void*, void> pHook = &OnUnpossessFamiliar;
        return HookService.RequestHook<UnpossessFamiliarHook>(pHook, FunctionsLinux._ZN12CNWSCreature17UnpossessFamiliarEv, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnUnpossessFamiliar(void* pCreature)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        ProcessEvent(new OnFamiliarUnpossess
        {
          Owner = NativeObjectExtensions.ToNwObject<NwCreature>(creature),
          Familiar = IntegerExtensions.ToNwObject<NwCreature>(creature.GetAssociateId((ushort)AssociateType.Familiar)),
        });

        Hook.CallOriginal(pCreature);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnFamiliarUnpossess"/>
    public event Action<OnFamiliarUnpossess> OnFamiliarUnpossess
    {
      add => EventService.Subscribe<OnFamiliarUnpossess, OnFamiliarUnpossess.Factory>(this, value);
      remove => EventService.Unsubscribe<OnFamiliarUnpossess, OnFamiliarUnpossess.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnFamiliarUnpossess"/>
    public event Action<OnFamiliarUnpossess> OnFamiliarUnpossess
    {
      add => EventService.SubscribeAll<OnFamiliarUnpossess, OnFamiliarUnpossess.Factory>(value);
      remove => EventService.UnsubscribeAll<OnFamiliarUnpossess, OnFamiliarUnpossess.Factory>(value);
    }
  }
}
