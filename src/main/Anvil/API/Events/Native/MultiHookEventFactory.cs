using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;

namespace NWN.API.Events
{
  [ServiceBinding(typeof(IEventFactory))]
  public abstract class MultiHookEventFactory : HookEventFactory, IEventFactory<NullRegistrationData>, IDisposable
  {
    private readonly HashSet<Type> activeEvents = new HashSet<Type>();
    private IDisposable[] hooks;

    public void Register<TEvent>(NullRegistrationData data) where TEvent : IEvent, new()
    {
      hooks ??= RequestHooks();
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
      hooks.DisposeAll();
      hooks = null;
    }

    protected abstract IDisposable[] RequestHooks();
  }
}
