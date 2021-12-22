using System;
using System.Numerics;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public abstract class OnDMSpawnObject : IEvent
  {
    public NwPlayer DungeonMaster { get; internal init; }

    NwObject IEvent.Context => DungeonMaster?.LoginCreature;
  }

  public sealed class OnDMSpawnObjectBefore : OnDMSpawnObject
  {
    public NwArea Area { get; internal init; }

    public ObjectTypes ObjectType { get; internal init; }

    public Vector3 Position { get; internal init; }

    public string ResRef { get; internal init; }
    public bool Skip { get; set; }
  }

  public sealed class OnDMSpawnObjectAfter : OnDMSpawnObject
  {
    public NwGameObject SpawnedObject { get; internal init; }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMSpawnObjectAfter"/>
    public event Action<OnDMSpawnObjectAfter> OnDMSpawnObjectAfter
    {
      add => EventService.Subscribe<OnDMSpawnObjectAfter, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSpawnObjectAfter, DMEventFactory>(LoginCreature, value);
    }

    /// <inheritdoc cref="Events.OnDMSpawnObjectBefore"/>
    public event Action<OnDMSpawnObjectBefore> OnDMSpawnObjectBefore
    {
      add => EventService.Subscribe<OnDMSpawnObjectBefore, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMSpawnObjectBefore, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMSpawnObjectAfter"/>
    public event Action<OnDMSpawnObjectAfter> OnDMSpawnObjectAfter
    {
      add => EventService.SubscribeAll<OnDMSpawnObjectAfter, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSpawnObjectAfter, DMEventFactory>(value);
    }

    /// <inheritdoc cref="Events.OnDMSpawnObjectBefore"/>
    public event Action<OnDMSpawnObjectBefore> OnDMSpawnObjectBefore
    {
      add => EventService.SubscribeAll<OnDMSpawnObjectBefore, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMSpawnObjectBefore, DMEventFactory>(value);
    }
  }
}
