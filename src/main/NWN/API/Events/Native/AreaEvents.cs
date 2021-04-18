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
    /// Called when a new <see cref="NwGameObject"/> has entered the <see cref="NwArea"/>.
    /// </summary>
    [NativeEvent(EventScriptType.AreaOnEnter)]
    public sealed class OnEnter : NativeEvent<NwArea, OnEnter>
    {
      /// <summary>
      /// Gets the <see cref="NwArea"/> that was entered.
      /// </summary>
      public NwArea Area { get; private set; }

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that entered the area.
      /// </summary>
      public NwGameObject EnteringObject { get; private set; }

      protected override void PrepareEvent(NwArea objSelf)
      {
        Area = objSelf;
        EnteringObject = NWScript.GetEnteringObject().ToNwObject<NwGameObject>();
      }
    }

    /// <summary>
    /// Called when an <see cref="NwGameObject"/> leaves the <see cref="NwArea"/>.
    /// </summary>
    [NativeEvent(EventScriptType.AreaOnExit)]
    public sealed class OnExit : NativeEvent<NwArea, OnExit>
    {
      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that was left.
      /// </summary>
      public NwArea Area { get; private set; }

      /// <summary>
      /// Gets the <see cref="NwGameObject"/> that left the <see cref="NwArea"/>.
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
    [NativeEvent(EventScriptType.AreaOnHeartbeat)]
    public sealed class OnHeartbeat : NativeEvent<NwArea, OnHeartbeat>
    {
      public NwArea Area { get; private set; }

      protected override void PrepareEvent(NwArea objSelf)
      {
        Area = objSelf;
      }
    }

    [NativeEvent(EventScriptType.AreaOnUserDefinedEvent)]
    public sealed class OnUserDefined : NativeEvent<NwArea, OnUserDefined>
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
