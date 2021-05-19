using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnFamiliarPossess : IEvent
  {
    public NwCreature Owner { get; private init; }

    public NwCreature Familiar { get; private init; }

    NwObject IEvent.Context => Owner;

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.PossessFamiliarHook>
    {
      internal delegate void PossessFamiliarHook(void* pCreature);

      protected override FunctionHook<PossessFamiliarHook> RequestHook()
      {
        delegate* unmanaged<void*, void> pHook = &OnPossessFamiliar;
        return HookService.RequestHook<PossessFamiliarHook>(pHook, FunctionsLinux._ZN12CNWSCreature15PossessFamiliarEv, HookOrder.Earliest);
      }

      [UnmanagedCallersOnly]
      private static void OnPossessFamiliar(void* pCreature)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        ProcessEvent(new OnFamiliarPossess
        {
          Owner = creature.ToNwObject<NwCreature>(),
          Familiar = creature.GetAssociateId((ushort)AssociateType.Familiar).ToNwObject<NwCreature>()
        });

        Hook.CallOriginal(pCreature);
      }
    }
  }
}
