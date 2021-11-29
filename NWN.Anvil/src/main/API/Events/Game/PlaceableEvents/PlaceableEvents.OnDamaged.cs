using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Events for <see cref="NwPlaceable"/>.
  /// </summary>
  public static partial class PlaceableEvents
  {
    /// <summary>
    /// Called when <see cref="NwGameObject"/> has damaged <see cref="NwPlaceable"/>.
    /// </summary>
    [GameEvent(EventScriptType.PlaceableOnDamaged)]
    public sealed class OnDamaged : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwPlaceable"/> that was damaged.
      /// </summary>
      public NwPlaceable DamagedObject { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlaceable>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that damaged the <see cref="NwPlaceable"/>.
      /// </summary>
      public NwGameObject Damager { get; } = NWScript.GetLastDamager().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets the total damage dealt to <see cref="NwPlaceable"/>.
      /// </summary>
      public int TotalDamageDealt { get; } = NWScript.GetTotalDamageDealt();

      NwObject IEvent.Context
      {
        get => DamagedObject;
      }

      /// <summary>
      /// Gets <see cref="DamageType"/> dealt to <see cref="NwPlaceable"/>.
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
  public sealed partial class NwPlaceable
  {
    /// <inheritdoc cref="PlaceableEvents.OnDamaged"/>
    public event Action<PlaceableEvents.OnDamaged> OnDamaged
    {
      add => EventService.Subscribe<PlaceableEvents.OnDamaged, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<PlaceableEvents.OnDamaged, GameEventFactory>(this, value);
    }
  }
}
