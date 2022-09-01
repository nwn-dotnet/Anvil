using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnDoSpotDetection : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    public NwCreature Target { get; private init; } = null!;

    public VisibilityOverride VisibilityOverride { get; set; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<DoSpotDetectionHook> Hook { get; set; } = null!;

      private delegate int DoSpotDetectionHook(void* pCreature, void* pTarget, int bTargetInvisible);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int, int> pHook = &OnDoSpotDetection;
        Hook = HookService.RequestHook<DoSpotDetectionHook>(pHook, FunctionsLinux._ZN12CNWSCreature15DoSpotDetectionEPS_i, HookOrder.Early);
        return new IDisposable[] { Hook };
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

        OnDoSpotDetection eventData = ProcessEvent(EventCallbackType.Before, new OnDoSpotDetection
        {
          Creature = creature.ToNwObject<NwCreature>()!,
          Target = target.ToNwObject<NwCreature>()!,
        });

        int retVal = eventData.VisibilityOverride switch
        {
          VisibilityOverride.Visible => true.ToInt(),
          VisibilityOverride.NotVisible => false.ToInt(),
          _ => Hook.CallOriginal(pCreature, pTarget, bTargetInvisible),
        };

        ProcessEvent(EventCallbackType.After, eventData);
        return retVal;
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
