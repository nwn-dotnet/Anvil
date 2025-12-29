using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Anvil.Services;

namespace Anvil.API.Events
{
  /// <summary>
  /// Provides a base implementation for native function hook events.<br/>
  /// See the documentation on FunctionHooks for how to use this class.
  /// </summary>
  [ServiceBinding(typeof(IEventFactory))]
  public abstract class HookEventFactory : IEventFactory<NullRegistrationData>, IDisposable
  {
    [Inject]
    protected static Lazy<EventService> EventService { get; private set; } = null!;

    [Inject]
    protected static HookService HookService { get; private set; } = null!;

    [Inject]
    protected static VirtualMachine VirtualMachine { get; private set; } = null!;

    private readonly HashSet<Type> activeEvents = [];
    private IDisposable[]? hooks;

    public void Dispose()
    {
      hooks.DisposeAll();
      hooks = null;
    }

    void IEventFactory<NullRegistrationData>.Register<TEvent>(NullRegistrationData data)
    {
      hooks ??= RequestHooks();
      activeEvents.Add(typeof(TEvent));
    }

    void IEventFactory.Unregister<TEvent>()
    {
      activeEvents.Remove(typeof(TEvent));
      if (activeEvents.Count == 0)
      {
        Dispose();
      }
    }

    /// <summary>
    /// Processes event callbacks, optionally within the NWScript VM context.
    /// </summary>
    /// <typeparam name="TEvent">The event payload type.</typeparam>
    /// <param name="eventType">The callback phase.</param>
    /// <param name="eventData">The event payload instance; ignored when null.</param>
    /// <param name="executeInScriptContext">If true (default), executes the event handlers within a new NWScript VM context, with <see cref="IEvent.Context"/> as OBJECT_SELF.</param>
    /// <returns>The original event payload (or null when input was null).</returns>
    [return: NotNullIfNotNull("eventData")]
    protected static TEvent? ProcessEvent<TEvent>(EventCallbackType eventType, TEvent? eventData, bool executeInScriptContext = true) where TEvent : class, IEvent
    {
      if (eventData == null)
      {
        return null;
      }

      if (executeInScriptContext)
      {
        VirtualMachine.ExecuteInScriptContext(() =>
        {
          eventData = EventService.Value.ProcessEvent(eventType, eventData);
        }, eventData.Context);
      }
      else
      {
        EventService.Value.ProcessEvent(eventType, eventData);
      }

      return eventData;
    }

    /// <summary>
    /// Requests and returns the native hooks required for this factory. Called once on first subscription.
    /// </summary>
    /// <returns>An array of disposables representing all function hooks associated with this event factory.</returns>
    protected abstract IDisposable[] RequestHooks();
  }
}
