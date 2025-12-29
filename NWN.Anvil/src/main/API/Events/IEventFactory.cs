namespace Anvil.API.Events
{
  /// <summary>
  /// Defines an event factory capable of managing event subscriptions/lifecycles.
  /// </summary>
  public interface IEventFactory
  {
    /// <summary>
    /// Unregisters the specified event type. This is triggered when no subscribers remain for a specific event type.
    /// </summary>
    /// <remarks>
    /// Event factories may cleanup hooks or any other registrations here.
    /// </remarks>
    /// <typeparam name="TEvent">The event payload type to unregister.</typeparam>
    void Unregister<TEvent>() where TEvent : IEvent, new();
  }

  /// <summary>
  /// Defines a typed event factory that manages event subscriptions/lifecycles with additional metadata related to the event.
  /// </summary>
  /// <typeparam name="TRegisterData">Additional registration metadata required by the factory.</typeparam>
  public interface IEventFactory<in TRegisterData> : IEventFactory
  {
    /// <summary>
    /// Registers the specified event type using the provided registration metadata.
    /// </summary>
    /// <typeparam name="TEvent">The event payload type to register.</typeparam>
    /// <param name="data">The registration metadata for the factory.</param>
    void Register<TEvent>(TRegisterData data) where TEvent : IEvent, new();
  }
}
