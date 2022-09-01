using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnDoListenDetection : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    public NwCreature Target { get; private init; } = null!;
    public VisibilityOverride VisibilityOverride { get; set; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<DoListenDetectionHook> Hook { get; set; } = null!;

      private delegate int DoListenDetectionHook(void* pCreature, void* pTarget, int bTargetInvisible);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, int, int> pHook = &OnDoListenDetection;
        Hook = HookService.RequestHook<DoListenDetectionHook>(pHook, FunctionsLinux._ZN12CNWSCreature17DoListenDetectionEPS_i, HookOrder.Early);
        return new IDisposable[] { Hook };
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

        OnDoListenDetection eventData = ProcessEvent(EventCallbackType.Before, new OnDoListenDetection
        {
          Creature = creature.ToNwObject<NwCreature>()!,
          Target = target.ToNwObject<NwCreature>()!,
        });

        int retVal = eventData.VisibilityOverride switch
        {
          VisibilityOverride.Visible => true.ToInt(),
          VisibilityOverride.NotVisible => false.ToInt(),
          _ => Hook.CallOriginal(pCreature, pTarget, bTargetInvisible)
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
    /// <inheritdoc cref="Events.OnDoListenDetection"/>
    public event Action<OnDoListenDetection> OnDoListenDetection
    {
      add => EventService.Subscribe<OnDoListenDetection, OnDoListenDetection.Factory>(this, value);
      remove => EventService.Unsubscribe<OnDoListenDetection, OnDoListenDetection.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDoListenDetection"/>
    public event Action<OnDoListenDetection> OnDoListenDetection
    {
      add => EventService.SubscribeAll<OnDoListenDetection, OnDoListenDetection.Factory>(value);
      remove => EventService.UnsubscribeAll<OnDoListenDetection, OnDoListenDetection.Factory>(value);
    }
  }
}
