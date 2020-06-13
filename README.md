# NWN.NET
NWN.NET is a C# library that attempts to wrap Neverwinter Script with C# niceties and contexts, instead of a collection of functions.

# Getting Started

### Dependencies
NWN.NET requires the following plugins to be enabled in NWNX in-order to run:
```
NWNX_DOTNET_SKIP=n
NWNX_OBJECT_SKIP=n
NWNX_UTIL_SKIP=n
```

Other plugins are optional, but may be required to access some extension APIs. An exception will be raised if you try to use an extension without the dependent plugin loaded.

### Bootstrap
To initialize the managed system, add a small bootstrap class like the following:

```csharp
using System;

namespace NWN
{
  public static class Internal
  {
    public static int Bootstrap(IntPtr arg, int argLength)
    {
      return NManager.Init(arg, argLength);
    }
  }
}
```

The class path should match the `ENTRYPOINT` environmental variable as defined in the [NWNX:EE config](https://nwnxee.github.io/unified/group__dotnet.html#dotnet). By default this is `NWN.Internal`.

# Services
The core of NWN.NET is built around a dependency injection model that is setup using class attributes. The system expects you to implement features in a similar way:

**Example: Basic Script Handler**
```csharp
  using NLog;
  using NWN.API;
  using NWN.Services;
  
  // This attribute indicates this class should be constructed on start, and available as a dependency "MyScriptHandler"
  // You can also bind yourself to an interface or base class. The system also supports multiple bindings.
  [ServiceBinding(typeof(MyScriptHandler))]
  public class MyScriptHandler
  {
    // Gets the server log. By default, this reports to "nwm.log"
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    // This function will be called as if the same script was called by a toolset event, or by another script.
    // Script name must be <= 16 characters similar to the toolset.
    // This function must always return void, or a bool in the case of a conditional.
    // The NwObject parameter is optional, but if defined, must always be a single parameter of the NWObject type.
    [ScriptHandler("test_nwscript")]
    private void OnScriptCalled(NwObject objSelf)
    {
      Log.Info($"test_nwscript called by {objSelf.Name}");
    }
  }
```

**Example: Chat Command System**
```csharp
  using System.Collections.Generic;
  using System.Linq;
  using NWN.API;
  using NWN.API.Events;
  using NWN.Services;

  public interface IChatCommand
  {
    string Command { get; }
    void ExecuteCommand(NwPlayer caller);
  }

  // Binds our first command to the IChatCommand interface so we can process them.
  [ServiceBinding(typeof(IChatCommand))]
  public class GpCommand : IChatCommand
  {
    public string Command { get; } = "!gp";
    private const int AMOUNT = 10000;

    public void ExecuteCommand(NwPlayer caller)
    {
      caller.Gold += AMOUNT;
    }
  }

  // Bind a second command to the same interface.
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

    // In our class constructor we define our dependencies.
    // In this case, we require the EventService to subscribe to the module chat event.
    // And we also inject all of the chat commands we bound above.
    public ChatHandler(EventService eventService, IEnumerable<IChatCommand> commands)
    {
      this.chatCommands = commands.ToList();
      eventService.Subscribe<ModuleEvents.OnPlayerChat>(OnChatMessage);
    }

    public void OnChatMessage(ModuleEvents.OnPlayerChat eventInfo)
    {
      // Get the message from the event.
      string message = eventInfo.Message;

      // Loop through all of our created commands, and execute the one that matches.
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
```

## Core Services
(WIP)

### Event Service (NWN.Services.EventService)
**Send a pink welcome message to a player when they connect**
```csharp
  using NWN.API;
  using NWN.API.Events;
  using NWN.Services;

  [ServiceBinding(typeof(WelcomeMessageService))]
  public class WelcomeMessageService
  {
    public WelcomeMessageService(EventService eventService)
    {
      eventService.Subscribe<NwModule, ModuleEvents.OnClientEnter>(NwModule.Instance, OnClientEnter);
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter eventInfo)
    {
      eventInfo.Player.SendServerMessage($"Welcome to the server, {eventInfo.Player.Name}!", Color.PINK);
    }
  }
```

### Scheduler Service (NWN.Services.SchedulerService)

### Loop Service (NWN.Services.LoopService)

### 2DA Factory (NWN.Services.TwoDimArrayFactory)
