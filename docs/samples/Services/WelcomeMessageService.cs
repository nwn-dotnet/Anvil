/*
 * Send a welcome message to each player that joins the server.
 */

using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace NWN.Anvil.Samples
{
  [ServiceBinding(typeof(WelcomeMessageService))]
  public class WelcomeMessageService
  {
    public WelcomeMessageService()
    {
      NwModule.Instance.OnClientEnter += OnClientEnter;
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter onEnter)
    {
      onEnter.Player.SendServerMessage($"Welcome to the server, {onEnter.Player}!", ColorConstants.Pink);
    }
  }
}
