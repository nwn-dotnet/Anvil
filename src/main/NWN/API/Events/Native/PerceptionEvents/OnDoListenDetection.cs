using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public class OnDoListenDetection : IEvent
  {
    public VisibilityOverride VisibilityOverride { get; set; }

    public NwCreature Creature { get; private init; }

    public NwCreature Target { get; private init; }

    NwObject IEvent.Context => Creature;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature17DoListenDetectionEPS_i)]
    internal delegate int DoListenDetectionHook(IntPtr pCreature, IntPtr pTarget, int bTargetInvisible);

    internal class Factory : NativeEventFactory<DoListenDetectionHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<DoListenDetectionHook> RequestHook(HookService hookService)
        => hookService.RequestHook<DoListenDetectionHook>(OnDoListenDetection, HookOrder.Early);

      private int OnDoListenDetection(IntPtr pCreature, IntPtr pTarget, int bTargetInvisible)
      {
        CNWSCreature target = new CNWSCreature(pTarget, false);
        if (target.m_nStealthMode == 0 && !bTargetInvisible.ToBool())
        {
          return true.ToInt();
        }

        CNWSCreature creature = new CNWSCreature(pCreature, false);

        OnDoListenDetection eventData = ProcessEvent(new OnDoListenDetection
        {
          Creature = creature.m_idSelf.ToNwObject<NwCreature>(),
          Target = target.m_idSelf.ToNwObject<NwCreature>()
        });

        switch (eventData.VisibilityOverride)
        {
          case VisibilityOverride.Visible:
            return true.ToInt();
          case VisibilityOverride.NotVisible:
            return false.ToInt();
          default:
            return Hook.CallOriginal(pCreature, pTarget, bTargetInvisible);
        }
      }
    }
  }
}
