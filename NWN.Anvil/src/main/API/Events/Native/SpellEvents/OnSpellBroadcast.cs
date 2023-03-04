using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed class OnSpellBroadcast : IEvent
  {
    public NwCreature Caster { get; private init; } = null!;

    public int ClassIndex { get; private init; }

    public NwFeat Feat { get; private init; } = null!;
    public bool PreventSpellCast { get; set; }

    public NwSpell Spell { get; private init; } = null!;

    NwObject IEvent.Context => Caster;

    public sealed unsafe class Factory : HookEventFactory
    {
      private static FunctionHook<BroadcastSpellCastHook> Hook { get; set; } = null!;

      [NativeFunction("_ZN12CNWSCreature18BroadcastSpellCastEjht", "")]
      private delegate void BroadcastSpellCastHook(void* pCreature, uint nSpellId, byte nMultiClass, ushort nFeat);

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, uint, byte, ushort, void> pHook = &OnBroadcastSpellCast;
        Hook = HookService.RequestHook<BroadcastSpellCastHook>(pHook, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static void OnBroadcastSpellCast(void* pCreature, uint nSpellId, byte nMultiClass, ushort nFeat)
      {
        CNWSCreature creature = CNWSCreature.FromPointer(pCreature);

        OnSpellBroadcast eventData = ProcessEvent(EventCallbackType.Before, new OnSpellBroadcast
        {
          Caster = creature.ToNwObject<NwCreature>()!,
          Spell = NwSpell.FromSpellId((int)nSpellId)!,
          ClassIndex = nMultiClass,
          Feat = NwFeat.FromFeatId(nFeat)!,
        });

        if (!eventData.PreventSpellCast)
        {
          Hook.CallOriginal(pCreature, nSpellId, nMultiClass, nFeat);
        }

        ProcessEvent(EventCallbackType.After, eventData);
      }
    }
  }
}

namespace Anvil.API
{
  public abstract partial class NwGameObject
  {
    /// <inheritdoc cref="Events.OnSpellBroadcast"/>
    public event Action<OnSpellBroadcast> OnSpellBroadcast
    {
      add => EventService.Subscribe<OnSpellBroadcast, OnSpellBroadcast.Factory>(this, value);
      remove => EventService.Unsubscribe<OnSpellBroadcast, OnSpellBroadcast.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnSpellBroadcast"/>
    public event Action<OnSpellBroadcast> OnSpellBroadcast
    {
      add => EventService.SubscribeAll<OnSpellBroadcast, OnSpellBroadcast.Factory>(value);
      remove => EventService.UnsubscribeAll<OnSpellBroadcast, OnSpellBroadcast.Factory>(value);
    }
  }
}
