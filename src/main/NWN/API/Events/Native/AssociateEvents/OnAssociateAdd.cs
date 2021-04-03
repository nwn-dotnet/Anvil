using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnAssociateAdd : IEvent
  {
    public NwCreature Owner { get; private init; }

    public NwCreature Associate { get; private init; }

    NwObject IEvent.Context => Owner;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature12AddAssociateEjt)]
    internal delegate void AddAssociateHook(IntPtr pCreature, uint associate, ushort associateType);

    internal class Factory : NativeEventFactory<AddAssociateHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<AddAssociateHook> RequestHook(HookService hookService)
        => hookService.RequestHook<AddAssociateHook>(OnAddAssociate, HookOrder.Earliest);

      private void OnAddAssociate(IntPtr pCreature, uint associate, ushort associateType)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);

        ProcessEvent(new OnAssociateAdd
        {
          Owner = creature.m_idSelf.ToNwObject<NwCreature>(),
          Associate = associate.ToNwObject<NwCreature>()
        });

        Hook.Original.Invoke(pCreature, associate, associateType);
      }
    }
  }
}
