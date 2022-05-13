using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for door objects.
  /// </summary>
  public static partial class DoorEvents
  {
    [GameEvent(EventScriptType.DoorOnDamage)]
    public sealed class OnDamaged : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that damaged the <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature DamagedBy { get; } = NWScript.GetLastDamager().ToNwObject<NwCreature>()!;

      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was damaged.
      /// </summary>
      public NwDoor Door { get; } = NWScript.OBJECT_SELF.ToNwObject<NwDoor>()!;

      /// <summary>
      /// Gets the total damage dealt to the <see cref="NwDoor"/>.
      /// </summary>
      public int TotalDamageDealt { get; } = NWScript.GetTotalDamageDealt();

      NwObject IEvent.Context => Door;

      /// <summary>
      /// Gets damage dealt to <see cref="NwDoor"/>, by <see cref="DamageType"/>.
      /// </summary>
      public int GetDamageDealtByType(DamageType damageType)
      {
        return NWScript.GetDamageDealtByType((int)damageType);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwDoor
  {
    /// <inheritdoc cref="DoorEvents.OnDamaged"/>
    public event Action<DoorEvents.OnDamaged> OnDamaged
    {
      add => EventService.Subscribe<DoorEvents.OnDamaged, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<DoorEvents.OnDamaged, GameEventFactory>(this, value);
    }
  }
}
