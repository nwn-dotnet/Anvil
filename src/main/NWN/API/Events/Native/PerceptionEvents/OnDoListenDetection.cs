using System;
using System.Runtime.InteropServices;
using NWN.API.Events;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public sealed class OnDoListenDetection : IEvent
  {
    public VisibilityOverride VisibilityOverride { get; set; }

    public NwCreature Creature { get; private init; }

    public NwCreature Target { get; private init; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.DoListenDetectionHook>
    {
      internal delegate int DoListenDetectionHook(void* pCreature, void* pTarget, int bTargetInvisible);

      protected override FunctionHook<DoListenDetectionHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, int, int> pHook = &OnDoListenDetection;
        return HookService.RequestHook<DoListenDetectionHook>(pHook, FunctionsLinux._ZN12CNWSCreature17DoListenDetectionEPS_i, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnDoListenDetection(void* pCreature, void* pTarget, int bTargetInvisible)
      {
        CNWSCreature target = CNWSCreature.FromPointer(pTarget);
        if (target.m_nStealthMode == 0 && !bTargetInvisible.ToBool())
        {
          return true.ToInt();
        }

        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnDoListenDetection eventData = ProcessEvent(new OnDoListenDetection
        {
          Creature = creature.ToNwObject<NwCreature>(),
          Target = target.ToNwObject<NwCreature>(),
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

namespace NWN.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="NWN.API.Events.OnDoListenDetection"/>
    public event Action<OnDoListenDetection> OnDoListenDetection
    {
      add => EventService.Subscribe<OnDoListenDetection, OnDoListenDetection.Factory>(this, value);
      remove => EventService.Unsubscribe<OnDoListenDetection, OnDoListenDetection.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {

  }
}
