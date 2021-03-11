using NWN.Services;

namespace NWN.API.Events
{
  public interface IEventFactory
  {
    void Init(EventService eventService);
    void Register<TEvent>(NwObject obj) where TEvent : IEvent, new();
    void Unregister<TEvent>() where TEvent : IEvent;
  }
}
