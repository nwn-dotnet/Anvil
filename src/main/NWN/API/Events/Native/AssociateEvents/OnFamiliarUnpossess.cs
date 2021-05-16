using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnFamiliarUnpossess : IEvent
  {
    public NwCreature Owner { get; private init; }

    public NwCreature Familiar { get; private init; }

    NwObject IEvent.Context => Owner;

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
        CNWSCreature creature = new CNWSCreature(pCreature, false);

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
