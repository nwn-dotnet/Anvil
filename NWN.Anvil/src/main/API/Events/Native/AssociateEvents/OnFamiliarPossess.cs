using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnFamiliarPossess : IEvent
  {
    public NwCreature Familiar { get; private init; }
    public NwCreature Owner { get; private init; }

    NwObject? IEvent.Context => Owner;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<PossessFamiliarHook> Hook { get; set; } = null!;

      private delegate void PossessFamiliarHook(void* pCreature);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void> pHook = &OnPossessFamiliar;
        Hook = HookService.RequestHook<PossessFamiliarHook>(pHook, FunctionsLinux._ZN12CNWSCreature15PossessFamiliarEv, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnPossessFamiliar(void* pCreature)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        ProcessEvent(new OnFamiliarPossess
        {
          Owner = creature.ToNwObject<NwCreature>()!,
          Familiar = creature.GetAssociateId((ushort)AssociateType.Familiar).ToNwObject<NwCreature>()!,
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
