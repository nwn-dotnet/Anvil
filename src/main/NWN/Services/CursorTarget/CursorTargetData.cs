using System.Numerics;
using NWN.API;
using NWN.API.Events;

namespace NWN.Services
{
  /// <summary>
  /// The selection info returned from the CursorTargetService when a selection is made.
  /// </summary>
  public class CursorTargetData
  {
    /// <summary>
    /// The player who made this selection.
    /// </summary>
    public NwPlayer Player;

    /// <summary>
    /// Gets the object the player selected.<br/>
    /// If the player selected a position, this is the Area that the position is relative to.
    /// </summary>
    public NwObject TargetObj { get; }

    /// <summary>
    /// Gets the position the player selected, or the position of the object the player selected.
    /// </summary>
    public Vector3 TargetPos { get; }

    internal CursorTargetData(ModuleEvents.OnPlayerTarget targetEvent)
    {
      Player = targetEvent.Player;
      TargetObj = targetEvent.TargetObject;
      TargetPos = targetEvent.TargetPosition;
    }
  }
}
