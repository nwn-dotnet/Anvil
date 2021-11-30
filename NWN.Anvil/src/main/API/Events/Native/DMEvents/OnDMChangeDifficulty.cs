using System;
using Anvil.API.Events;

namespace Anvil.API.Events
{
  public sealed class OnDMChangeDifficulty : IEvent
  {
    public NwPlayer DungeonMaster { get; internal init; }
    public GameDifficulty NewDifficulty { get; internal init; }

    public bool Skip { get; set; }

    NwObject IEvent.Context
    {
      get => DungeonMaster?.LoginCreature;
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="Events.OnDMChangeDifficulty"/>
    public event Action<OnDMChangeDifficulty> OnDMChangeDifficulty
    {
      add => EventService.Subscribe<OnDMChangeDifficulty, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMChangeDifficulty, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="Events.OnDMChangeDifficulty"/>
    public event Action<OnDMChangeDifficulty> OnDMChangeDifficulty
    {
      add => EventService.SubscribeAll<OnDMChangeDifficulty, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMChangeDifficulty, DMEventFactory>(value);
    }
  }
}
