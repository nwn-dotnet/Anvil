namespace Anvil.API.Events
{
  /// <summary>
  /// Defines an event payload that can be skipped before default processing.
  /// </summary>
  public interface IEventSkippable : IEvent
  {
    /// <summary>
    /// Set to true to skip the event's default processing.
    /// </summary>
    public bool Skip { get; set; }
  }
}
