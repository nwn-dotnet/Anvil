using System;
using System.Runtime.InteropServices;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnDoListenDetection : IEvent
  {
    public VisibilityOverride VisibilityOverride { get; set; }

    public NwCreature Creature { get; private init; }

    public NwCreature Target { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : NativeEventFactory<Factory.DoListenDetectionHook>
    {
      internal delegate int DoListenDetectionHook(void* pCreature, void* pTarget, int bTargetInvisible);

      protected override FunctionHook<DoListenDetectionHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, int, int> pHook = &OnDoListenDetection;
        return HookService.RequestHook<DoListenDetectionHook>(NWNXLib.Functions._ZN12CNWSCreature17DoListenDetectionEPS_i, pHook, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnDoListenDetection(void* pCreature, void* pTarget, int bTargetInvisible)
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
