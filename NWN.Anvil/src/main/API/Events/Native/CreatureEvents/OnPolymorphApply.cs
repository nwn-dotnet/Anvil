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
  /// Called when a creature is about to lose a polymorph effect.
  /// </summary>
  public sealed class OnPolymorphRemove : IEvent
  {
    /// <summary>
    /// Gets the creature that will lose the polymorph effect.
    /// </summary>
    public NwCreature Creature { get; private init; } = null!;

    /// <summary>
    /// Gets the active polymorph type of this creature.
    /// </summary>
    public PolymorphTableEntry PolymorphType { get; private init; } = null!;

    /// <summary>
    /// Gets or sets if the creature polymorph should be preserved instead of removed.
    /// </summary>
    public bool PreventRemove { get; set; }

    NwObject IEvent.Context => Creature;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<Functions.CNWSEffectListHandler.OnRemovePolymorph> Hook { get; set; } = null!;

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, void*, int> pHook = &OnRemovePolymorph;
        Hook = HookService.RequestHook<Functions.CNWSEffectListHandler.OnRemovePolymorph>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnRemovePolymorph(void* pEffectListHandler, void* pObject, void* pEffect)
      {
        NwCreature? creature = CNWSObject.FromPointer(pObject).ToNwObjectSafe<NwCreature>();
        CGameEffect effect = CGameEffect.FromPointer(pEffect);
        PolymorphTableEntry? polymorphType = NwGameTables.PolymorphTable.ElementAtOrDefault(effect.GetInteger(0));

        if (creature == null || polymorphType == null)
        {
          return Hook.CallOriginal(pEffectListHandler, pObject, pEffect);
        }

        OnPolymorphRemove eventData = ProcessEvent(EventCallbackType.Before, new OnPolymorphRemove
        {
          Creature = creature,
          PolymorphType = polymorphType,
        });

        int retVal = !eventData.PreventRemove ? Hook.CallOriginal(pEffectListHandler, pObject, pEffect) : 0;
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
    /// <inheritdoc cref="OnPolymorphRemove"/>
    public event Action<OnPolymorphRemove> OnPolymorphRemove
    {
      add => EventService.Subscribe<OnPolymorphRemove, OnPolymorphRemove.Factory>(this, value);
      remove => EventService.Unsubscribe<OnPolymorphRemove, OnPolymorphRemove.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="OnPolymorphRemove"/>
    public event Action<OnPolymorphRemove> OnPolymorphRemove
    {
      add => EventService.SubscribeAll<OnPolymorphRemove, OnPolymorphRemove.Factory>(value);
      remove => EventService.UnsubscribeAll<OnPolymorphRemove, OnPolymorphRemove.Factory>(value);
    }
  }
}
