using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnFamiliarPossess : IEvent
  {
    public NwCreature Familiar { get; private init; } = null!;
    public NwCreature Owner { get; private init; } = null!;

    NwObject IEvent.Context => Owner;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.PossessFamiliar> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void> pHook = &OnPossessFamiliar;
        Hook = HookService.RequestHook<Functions.CNWSCreature.PossessFamiliar>(pHook, HookOrder.Earliest);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static void OnPossessFamiliar(void* pCreature)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnFamiliarPossess eventData = ProcessEvent(EventCallbackType.Before, new OnFamiliarPossess
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
    /// <inheritdoc cref="Events.OnFamiliarPossess"/>
    public event Action<OnFamiliarPossess> OnFamiliarPossess
    {
      add => EventService.Subscribe<OnFamiliarPossess, OnFamiliarPossess.Factory>(this, value);
      remove => EventService.Unsubscribe<OnFamiliarPossess, OnFamiliarPossess.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnFamiliarPossess"/>
    public event Action<OnFamiliarPossess> OnFamiliarPossess
    {
      add => EventService.SubscribeAll<OnFamiliarPossess, OnFamiliarPossess.Factory>(value);
      remove => EventService.UnsubscribeAll<OnFamiliarPossess, OnFamiliarPossess.Factory>(value);
    }
  }
}
