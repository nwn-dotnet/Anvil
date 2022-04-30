using System;
using Anvil.API.Events;
using Anvil.Services;
using NWN.Core;

namespace Anvil.API.Events
{
  /// <summary>
  /// Global module events.
  /// </summary>
  public static partial class ModuleEvents
  {
    [Inject]
    private static NuiWindowEventService NuiWindowEventService { get; set; }

    /// <summary>
    /// Called when a player triggers an event in the NUI system.
    /// </summary>
    [GameEvent(EventScriptType.ModuleOnNuiEvent)]
    public sealed class OnNuiEvent : IEvent
    {
      private readonly string eventPayload;

      public OnNuiEvent()
      {
        Json payload = NWScript.NuiGetEventPayload();
        eventPayload = payload.Dump();

        EventType = NWScript.NuiGetEventType() switch
        {
          "click" => NuiEventType.Click,
          "watch" => NuiEventType.Watch,
          "open" => NuiEventType.Open,
          "close" => NuiEventType.Close,
          "focus" => NuiEventType.Focus,
          "blur" => NuiEventType.Blur,
          "mousedown" => NuiEventType.MouseDown,
          "mouseup" => NuiEventType.MouseUp,
          _ => NuiEventType.Unknown,
        };

        Token = new NuiWindowToken(Player, NWScript.NuiGetEventWindow());
      }

      /// <summary>
      /// Get the array index of the current event.<br/>
      /// This can be used to get the index into an array, for example when rendering lists of buttons.<br/>
      /// Returns -1 if the event is not originating from within an array.
      /// </summary>
      public int ArrayIndex { get; } = NWScript.NuiGetEventArrayIndex();

      public NwObject Context => Player?.ControlledCreature;

      /// <summary>
      /// Gets the ID of the <see cref="NuiElement"/> that triggered the event.
      /// </summary>
      public string ElementId { get; } = NWScript.NuiGetEventElement();

      /// <summary>
      /// Gets the type of Nui event that occurred.
      /// </summary>
      public NuiEventType EventType { get; }

      /// <summary>
      /// Gets the player that triggered this event.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.NuiGetEventPlayer().ToNwPlayer();

      /// <summary>
      /// Gets the window token associated with this event.
      /// </summary>
      public NuiWindowToken Token { get; }

      /// <summary>
      /// Gets the window token associated with this event.
      /// </summary>
      [Obsolete("Use Token instead.")]
      public int WindowToken { get; } = NWScript.NuiGetEventWindow();

      /// <summary>
      /// Gets the payload data associated with this event.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <returns>The payload data, or null if the event has no payload.</returns>
      public T GetEventPayload<T>()
      {
        return JsonUtility.FromJson<T>(eventPayload);
      }
    }
  }
}

namespace Anvil.API
{
  public sealed partial class NwModule
  {
    /// <inheritdoc cref="ModuleEvents.OnNuiEvent"/>
    public event Action<ModuleEvents.OnNuiEvent> OnNuiEvent
    {
      add => EventService.SubscribeAll<ModuleEvents.OnNuiEvent, GameEventFactory, GameEventFactory.RegistrationData>(new GameEventFactory.RegistrationData(this), value);
      remove => EventService.UnsubscribeAll<ModuleEvents.OnNuiEvent, GameEventFactory>(value);
    }
  }

  public sealed partial class NwPlayer
  {
    /// <inheritdoc cref="ModuleEvents.OnNuiEvent"/>
    public event Action<ModuleEvents.OnNuiEvent> OnNuiEvent
    {
      add => EventService.Subscribe<ModuleEvents.OnNuiEvent, GameEventFactory, GameEventFactory.RegistrationData>(ControlledCreature, new GameEventFactory.RegistrationData(NwModule.Instance), value);
      remove => EventService.Unsubscribe<ModuleEvents.OnNuiEvent, GameEventFactory>(ControlledCreature, value);
    }
  }

  public readonly partial struct NuiWindowToken
  {
    /// <inheritdoc cref="ModuleEvents.OnNuiEvent"/>
    public event Action<ModuleEvents.OnNuiEvent> OnNuiEvent
    {
      add => NuiWindowEventService.Subscribe(this, value);
      remove => NuiWindowEventService.Unsubscribe(this, value);
    }
  }
}
