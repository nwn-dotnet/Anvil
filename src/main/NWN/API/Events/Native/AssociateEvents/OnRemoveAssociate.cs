using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public class OnRemoveAssociate : IEvent
  {
    public NwCreature Owner { get; private init; }

    public NwCreature Associate { get; private init; }

    NwObject IEvent.Context => Owner;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature15RemoveAssociateEj)]
    internal delegate void RemoveAssociateHook(IntPtr pCreature, uint associate);

    internal class Factory : NativeEventFactory<RemoveAssociateHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<RemoveAssociateHook> RequestHook(HookService hookService)
        => hookService.RequestHook<RemoveAssociateHook>(OnRemoveAssociate, HookOrder.Earliest);

      private void OnRemoveAssociate(IntPtr pCreature, uint associate)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        ProcessEvent(new OnRemoveAssociate
        {
          Owner = creature.m_idSelf.ToNwObject<NwCreature>(),
          Associate = associate.ToNwObject<NwCreature>()
        });

        Hook.Original.Invoke(pCreature, associate);
      }
    }
  }
}
