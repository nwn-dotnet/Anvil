using System;
using System.Linq;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when a creature is about to be affected by a polymorph effect.
  /// </summary>
  public sealed class OnPolymorphApply : IEvent
  {
    /// <summary>
    /// Gets the creature that is being polymorphed.
    /// </summary>
    public NwCreature Creature { get; private init; } = null!;

    /// <summary>
    /// Gets the polymorph type that this creature will transform to.
    /// </summary>
    public PolymorphTableEntry PolymorphType { get; private init; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the creature should be prevented from being polymorphed.
    /// </summary>
    public bool PreventPolymorph { get; set; }

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSEffectListHandler.OnApplyPolymorph> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int, int> pHook = &OnApplyPolymorph;
        Hook = HookService.RequestHook<Functions.CNWSEffectListHandler.OnApplyPolymorph>(pHook, HookOrder.Early);
        return [Hook];
      }

      [UnmanagedCallersOnly]
      private static int OnApplyPolymorph(void* pEffectListHandler, void* pObject, void* pEffect, int bLoadingGame = 0)
      {
        NwCreature? creature = CNWSObject.FromPointer(pObject).ToNwObjectSafe<NwCreature>();
        CGameEffect effect = CGameEffect.FromPointer(pEffect);
        PolymorphTableEntry? polymorphType = NwGameTables.PolymorphTable.ElementAtOrDefault(effect.GetInteger(0));

        if (creature == null || polymorphType == null)
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame);
        }

        OnPolymorphApply eventData = ProcessEvent(EventCallbackType.Before, new OnPolymorphApply
        {
          Creature = creature,
          PolymorphType = polymorphType,
        });

        int retVal = !eventData.PreventPolymorph ? Hook.CallOriginal(pEffectListHandler, pObject, pEffect, bLoadingGame) : 0;
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
    /// <inheritdoc cref="OnPolymorphApply"/>
    public event Action<OnPolymorphApply> OnPolymorphApply
    {
      add => EventService.Subscribe<OnPolymorphApply, OnPolymorphApply.Factory>(this, value);
      remove => EventService.Unsubscribe<OnPolymorphApply, OnPolymorphApply.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="OnPolymorphApply"/>
    public event Action<OnPolymorphApply> OnPolymorphApply
    {
      add => EventService.SubscribeAll<OnPolymorphApply, OnPolymorphApply.Factory>(value);
      remove => EventService.UnsubscribeAll<OnPolymorphApply, OnPolymorphApply.Factory>(value);
    }
  }
}
