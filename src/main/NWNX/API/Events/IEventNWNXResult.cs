using NWN.API.Events;

namespace NWNX.API.Events
{
  public interface IEventNWNXResult : IEvent
  {
    internal string EventResult { get; }
  }
}
