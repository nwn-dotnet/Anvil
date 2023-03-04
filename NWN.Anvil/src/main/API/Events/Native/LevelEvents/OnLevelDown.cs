using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnLevelDown : IEvent
  {
    public NwCreature Creature { get; private init; } = null!;

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<LevelDownHook> Hook { get; set; } = null!;

      [NativeFunction("_ZN17CNWSCreatureStats9LevelDownEP13CNWLevelStats", "")]
      private delegate void LevelDownHook(void* pCreatureStats, void* pLevelUpStats);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void> pHook = &OnLevelDown;
        Hook = HookService.RequestHook<LevelDownHook>(pHook, HookOrder.Earliest);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnLevelDown(void* pCreatureStats, void* pLevelUpStats)
      {
        OnLevelDown eventData = ProcessEvent(EventCallbackType.Before, new OnLevelDown
        {
          Creature = CNWSCreatureStats.FromPointer(pCreatureStats).m_pBaseCreature.ToNwObject<NwCreature>()!,
        });

        Hook.CallOriginal(pCreatureStats, pLevelUpStats);
        ProcessEvent(EventCallbackType.After, eventData);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnLevelDown"/>
    public event Action<OnLevelDown> OnLevelDown
    {
      add => EventService.Subscribe<OnLevelDown, OnLevelDown.Factory>(this, value);
      remove => EventService.Unsubscribe<OnLevelDown, OnLevelDown.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnLevelDown"/>
    public event Action<OnLevelDown> OnLevelDown
    {
      add => EventService.SubscribeAll<OnLevelDown, OnLevelDown.Factory>(value);
      remove => EventService.UnsubscribeAll<OnLevelDown, OnLevelDown.Factory>(value);
    }
  }
}
