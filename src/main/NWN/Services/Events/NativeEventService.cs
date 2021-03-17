using System;
using System.Collections.Generic;
using System.Reflection;
using NLog;
using NWN.API;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;

namespace NWN.Services
{
  //! ## Examples
  //! @include WelcomeMessageService.cs

  /// <summary>
  /// Provides access to subscribe and unsubscribe from object events.<br/>
  /// </summary>
  [ServiceBinding(typeof(NativeEventService))]
  public sealed class NativeEventService
  {
    private readonly EventService eventService;

    public NativeEventService(EventService eventService)
    {
      this.eventService = eventService;
    }

    /// <summary>
    /// Subscribes to the specified event on the given object.
    /// </summary>
    /// <param name="nwObject">The subscribe target for this event.</param>
    /// <param name="callback">The callback function/handler for this event.</param>
    /// <typeparam name="TObject">The type of nwObject.</typeparam>
    /// <typeparam name="TEvent">The event to subscribe to.</typeparam>
    [Obsolete("Use the available C# events instead (e.g. NwModule.OnClientEnter)")]
    public void Subscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> callback)
      where TEvent : IEvent, new()
      where TObject : NwObject
    {
      eventService.Subscribe<TEvent, GameEventFactory>(nwObject, callback).Register<TEvent>(nwObject);
    }

    /// <summary>
    /// Removes an existing event handler from an object that was added using <see cref="Subscribe{TObject,TEvent}"/>.
    /// </summary>
    /// <param name="nwObject">The object containing the existing subscription.</param>
    /// <param name="callback">The existing handler/callback.</param>
    /// <typeparam name="TObject">The type of nwObject.</typeparam>
    /// <typeparam name="TEvent">The event to unsubscribe from.</typeparam>
    [Obsolete("Use the available C# events instead (e.g. NwModule.OnClientEnter)")]
    public void Unsubscribe<TObject, TEvent>(TObject nwObject, Action<TEvent> callback)
      where TEvent : IEvent, new()
      where TObject : NwObject
    {
      eventService.Unsubscribe<TEvent, GameEventFactory>(nwObject, callback);
    }
  }
}
