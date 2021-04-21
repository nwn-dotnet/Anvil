using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for <see cref="NwArea"/>.
  /// </summary>
  public static class AreaEvents
  {
    /// <summary>
    /// Called when a new <see cref="NwGameObject"/> has entered the <see cref="NwArea"/>.
    /// </summary>
    [GameEvent(EventScriptType.AreaOnEnter)]
    public sealed class OnEnter : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwArea"/> that was entered.
      /// </summary>
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that entered the <see cref="NwArea"/>.
      /// </summary>
      public NwGameObject EnteringObject { get; } = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();

      NwObject IEvent.Context => Area;
    }

    /// <summary>
    /// Called when an <see cref="NwGameObject"/> leaves the <see cref="NwArea"/>.
    /// </summary>
    [GameEvent(EventScriptType.AreaOnExit)]
    public sealed class OnExit : IEvent
    {
      /// <summary>
      /// Gets the <see cref="NwArea"/> that was left.
      /// </summary>
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>();

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that left the <see cref="NwArea"/>.<br/>
      /// If this is a disconnecting player, this value will be a <see cref="NwCreature"/>. See <see cref="IsDisconnectingPlayer"/> to determine this state.
      /// </summary>
      public NwGameObject ExitingObject { get; } = NWScript.GetExitingObject().ToNwObject<NwGameObject>();

      /// <summary>
      /// Gets a value indicating whether the <see cref="ExitingObject"/> is a player leaving the server.
      /// </summary>
      public bool IsDisconnectingPlayer
      {
        get
        {
          if (ExitingObject is NwCreature && !(ExitingObject is NwPlayer))
          {
            string objectId = ExitingObject.ToString();
            return objectId.Length == 8 && objectId.StartsWith("7ff");
          }

          return false;
        }
      }

      NwObject IEvent.Context => Area;
    }

    /// <summary>
    /// Called at a regular interval (approx. 6 seconds).
    /// </summary>
    [GameEvent(EventScriptType.AreaOnHeartbeat)]
    public sealed class OnHeartbeat : IEvent
    {
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>();

      NwObject IEvent.Context => Area;
    }

    [GameEvent(EventScriptType.AreaOnUserDefinedEvent)]
    public sealed class OnUserDefined : IEvent
    {
      public NwArea Area { get; } = NWScript.OBJECT_SELF.ToNwObject<NwArea>();

      public int EventNumber { get; } = NWScript.GetUserDefinedEventNumber();

      NwObject IEvent.Context => Area;

      public static void Signal(NwArea area, int eventId)
      {
        Event nwEvent = NWScript.EventUserDefined(eventId);
        NWScript.SignalEvent(area, nwEvent);
      }
    }
  }
}
