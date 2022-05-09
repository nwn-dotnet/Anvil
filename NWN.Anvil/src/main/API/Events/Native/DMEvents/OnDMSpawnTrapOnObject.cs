using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public sealed class OnDMSpawnTrapOnObject : IEvent
  {
    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    public NwStationary Target { get; internal init; }

    NwObject? IEvent.Context => DungeonMaster?.LoginCreature;
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
