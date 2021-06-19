using System;
using NWN.API.Events;

namespace NWN.API.Events
{
  public sealed class OnDMPlayerDMLogin : IEvent
  {
    public string Password { get; internal init; }

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
    /// <inheritdoc cref="NWN.API.Events.OnDMPlayerDMLogin"/>
    public event Action<OnDMPlayerDMLogin> OnDMPlayerDMLogin
    {
      add => EventService.Subscribe<OnDMPlayerDMLogin, DMEventFactory>(LoginCreature, value);
      remove => EventService.Unsubscribe<OnDMPlayerDMLogin, DMEventFactory>(LoginCreature, value);
    }
  }

  public sealed partial class NwModule
  {
    /// <inheritdoc cref="NWN.API.Events.OnDMPlayerDMLogin"/>
    public event Action<OnDMPlayerDMLogin> OnDMPlayerDMLogin
    {
      add => EventService.SubscribeAll<OnDMPlayerDMLogin, DMEventFactory>(value);
      remove => EventService.UnsubscribeAll<OnDMPlayerDMLogin, DMEventFactory>(value);
    }
  }
}
