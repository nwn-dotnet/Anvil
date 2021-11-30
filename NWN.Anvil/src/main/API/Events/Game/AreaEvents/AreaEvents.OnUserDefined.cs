using System;
using Anvil.API.Events;
using NWN.Core;

namespace Anvil.API.Events
{
  public static partial class AreaEvents
  {
    [GameEvent(EventScriptType.AreaOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>();

      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context
      {
        get => Area;
      }

      public static void Signal(NwArea area, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(area, nwEvent);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwArea
  {
    /// <inheritdoc cref="AreaEvents.OnUserDefined"/>
    public event Action<AreaEvents.OnUserDefined> OnUserDefined
    {
      add => EventService.Subscribe<AreaEvents.OnUserDefined, GameEventFactory, GameEventFactory.RegistrationData>(this, new GameEventFactory.RegistrationData(this), value);
      remove => EventService.Unsubscribe<AreaEvents.OnUserDefined, GameEventFactory>(this, value);
    }
  }
}
