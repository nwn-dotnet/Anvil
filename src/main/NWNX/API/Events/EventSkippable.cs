using NWN.API.Events;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public abstract class EventSkippable<T> : Event<T> where T : Event<T>
  {
    /// <summary>
    /// Gets or sets a value indicating whether this event will be skipped.
    /// </summary>
    public bool Skip { get; set; }

    protected override void InvokeCallbacks()
    {
      Skip = false;
      base.InvokeCallbacks();

      if (Skip)
      {
        EventsPlugin.SkipEvent();
      }
    }
  }
}
