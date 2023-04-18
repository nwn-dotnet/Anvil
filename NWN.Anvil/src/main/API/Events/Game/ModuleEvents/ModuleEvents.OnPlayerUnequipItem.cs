using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Global module events.
  /// </summary>
  public static partial class ModuleEvents
  {
    /// <summary>
    /// Triggered just before a <see cref="NwCreature"/> un-equips an <see cref="NwItem"/>.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnUnequipItem)]
    public sealed class OnPlayerUnequipItem : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwItem"/> that was last unequipped.
      /// </summary>
      public NwItem Item { get; } = NWScript.GetPCItemLastUnequipped().ToNwObject<NwItem>()!;

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that triggered the event.
      /// </summary>
      public NwCreature UnequippedBy { get; } = NWScript.GetPCItemLastUnequippedBy().ToNwObject<NwCreature>()!;

      /// <summary>
      /// Gets the slot that this item was taken from.
      /// </summary>
      public InventorySlot Slot { get; } = (InventorySlot)NWScript.GetPCItemLastUnequippedSlot();

      NwObject IEvent.Context => UnequippedBy;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerUnequipItem"/>
    public event Action<ModuleEvents.OnPlayerUnequipItem> OnPlayerUnequipItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerUnequipItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerUnequipItem, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerUnequipItem"/>
    public event Action<ModuleEvents.OnPlayerUnequipItem> OnPlayerUnequipItem
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerUnequipItem, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerUnequipItem, GameEventFactory>(ControlledCreature, value);
    }
  }
}
