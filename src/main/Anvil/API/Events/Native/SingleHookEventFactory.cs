using System;
using System.Collections.Generic;
using NWN.Services;

namespace NWN.API.Events
{
  [ServiceBinding(typeof(IEventFactory))]
  public abstract class SingleHookEventFactory<THook> : HookEventFactory, IEventFactory<NullRegistrationData>, IDisposable
    where THook : Delegate
  {
    private readonly HashSet<Type> activeEvents = new HashSet<Type>();

    protected static FunctionHook<THook> Hook { get; set; }

    public void Register<TEvent>(NullRegistrationData data) where TEvent : IEvent, new()
    {
      Hook ??= RequestHook();
      activeEvents.Add(typeof(TEvent));
    }

    public void Unregister<TEvent>() where TEvent : IEvent, new()
    {
      activeEvents.Remove(typeof(TEvent));
      if (activeEvents.Count == 0)
      {
        Dispose();
      }
    }

    public void Dispose()
    {
      Hook?.Dispose();
      Hook = null;
    }

    protected abstract FunctionHook<THook> RequestHook();
  }
}
