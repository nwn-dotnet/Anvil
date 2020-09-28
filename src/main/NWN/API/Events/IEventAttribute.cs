using NWN.Services;

namespace NWN.API.Events
{
  public interface IEventAttribute
  {
    void InitHook(string scriptName);

    void InitObjectHook<TObject, TEvent>(EventHandler eventHandler, TObject nwObject, string scriptName) where TEvent : Event<TObject, TEvent>, new() where TObject : NwObject;
  }
}
