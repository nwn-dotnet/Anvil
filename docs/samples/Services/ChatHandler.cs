/*
 * Implement a "Chat Command System" using a common interface, and binding to the interface.
 */

using System.Collections.Generic;
using System.Linq;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace NWN.Anvil.Samples
{
  // Our base chat command interface...
  public interface IChatCommand
  {
    string Command { get; }
    void ExecuteCommand(NwPlayer caller);
  }

  // ...Each one of our commands implements the IChatCommand interface...
  [ServiceBinding(typeof(IChatCommand))]
  public class GpCommand : IChatCommand
  {
    public string Command { get; } = "!gp";
    private const int AMOUNT = 10000;

    public void ExecuteCommand(NwPlayer caller)
    {
      caller.ControlledCreature.GiveGold(AMOUNT);
    }
  }

  /// ...and uses the interface type instead of the class type inside the ServiceBinding attribute.
  [ServiceBinding(typeof(IChatCommand))]
  public class SaveCommand : IChatCommand
  {
    public string Command { get; } = "!save";

    public void ExecuteCommand(NwPlayer caller)
    {
      caller.ExportCharacter();
      caller.SendServerMessage("Character Saved");
    }
  }

  [ServiceBinding(typeof(ChatHandler))]
  public class ChatHandler
  {
    private readonly List<IChatCommand> chatCommands;

    // We add a dependency to the chat commands created above by defining an IEnumerable parameter of the interface type.
    public ChatHandler(IEnumerable<IChatCommand> commands)
    {
      // Store all define chat commands.
      this.chatCommands = commands.ToList();

      // Subscribe to the global module chat event. When this event occurs, we call the OnChatMessage method.
      NwModule.Instance.OnPlayerChat += OnChatMessage;
    }

    public void OnChatMessage(ModuleEvents.OnPlayerChat eventInfo)
    {
      // Get the message from the event.
      string message = eventInfo.Message;

      // Loop through all of our created commands, and execute the behaviour of the one that matches.
      foreach (IChatCommand command in chatCommands)
      {
        if (command.Command == message)
        {
          command.ExecuteCommand(eventInfo.Sender);
          break;
        }
      }
    }
  }
}
