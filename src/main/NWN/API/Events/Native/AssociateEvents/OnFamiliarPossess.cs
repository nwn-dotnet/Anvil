using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public class OnFamiliarPossess : IEvent
  {
    public NwCreature Owner { get; private init; }

    public NwCreature Familiar { get; private init; }

    NwObject IEvent.Context => Owner;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature15PossessFamiliarEv)]
    internal delegate void PossessFamiliarHook(IntPtr pCreature);

    internal class Factory : NativeEventFactory<PossessFamiliarHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<PossessFamiliarHook> RequestHook(HookService hookService)
        => hookService.RequestHook<PossessFamiliarHook>(OnPossessFamiliar, HookOrder.Earliest);

      private void OnPossessFamiliar(IntPtr pCreature)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        ProcessEvent(new OnFamiliarPossess
        {
          Owner = creature.m_idSelf.ToNwObject<NwCreature>(),
          Familiar = creature.GetAssociateId((ushort)AssociateType.Familiar).ToNwObject<NwCreature>()
        });

        Hook.Original.Invoke(pCreature);
      }
    }
  }
}
