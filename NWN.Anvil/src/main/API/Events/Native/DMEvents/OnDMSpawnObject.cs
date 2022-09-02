using System;
using System.Numerics;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public sealed class OnDMSpawnObject : DMEvent
  {
    public NwArea Area { get; internal init; } = null!;

    public ObjectTypes ObjectType { get; internal init; }

    public Vector3 Position { get; internal init; }

    public string ResRef { get; internal init; } = null!;

    public NwGameObject? SpawnedObject { get; internal set; }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMSpawnObject"/>
    public event Action<OnDMSpawnObject> OnDMSpawnObject
    {
      add => EventService.Subscribe<OnDMSpawnObject, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSpawnObject, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMSpawnObject"/>
    public event Action<OnDMSpawnObject> OnDMSpawnObject
    {
      add => EventService.SubscribeAll<OnDMSpawnObject, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSpawnObject, DMEventFactory>(value);
    }
  }
}
