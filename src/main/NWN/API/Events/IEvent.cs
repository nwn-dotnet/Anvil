using System;
using NWN.Services;

namespace NWN.API.Events
{
  public interface IEvent
  {
    bool HasSubscribers { get; }

    ScriptHandleResult Broadcast(NwObject objSelf);

    void ClearSubscribers();
  }

  public interface IEvent<out T> : IEvent
  {
    void Subscribe(Action<T> callback);

    void Unsubscribe(Action<T> callback);
  }
}
