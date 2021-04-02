using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public class OnUnpossessFamiliar : IEvent
  {
    public NwCreature Owner { get; private init; }

    public NwCreature Familiar { get; private init; }

    NwObject IEvent.Context => Owner;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature17UnpossessFamiliarEv)]
    internal delegate void UnpossessFamiliarHook(IntPtr pCreature);

    internal class Factory : NativeEventFactory<UnpossessFamiliarHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<UnpossessFamiliarHook> RequestHook(HookService hookService)
        => hookService.RequestHook<UnpossessFamiliarHook>(OnUnpossessFamiliar, HookOrder.Earliest);

      private void OnUnpossessFamiliar(IntPtr pCreature)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        ProcessEvent(new OnUnpossessFamiliar
        {
          Owner = creature.m_idSelf.ToNwObject<NwCreature>(),
          Familiar = creature.GetAssociateId((ushort)AssociateType.Familiar).ToNwObject<NwCreature>()
        });

        Hook.Original.Invoke(pCreature);
      }
    }
  }
}
