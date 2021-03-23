namespace NWN.API.Events
{
  public interface IEventSkippable : IEvent
  {
    /// <summary>
    /// Gets or sets a value indicating whether this event will be skipped.
    /// </summary>
    public bool Skip { get; set; }
  }
}
