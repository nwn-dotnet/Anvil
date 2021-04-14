using System;
using NWN.API;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;

namespace NWN.Services
{
  /// <summary>
  /// A managed implementation of selection/target mode logic utilising C# style callbacks.
  /// </summary>
  [ServiceBinding(typeof(CursorTargetService))]
  public class CursorTargetService
  {
    /// <summary>
    /// Instructs the specified player to enter cursor targeting mode, invoking the specified handler once the player selects something.
    /// </summary>
    /// <param name="player">The player who should enter selection mode.</param>
    /// <param name="handler">The lamda/method to invoke once this player selects something.</param>
    /// <param name="validTargets">The type of objects that are valid for selection. ObjectTypes is a flags enum, so multiple types may be specified using the OR operator (ObjectTypes.Creature | ObjectTypes.Placeable).</param>
    /// <param name="cursorType">The type of cursor to show if the player is hovering over a valid target.</param>
    /// <param name="badTargetCursor">The type of cursor to show if the player is hovering over an invalid target.</param>
    public void EnterTargetMode(NwPlayer player, Action<ModuleEvents.OnPlayerTarget> handler, ObjectTypes validTargets = ObjectTypes.All, MouseCursor cursorType = MouseCursor.Magic, MouseCursor badTargetCursor = MouseCursor.NoMagic)
    {
      NWScript.EnterTargetingMode(player, (int) validTargets, (int) cursorType, (int) badTargetCursor);
      player.OnCursorTarget -= handler;
      player.OnCursorTarget += handler;
    }
  }
}
