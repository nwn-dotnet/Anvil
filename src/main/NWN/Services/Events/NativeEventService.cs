using System;
using NWN.API;
using NWN.API.Events;

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
      // To maintain previous behaviour, we invoke "SubscribeAll" for module events, which previously did not have an object context.
      if (nwObject is NwModule)
      {
        eventService.SubscribeAll<TEvent, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(nwObject), callback);
      }
      else
      {
        eventService.Subscribe<TEvent, GameEventFactory, GameEventFactory.RegistrationData>(nwObject, new GameEventFactory.RegistrationData(nwObject), callback);
      }
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
      // To maintain previous behaviour, we invoke "UnsubscribeAll" for module events, which previously did not have an object context.
      if (nwObject is NwModule)
      {
        eventService.UnsubscribeAll<TEvent, GameEventFactory>(callback);
      }
      else
      {
        eventService.Unsubscribe<TEvent, GameEventFactory>(nwObject, callback);
      }
    }
  }
}
