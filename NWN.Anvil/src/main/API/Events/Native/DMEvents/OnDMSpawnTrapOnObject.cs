using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a DM spawns a trap on a target object.
  /// </summary>
  public sealed class OnDMSpawnTrapOnObject : DMEvent
  {
    /// <summary>
    /// Gets the target object where the trap will be added.
    /// </summary>
    public NwStationary Target { get; internal init; } = null!;
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMSpawnTrapOnObject"/>
    public event Action<OnDMSpawnTrapOnObject> OnDMSpawnTrapOnObject
    {
      add => EventService.Subscribe<OnDMSpawnTrapOnObject, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSpawnTrapOnObject, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMSpawnTrapOnObject"/>
    public event Action<OnDMSpawnTrapOnObject> OnDMSpawnTrapOnObject
    {
      add => EventService.SubscribeAll<OnDMSpawnTrapOnObject, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSpawnTrapOnObject, DMEventFactory>(value);
    }
  }
}
