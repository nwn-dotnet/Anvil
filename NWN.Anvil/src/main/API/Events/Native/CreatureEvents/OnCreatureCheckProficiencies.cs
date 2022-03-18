using System;
using System.Runtime.InteropServices;
using Anvil.API.Events;
using NWN.Native.API;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Called when an item is tested against a creature's proficiencies to determine if the creature may attempt to equip the item.
  /// </summary>
  public sealed class OnCreatureCheckProficiencies : IEvent
  {
    /// <summary>
    /// The creature whose proficiencies are being checked for the item to be equipped.
    /// </summary>
    public NwCreature Creature { get; private init; }

    /// <summary>
    /// The item attempting to be equipped.
    /// </summary>
    public NwItem Item { get; private init; }

    /// <summary>
    /// Gets or sets an override result to use for this proficiency check.
    /// </summary>
    public CheckProficiencyOverride ResultOverride { get; set; }

    /// <summary>
    /// The inventory slot the item is attempting to be equipped to.
    /// </summary>
    public EquipmentSlots TargetSlot { get; private init; }

    NwObject IEvent.Context => Creature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      internal delegate int CheckProficienciesHook(void* pCreature, void* pItem, uint nEquipToSlot);

      private static FunctionHook<CheckProficienciesHook> Hook { get; set; }

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, void*, uint, int> pHook = &OnCheckProficiencies;
        Hook = HookService.RequestHook<CheckProficienciesHook>(pHook, FunctionsLinux._ZN12CNWSCreature18CheckProficienciesEP8CNWSItemj, HookOrder.Early);
        return new IDisposable[] { Hook };
      }

      [UnmanagedCallersOnly]
      private static int OnCheckProficiencies(void* pCreature, void* pItem, uint nEquipToSlot)
      {
        NwCreature creature = CNWSCreature.FromPointer(pCreature).ToNwObject<NwCreature>();
        NwItem item = CNWSItem.FromPointer(pItem).ToNwObject<NwItem>();

        if (creature == null || item == null)
        {
          return Hook.CallOriginal(pCreature, pItem, nEquipToSlot);
        }

        OnCreatureCheckProficiencies eventData = ProcessEvent(new OnCreatureCheckProficiencies
        {
          Creature = creature,
          Item = item,
          TargetSlot = (EquipmentSlots)nEquipToSlot,
        });

        return eventData.ResultOverride switch
        {
          CheckProficiencyOverride.HasProficiency => true.ToInt(),
          CheckProficiencyOverride.NoProficiency => false.ToInt(),
          _ => Hook.CallOriginal(pCreature, pItem, nEquipToSlot),
        };
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="Events.OnCreatureCheckProficiencies"/>
    public event Action<OnCreatureCheckProficiencies> OnCreatureCheckProficiencies
    {
      add => EventService.Subscribe<OnCreatureCheckProficiencies, OnCreatureCheckProficiencies.Factory>(this, value);
      remove => EventService.Unsubscribe<OnCreatureCheckProficiencies, OnCreatureCheckProficiencies.Factory>(this, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnCreatureCheckProficiencies"/>
    public event Action<OnCreatureCheckProficiencies> OnCreatureCheckProficiencies
    {
      add => EventService.SubscribeAll<OnCreatureCheckProficiencies, OnCreatureCheckProficiencies.Factory>(value);
      remove => EventService.UnsubscribeAll<OnCreatureCheckProficiencies, OnCreatureCheckProficiencies.Factory>(value);
    }
  }
}
