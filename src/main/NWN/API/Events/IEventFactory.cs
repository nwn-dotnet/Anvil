using NWN.Services;

namespace NWN.API.Events
{
  public interface IEventFactory
  {
    void Init(EventService eventService);

    void Unregister<TEvent>() where TEvent : IEvent;
  }
}
