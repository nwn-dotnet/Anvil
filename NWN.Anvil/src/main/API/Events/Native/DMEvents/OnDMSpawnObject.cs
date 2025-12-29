using System;
using System.Numerics;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  /// <summary>
  /// Triggered when a DM spawns an object.
  /// </summary>
  public sealed class OnDMSpawnObject : DMEvent
  {
    /// <summary>
    /// Gets the area where the object will be spawned.
    /// </summary>
    public NwArea Area { get; internal init; } = null!;

    /// <summary>
    /// Gets the type of object that will be spawned.
    /// </summary>
    public ObjectTypes ObjectType { get; internal init; }

    /// <summary>
    /// Gets the spawn position.
    /// </summary>
    public Vector3 Position { get; internal init; }

    /// <summary>
    /// Gets the resource reference that will be spawned.
    /// </summary>
    public string ResRef { get; internal init; } = null!;

    /// <summary>
    /// Gets the spawned object.
    /// </summary>
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
