using System.Collections.Generic;
using NWN.API;
using NWN.API.Constants;
using NWN.API.Events;
using NWN.Core;

namespace NWN.Services
{
  [ServiceBinding(typeof(CursorTargetService))]
  public class CursorTargetService
  {
    public delegate void TargetEvent(CursorTargetData targetData);

    private readonly Dictionary<NwPlayer, TargetEvent> awaitingTargetActions = new Dictionary<NwPlayer, TargetEvent>();

    public CursorTargetService(NativeEventService eventService)
    {
      eventService.Subscribe<NwModule, ModuleEvents.OnPlayerTarget>(NwModule.Instance, OnPlayerTarget);
      eventService.Subscribe<NwModule, ModuleEvents.OnClientLeave>(NwModule.Instance, OnPlayerLeave);
    }

    private void OnPlayerLeave(ModuleEvents.OnClientLeave leaveEvent)
    {
      awaitingTargetActions.Remove(leaveEvent.Player);
    }

    /// <summary>
    /// Instructs the specified player to enter cursor targeting mode, invoking the specified handler once the player selects something.
    /// </summary>
    /// <param name="player">The player who should enter selection mode.</param>
    /// <param name="handler">The lamda/method to invoke once this player selects something.</param>
    /// <param name="validTargets">The type of objects that are valid for selection. ObjectTypes is a flags enum, so multiple types may be specified using the OR operator (ObjectTypes.Creature | ObjectTypes.Placeable).</param>
    /// <param name="cursorType">The type of cursor to show if the player is hovering over a valid target.</param>
    /// <param name="badTargetCursor">The type of cursor to show if the player is hovering over an invalid target.</param>
    public void EnterTargetMode(NwPlayer player, TargetEvent handler, ObjectTypes validTargets = ObjectTypes.All, MouseCursor cursorType = MouseCursor.Magic, MouseCursor badTargetCursor = MouseCursor.NoMagic)
    {
      NWScript.EnterTargetingMode(player, (int) validTargets, (int) cursorType, (int) badTargetCursor);
      awaitingTargetActions[player] = handler;
    }

    private void OnPlayerTarget(ModuleEvents.OnPlayerTarget targetEvent)
    {
      if (awaitingTargetActions.TryGetValue(targetEvent.Player, out TargetEvent callback))
      {
        awaitingTargetActions.Remove(targetEvent.Player);

        CursorTargetData eventData = new CursorTargetData(targetEvent);
        callback?.Invoke(eventData);
      }
    }
  }
}
