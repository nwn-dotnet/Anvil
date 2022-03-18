using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnFamiliarUnpossess : IEvent
  {
    public NwCreature Familiar { get; private init; }
    public NwCreature Owner { get; private init; }

    NwObject IEvent.Context => Owner;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<UnpossessFamiliarHook> Hook { get; set; }

      private delegate void UnpossessFamiliarHook(void* pCreature);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void> pHook = &OnUnpossessFamiliar;
        Hook = HookService.RequestHook<UnpossessFamiliarHook>(pHook, FunctionsLinux._ZN12CNWSCreature17UnpossessFamiliarEv, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnUnpossessFamiliar(void* pCreature)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        ProcessEvent(new OnFamiliarUnpossess
        {
          Owner = creature.ToNwObject<NwCreature>(),
          Familiar = creature.GetAssociateId((ushort)AssociateType.Familiar).ToNwObject<NwCreature>(),
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
