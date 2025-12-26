/*
 * Find a trigger with the tag "mytrigger" and create a handler for its "OnEnter" event.
 */

using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using NLog;

namespace NWN.Anvil.Samples
{
  [ServiceBinding(typeof(TriggerHandlerService))]
  public class TriggerHandlerService
  {
    // Gets the server log. By default, this reports to "anvil.log"
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public TriggerHandlerService()
    {
      NwTrigger? trigger = NwObject.FindObjectsWithTag<NwTrigger>("mytrigger").FirstOrDefault();
      if (trigger != null)
      {
        trigger.OnEnter += OnTriggerEnter;
      }
    }

    private void OnTriggerEnter(TriggerEvents.OnEnter obj)
    {
      if (obj.EnteringObject.IsPlayerControlled(out NwPlayer? player))
      {
        Log.Info("Player entered trigger: " + player.PlayerName);
      }
    }
  }
}
