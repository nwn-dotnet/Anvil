namespace NWN.API.Events
{
  public interface IEventFactory
  {
    void Init();

    void Unregister<TEvent>() where TEvent : IEvent;
  }
}
