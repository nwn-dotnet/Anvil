using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnDoSpotDetection : IEvent
  {
    public VisibilityOverride VisibilityOverride { get; set; }

    public NwCreature Creature { get; private init; }

    public NwCreature Target { get; private init; }

    NwObject IEvent.Context
    {
      get => Creature;
    }

    internal sealed unsafe class Factory : SingleHookEventFactory<Factory.DoSpotDetectionHook>
    {
      internal delegate int DoSpotDetectionHook(void* pCreature, void* pTarget, int bTargetInvisible);

      protected override FunctionHook<DoSpotDetectionHook> RequestHook()
      {
        delegate* unmanaged<void*, void*, int, int> pHook = &OnDoSpotDetection;
        return HookService.RequestHook<DoSpotDetectionHook>(pHook, FunctionsLinux._ZN12CNWSCreature15DoSpotDetectionEPS_i, HookOrder.Early);
      }

      [UnmanagedCallersOnly]
      private static int OnDoSpotDetection(void* pCreature, void* pTarget, int bTargetInvisible)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
        CNWSCreature target = CNWSCreature.FromPointer(pTarget);

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

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnDoSpotDetection"/>
    public event Action<OnDoSpotDetection> OnDoSpotDetection
    {
      add => EventService.Subscribe<OnDoSpotDetection, OnDoSpotDetection.Factory>(this, value);
      remove => EventService.Unsubscribe<OnDoSpotDetection, OnDoSpotDetection.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDoSpotDetection"/>
    public event Action<OnDoSpotDetection> OnDoSpotDetection
    {
      add => EventService.SubscribeAll<OnDoSpotDetection, OnDoSpotDetection.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDoSpotDetection, OnDoSpotDetection.Factory>(value);
    }
  }
}
