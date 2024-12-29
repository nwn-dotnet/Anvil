using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnFamiliarUnpossess : IEvent
  {
    public NwCreature Familiar { get; private init; } = null!;
    public NwCreature Owner { get; private init; } = null!;

    NwObject IEvent.Context => Owner;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.UnpossessFamiliar> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void> pHook = &OnUnpossessFamiliar;
        Hook = HookService.RequestHook<Functions.CNWSCreature.UnpossessFamiliar>(pHook, HookOrder.Earliest);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static void OnUnpossessFamiliar(void* pCreature)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnFamiliarUnpossess eventData = ProcessEvent(EventCallbackType.Before, new OnFamiliarUnpossess
        {
          Owner = creature.ToNwObject<NwCreature>()!,
          Familiar = creature.GetAssociateId((ushort)AssociateType.Familiar).ToNwObject<NwCreature>()!,
        });

        Hook.CallOriginal(pCreature);
        ProcessEvent(EventCallbackType.After, eventData);
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
