using System;
using NWN.API.Events;
using NWN.Services;

namespace NWNX.Services
{
  [ServiceBinding(typeof(NWNXEventService))]
  public sealed class NWNXEventService
  {
    private readonly EventService eventService;

    public NWNXEventService(EventService eventService)
    {
      this.eventService = eventService;
    }

    /// <summary>
    /// Subscribes to the specified event.
    /// </summary>
    /// <param name="callback">The callback function/handler for this event.</param>
    /// <typeparam name="TEvent">The event to subscribe to.</typeparam>
    [Obsolete("Use the EventService.SubscribeAll function, using the `NWNXEventFactory` as the factory. NWNX events will be removed in the future and replaced with native implementations.")]
    public void Subscribe<TEvent>(Action<TEvent> callback) where TEvent : IEvent, new()
    {
      eventService.SubscribeAll<TEvent, NWNXEventFactory>(callback)
        .Register<TEvent>();
    }

    /// <summary>
    /// Removes an existing global event handler that was added using <see cref="Subscribe{TEvent}"/>.
    /// </summary>
    /// <param name="callback">The existing handler/callback.</param>
    /// <typeparam name="TEvent">The event to unsubscribe from.</typeparam>
    [Obsolete("Use the EventService.UnsubscribeAll function, using the `NWNXEventFactory` as the factory. NWNX events will be removed in the future and replaced with native implementations.")]
    public void Unsubscribe<TEvent>(Action<TEvent> callback) where TEvent : IEvent, new()
    {
      eventService.UnsubscribeAll<TEvent, NWNXEventFactory>(callback);
    }
  }
}
