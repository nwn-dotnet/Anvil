using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific creature.
  /// </summary>
  public static partial class CreatureEvents
  {
    /// <summary>
    /// Triggered by <see cref="NwCreature"/> when its inventory has been disturbed.
    /// </summary>
    [GameEvent(EventScriptType.CreatureOnDisturbed)]
    public sealed class OnDisturbed : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that had its inventory disturbed.
      /// </summary>
      public NwCreature CreatureDisturbed { get; }

      /// <summary>
      /// Gets the <see cref="NwItem"/> that was disturbed in the inventory.
      /// </summary>
      public NwItem DisturbedItem { get; }

      /// <summary>
      /// Gets the <see cref="NwCreature"/> that disturbed another <see cref="NwCreature"/> inventory.
      /// </summary>
      public NwCreature Disturber { get; }

      public InventoryDisturbType DisturbType { get; }

      NwObject IEvent.Context => CreatureDisturbed;

      public OnDisturbed()
      {
        CreatureDisturbed = NWScript.OBJECT_SELF.ToNwObject<NwCreature>()!;
        DisturbedItem = NWScript.GetInventoryDisturbItem(CreatureDisturbed).ToNwObject<NwItem>()!;
        Disturber = NWScript.GetLastDisturbed(CreatureDisturbed).ToNwObject<NwCreature>()!;
        DisturbType = (InventoryDisturbType)NWScript.GetInventoryDisturbType(CreatureDisturbed);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwCreature
  {
    /// <inheritdoc cref="CreatureEvents.OnDisturbed"/>
    public event Action<CreatureEvents.OnDisturbed> OnDisturbed
    {
      add => EventService.Subscribe<CreatureEvents.OnDisturbed, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<CreatureEvents.OnDisturbed, GameEventFactory>(this, value);
    }
  }
}
