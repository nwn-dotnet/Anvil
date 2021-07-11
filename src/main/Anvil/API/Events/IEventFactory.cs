namespace NWN.API.Events
{
  public interface IEventFactory
  {
    void Unregister<TEvent>() where TEvent : IEvent, new();
  }

  public interface IEventFactory<in TRegisterData> : IEventFactory
  {
    void Register<TEvent>(TRegisterData data) where TEvent : IEvent, new();
  }
}
