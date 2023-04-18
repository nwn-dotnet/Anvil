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
    /// Triggered when a <see cref="NwCreature"/> equips an <see cref="NwItem"/>.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnEquipItem)]
    public sealed class OnPlayerEquipItem : IEvent
    {
      public OnPlayerEquipItem()
      {
        // Patch player reference due to a reference bug during client enter context
        // See https://github.com/Beamdog/nwn-issues/issues/367
        if (Player is null && Item?.Possessor is NwCreature creature)
        {
          Player = creature;
        }
      }

      /// <summary>
      /// Gets the last equipped <see cref="NwItem"/> that triggered the event.
      /// </summary>
      /// <remarks>
      /// Nullable because of an edge case where the item is invalid on logout while polymorphed.
      /// See https://github.com/Beamdog/nwn-issues/issues/464
      /// </remarks>
      public NwItem? Item { get; } = NWScript.GetPCItemLastEquipped().ToNwObject<NwItem>();

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that last equipped <see cref="NwItem"/>.
      /// </summary>
      public NwCreature? Player { get; } = NWScript.GetPCItemLastEquippedBy().ToNwObject<NwCreature>();

      /// <summary>
      /// Gets the updated slot for the equipped item.
      /// </summary>
      public InventorySlot Slot { get; } = (InventorySlot)NWScript.GetPCItemLastEquippedSlot();

      NwObject? IEvent.Context => Player;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerEquipItem"/>
    public event Action<ModuleEvents.OnPlayerEquipItem> OnPlayerEquipItem
    {
      add => EventService.SubscribeAll<ModuleEvents.OnPlayerEquipItem, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnPlayerEquipItem, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnPlayerEquipItem"/>
    public event Action<ModuleEvents.OnPlayerEquipItem> OnPlayerEquipItem
    {
      add => EventService.Subscribe<ModuleEvents.OnPlayerEquipItem, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnPlayerEquipItem, GameEventFactory>(ControlledCreature, value);
    }
  }
}
