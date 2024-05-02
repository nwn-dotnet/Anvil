using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Built-in events associated with a specific door.
  /// </summary>
  public static partial class DoorEvents
  {
    [GameEvent(EventScriptType.DoorOnDamage)]
    public sealed class OnDamaged : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwCreature"/> that damaged the <see cref="NwDoor"/>.
      /// </summary>
      public NwCreature DamagedBy { get; }

      /// <summary>
      /// Gets the <see cref="NwDoor"/> that was damaged.
      /// </summary>
      public NwDoor Door { get; }

      /// <summary>
      /// Gets the total damage dealt to the <see cref="NwDoor"/>.
      /// </summary>
      public int TotalDamageDealt { get; }

      NwObject IEvent.Context => Door;

      /// <summary>
      /// Gets damage dealt to <see cref="NwDoor"/>, by <see cref="DamageType"/>.
      /// </summary>
      public int GetDamageDealtByType(DamageType damageType)
      {
        return NWScript.GetDamageDealtByType((int)damageType);
      }

      public OnDamaged()
      {
        uint objSelf = NWScript.OBJECT_SELF;
        Door = objSelf.ToNwObject<NwDoor>()!;
        TotalDamageDealt = NWScript.GetTotalDamageDealt();
        DamagedBy = NWScript.GetLastDamager(objSelf).ToNwObject<NwCreature>()!;
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
