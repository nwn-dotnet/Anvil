using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public class OnDoSpotDetection : IEvent
  {
    public VisibilityOverride VisibilityOverride { get; set; }

    public NwCreature Creature { get; private init; }

    public NwCreature Target { get; private init; }

    NwObject IEvent.Context => Creature;

    [NativeFunction(NWNXLib.Functions._ZN12CNWSCreature15DoSpotDetectionEPS_i)]
    internal delegate int DoSpotDetectionHook(IntPtr pCreature, IntPtr pTarget, int bTargetInvisible);

    internal class Factory : NativeEventFactory<DoSpotDetectionHook>
    {
      public Factory(Lazy<EventService> eventService, HookService hookService) : base(eventService, hookService) {}

      protected override FunctionHook<DoSpotDetectionHook> RequestHook(HookService hookService)
        => hookService.RequestHook<DoSpotDetectionHook>(OnDoSpotDetection, HookOrder.Early);

      private int OnDoSpotDetection(IntPtr pCreature, IntPtr pTarget, int bTargetInvisible)
      {
        CNWSCreature creature = new CNWSCreature(pCreature, false);
        CNWSCreature target = new CNWSCreature(pTarget, false);

        if (bTargetInvisible.ToBool() || creature.GetBlind().ToBool())
        {
          return false.ToInt();
        }

        if (target.m_nStealthMode == 0)
        {
          return true.ToInt();
        }

        OnDoSpotDetection eventData = ProcessEvent(new OnDoSpotDetection
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
