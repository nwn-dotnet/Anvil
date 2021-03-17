using NWN.API.Constants;
using NWN.Core;

// TODO Populate event data.
namespace NWN.API.Events
{
  /// <summary>
  /// Events for Areas.
  /// </summary>
  public static class AreaEvents
  {
    /// <summary>
    /// Called when a new object has entered the area.
    /// </summary>
    [NativeEvent(EventScriptType.AreaOnEnter)]
    public sealed record OnEnter : IEvent
    {
      /// <summary>
      /// Gets the area that was entered.
      /// </summary>
      public readonly NwArea Area = NWScript.OBJECT_SELF.ToNwObject<NwArea>();

      /// <summary>
      /// Gets the game object that entered the area.
      /// </summary>
      public readonly NwGameObject EnteringObject = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Area;
    }

    /// <summary>
    /// Called when an object leaves the area.
    /// </summary>
    [NativeEvent(EventScriptType.AreaOnExit)]
    public sealed record OnExit : IEvent
    {
      /// <summary>
      /// Gets the area that was left.
      /// </summary>
      public NwArea Area { get; }

      /// <summary>
      /// Gets the game object that left the area.
      /// </summary>
      public NwGameObject ExitingObject { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Area;

      public OnExit()
      {
        Area = NWScript.OBJECT_SELF.ToNwObject<NwArea>();
        ExitingObject = NWScript.GetExitingObject().ToNwObject<NwGameObject>();
      }
    }

    /// <summary>
    /// Called at a regular interval (approx. 6 seconds).
    /// </summary>
    [NativeEvent(EventScriptType.AreaOnHeartbeat)]
    public sealed record OnHeartbeat : IEvent
    {
      public NwArea Area { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Area;

      public OnHeartbeat()
      {
        Area = NWScript.OBJECT_SELF.ToNwObject<NwArea>();
      }
    }

    [NativeEvent(EventScriptType.AreaOnUserDefinedEvent)]
    public sealed record OnUserDefined : IEvent
    {
      public NwArea Area { get; }

      public int EventNumber { get; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Area;

      public OnUserDefined()
      {
        Area = NWScript.OBJECT_SELF.ToNwObject<NwArea>();
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }
  }
}
