using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a creature is about to learn a spell from a scroll.
  /// </summary>
  public sealed class OnItemScrollLearn : IEvent
  {
    /// <summary>
    /// Gets the creature learning the scroll.
    /// </summary>
    public NwCreature Creature { get; private init; } = null!;

    /// <summary>
    /// Set to true to prevent this scroll from being learned.
    /// </summary>
    public bool PreventLearnScroll { get; set; }

    /// <summary>
    /// Gets the scroll item that is being learned.
    /// </summary>
    public NwItem Scroll { get; private init; } = null!;

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSCreature.LearnScroll> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, int> pHook = &OnLearnScroll;
        Hook = HookService.RequestHook<Functions.CNWSCreature.LearnScroll>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnLearnScroll(void* pCreature, uint oidScrollToLearn)
      {
        OnItemScrollLearn eventData = ProcessEvent(EventCallbackType.Before, new OnItemScrollLearn
        {
          Creature = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>()!,
          Scroll = oidScrollToLearn.ToNwObject<NwItem>()!,
        });

        int retVal = !eventData.PreventLearnScroll ? Hook.CallOriginal(pCreature, oidScrollToLearn) : false.ToInt();
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
    /// <inheritdoc cref="Events.OnItemScrollLearn"/>
    public event Action<OnItemScrollLearn> OnItemScrollLearn
    {
      add => EventService.Subscribe<OnItemScrollLearn, OnItemScrollLearn.Factory>(this, value);
      remove => EventService.Unsubscribe<OnItemScrollLearn, OnItemScrollLearn.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnItemScrollLearn"/>
    public event Action<OnItemScrollLearn> OnItemScrollLearn
    {
      add => EventService.SubscribeAll<OnItemScrollLearn, OnItemScrollLearn.Factory>(value);
      remove => EventService.UnsubscribeAll<OnItemScrollLearn, OnItemScrollLearn.Factory>(value);
    }
  }
}
