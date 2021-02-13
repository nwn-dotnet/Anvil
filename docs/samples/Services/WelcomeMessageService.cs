/*
 * Send a welcome message to each player that joins the server.
 */

using NWN.API;
using NWN.API.Events;
using NWN.Services;

[ServiceBinding(typeof(WelcomeMessageService))]
public class WelcomeMessageService
{
  public WelcomeMessageService(NativeEventService eventService)
  {
    eventService.Subscribe<NwModule, ModuleEvents.OnClientEnter>(NwModule.Instance, OnClientEnter);
  }

  private void OnClientEnter(ModuleEvents.OnClientEnter onEnter)
  {
    onEnter.Player.SendServerMessage($"Welcome to the server, {onEnter.Player}!", Color.PINK);
  }
}
