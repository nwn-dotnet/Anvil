using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Base interface for all game and engine events managed by the <see cref="EventService"/>.
  /// </summary>
  public interface IEvent
  {
    /// <summary>
    /// Gets the context object used to execute callbacks.
    /// </summary>
    /// <remarks>
    /// This object is used to filter events when subscribing for an event on a specific object.<br/>
    /// Additionally, if the event is executed in a VM context, this object will be set as OBJECT_SELF.
    /// </remarks>
    NwObject? Context { get; }
  }
}
