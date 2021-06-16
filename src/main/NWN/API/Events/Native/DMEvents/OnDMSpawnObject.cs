using System;
using NWN.API.Events;

namespace NWN.API.Events
{
  public sealed class OnDMSpawnObject : IEvent
  {
    public NwGameObject SpawnedObject { get; internal init; }

    public NwPlayer DungeonMaster { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }
}

namespace NWN.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMSpawnObject"/>
    public event Action<OnDMSpawnObject> OnDMSpawnObject
    {
      add => EventService.Subscribe<OnDMSpawnObject, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSpawnObject, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMSpawnObject"/>
    public event Action<OnDMSpawnObject> OnDMSpawnObject
    {
      add => EventService.SubscribeAll<OnDMSpawnObject, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSpawnObject, DMEventFactory>(value);
    }
  }
}
