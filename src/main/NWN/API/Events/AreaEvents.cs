using NWN.API.Constants;
using NWN.Core;

namespace NWN.API.Events
{
  // TODO Populate event data.
  /// <summary>
  /// Events for Areas.
  /// </summary>
  public static class AreaEvents
  {
    /// <summary>
    /// Called when a new object has entered the area.
    /// </summary>
    [ScriptEvent(EventScriptType.AreaOnEnter)]
    public sealed class OnEnter : Event<NwArea, OnEnter>
    {
      /// <summary>
      /// Gets the area that was entered.
      /// </summary>
      public NwArea Area { get; private set; }

      /// <summary>
      /// Gets the game object that entered the area.
      /// </summary>
      public NwGameObject EnteringObject { get; private set; }

      protected override void PrepareEvent(NwArea objSelf)
      {
        Area = objSelf;
        EnteringObject = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();
      }
    }

    /// <summary>
    /// Called when an object leaves the area.
    /// </summary>
    [ScriptEvent(EventScriptType.AreaOnExit)]
    public sealed class OnExit : Event<NwArea, OnExit>
    {
      /// <summary>
      /// Gets the area that was left.
      /// </summary>
      public NwArea Area { get; private set; }

      /// <summary>
      /// Gets the game object that left the area.
      /// </summary>
      public NwGameObject ExitingObject { get; private set; }

      protected override void PrepareEvent(NwArea objSelf)
      {
        Area = objSelf;
        ExitingObject = NWScript.GetExitingObject().ToNwObject<NwGameObject>();
      }
    }

    /// <summary>
    /// Called at a regular interval (approx. 6 seconds).
    /// </summary>
    [ScriptEvent(EventScriptType.AreaOnHeartbeat)]
    public sealed class OnHeartbeat : Event<NwArea, OnHeartbeat>
    {
      public NwArea Area { get; private set; }

      protected override void PrepareEvent(NwArea objSelf)
      {
        Area = objSelf;
      }
    }

    [ScriptEvent(EventScriptType.AreaOnUserDefinedEvent)]
    public sealed class OnUserDefined : Event<NwArea, OnUserDefined>
    {
      public NwArea Area { get; private set; }
      public int EventNumber { get; private set; }

      protected override void PrepareEvent(NwArea objSelf)
      {
        Area = objSelf;
        EventNumber = NWScript.GetUserDefinedEventNumber();
      }
    }
  }
}