# NWN.Managed
NWN.Managed is a C# library that attempts to wrap Neverwinter Script with C# niceties and contexts, instead of a collection of functions. It is a managed implementation of [NWN.Core](https://github.com/nwn-dotnet/NWN.Core).

# Getting Started

### Running NWN.Managed
1. Download the latest [Release](https://github.com/nwn-dotnet/NWN.Managed/releases) for your server version.
2. Extract the Binaries to a folder accessible by the server.
3. Configure NWNX options to the following:

```sh
NWNX_DOTNET_SKIP=n
NWNX_OBJECT_SKIP=n
NWNX_UTIL_SKIP=n
NWNX_DOTNET_ASSEMBLY=/your/path/to/NWN.Managed # Where "NWN.Managed.dll" was extracted in step 2, without the extension. E.g: NWNX_DOTNET_ASSEMBLY=/nwn/home/modbin/NWN.Managed
# NWNX_DOTNET_ENTRYPOINT= # Make sure this option does not exist in your config
```

The DotNET, Object and Util plugins are required for the library to work. Make sure they are enabled!

Other plugins are optional, but may be required to access some extension APIs. An exception will be raised if you try to use an extension without the dependent plugin loaded.

For a step by step guide how to set up a local developement environment using your IDE of choice and windows. see [Development with Docker on Windows](Development_with_Docker_on_Windows.md).

# Plugins & Services
Adding module behaviours starts by creating your own plugin assembly (.dll).

To get started, it is recommended to start by making a copy of the sample project found [HERE](https://github.com/nwn-dotnet/NWN.Samples/tree/master/managed/plugin-sample) with the package dependencies already setup for you.

The core of NWN.Managed is built around a dependency injection model, and the library expects you to implement features in your plugins a similar way.

Using a class attribute (ServiceBinding), the system will automatically wire up all of the dependencies for that class as defined in the parameters of its constructor:

**Example: Basic Script Handler**
```csharp
  using NLog;
  using NWN.API;
  using NWN.Services;

  // The "ServiceBinding" attribute indicates this class should be created on start, and available to other classes as a dependency "MyScriptHandler"
  // You can also bind yourself to an interface or base class. The system also supports multiple bindings.
  [ServiceBinding(typeof(MyScriptHandler))]
  public class MyScriptHandler
  {
    // Gets the server log. By default, this reports to "nwm.log"
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly NativeEventService eventService;

    // As this class has the ServiceBinding attribute, the constructor of this class will be called during server startup.
    // The EventService is a core service from NWN.Managed. As it is defined as a constructor parameter, it will be injected during startup.
    public MyScriptHandler(NativeEventService eventService)
    {
      this.eventService = eventService;
    }

    // This function will be called as if the same script was called by a toolset event, or by another script.
    // Script name must be <= 16 characters similar to the toolset.
    // This function must always return void, or a bool in the case of a conditional.
    // The NwObject parameter is optional, but if defined, must always be a single parameter of the NWObject type.
    [ScriptHandler("test_nwscript")]
    private void OnScriptCalled(CallInfo callInfo)
    {
      Log.Info($"test_nwscript called by {callInfo.ObjectSelf.Name}");
    }
  }
```

**Example: Chat Command System using interfaces**
```csharp
  using System.Collections.Generic;
  using System.Linq;
  using NWN.API;
  using NWN.API.Events;
  using NWN.Services;

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
      caller.Gold += AMOUNT;
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

    // We set the EventService as a dependency so we can subscribe to the module chat event.
    // And we add a dependency to the chat commands created above by defining an IEnumerable parameter of the interface type.
    public ChatHandler(NativeEventService eventService, IEnumerable<IChatCommand> commands)
    {
      // Store all define chat commands.
      this.chatCommands = commands.ToList();

      // Using the event service, subscribe to the global module chat event. When this event occurs, we call the OnChatMessage method.
      eventService.Subscribe<NwModule, ModuleEvents.OnPlayerChat>(NwModule.Instance, OnChatMessage);
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
```

**Example: Find a trigger by tag and attach OnEnter event**
```csharp
using System.Linq;
using NLog;
using NWN.API;
using NWN.API.Events;
using NWN.Services;

namespace Sample
{
    // [ServiceBinding] indicates that this class will be created during server startup.
    [ServiceBinding(typeof(MyPluginService))]
    public class MyPluginService
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        // Called at startup. NWN.Managed resolves EventService for us.
        public MyPluginService(NativeEventService eventService)
        {
            var trigger = NwObject.FindObjectsWithTag<NwTrigger>("mytrigger").FirstOrDefault();
            eventService.Subscribe<NwTrigger, TriggerEvents.OnEnter>(trigger, OnTriggerEnter);
        }

        private void OnTriggerEnter(TriggerEvents.OnEnter obj)
        {
            if (obj.EnteringObject is NwPlayer player)
            {
                Log.Info("Player entered trigger: " + player?.PlayerName);
            }
        }
    }
}
```

## Core Services
### Event Service (NWN.Services.EventService)
**Send a pink welcome message to a player when they connect**
```csharp
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

    private void OnClientEnter(ModuleEvents.OnClientEnter eventInfo)
    {
      eventInfo.Player.SendServerMessage($"Welcome to the server, {eventInfo.Player.Name}!", Color.PINK);
    }
  }
```

### Async Tasks (NWN.API.NwTask)
```csharp
  using System;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;
  using NWN.API;
  using NWN.Services;

  [ServiceBinding(typeof(MyAsyncService))]
  public class MyAsyncService
  {
    public MyAsyncService()
    {
      DoAsyncStuff();
    }

    private async void DoAsyncStuff()
    {
      // Do some heavy work on another thread using a standard task, then return to a safe script context.
      await Task.Run(() => Thread.Sleep(1000));
      await NwTask.SwitchToMainThread();

      // Wait for a frame, or a certain amount of frames to pass.
      await NwTask.NextFrame();
      await NwTask.DelayFrame(100);

      // Wait for 30 seconds to pass. (DelayCommand replacement)
      await NwTask.Delay(TimeSpan.FromSeconds(30));

      // Wait for a certain game period to pass.
      await NwTask.Delay(NwTimeSpan.FromRounds(2));
      await NwTask.Delay(NwTimeSpan.FromTurns(3));
      await NwTask.Delay(NwTimeSpan.FromHours(1));

      // Wait for an expression to evaluate to true
      await NwTask.WaitUntil(() => NwModule.Instance.Players.Count() > 5);

      // Wait for a value to change.
      await NwTask.WaitUntilValueChanged(() => NwModule.Instance.Players.Count());

      // Start some tasks.
      Task task1 = Task.Run(() => true); // Executed in the thread pool, you cannot use NWN APIs here.

      Task task2 = Task.Run(async () =>
      {
        // Executed in the thread pool, you cannot use NWN APIs here.
        await Task.Delay(TimeSpan.FromSeconds(5));
        return 20;
      });

      Task task3 = NwTask.Run(async () =>
      {
        // Executed in the server thread, you can use NWN APIs here.
        await NwTask.Delay(NwTimeSpan.FromRounds(5));
        NwModule.Instance.SendMessageToAllDMs("5 rounds elapsed!");
        return 20;
      });

      // ...wait for any of them to complete.
      await NwTask.WhenAny(task1, task2, task3);

      // ...wait for all of them to complete.
      await NwTask.WhenAll(task1, task2, task3);
    }
  }
```

### Loop Service (NWN.Services.LoopService)
**Report the tick rate of the server every server loop.**
```csharp
  [ServiceBinding(typeof(IUpdateable))]
  [ServiceBinding(typeof(PerformanceReportService))]
  public class PerformanceReportService : IUpdateable
  {
    public void Update()
    {
      Console.WriteLine($"Current tick rate: {Util.ServerTicksPerSecond}");
    }
  }
```

### 2DA Factory (NWN.Services.TwoDimArrayFactory)
**Report the amount of XP the player has until their next level.**
```csharp
  // This is the deserialization class for this specific type of 2da.
  // We can implement our own helper functions here that operate on the 2da data, and cache it.
  public class ExpTable : ITwoDimArray
  {
    private readonly List<Entry> entries = new List<Entry>();

    /// <summary>
    /// Gets the max possible player level.
    /// </summary>
    public int MaxLevel => entries[^1].Level;

    /// <summary>
    /// Gets the amount of XP needed for the specified level.
    /// </summary>
    /// <param name="level">The level to lookup.</param>
    public int GetXpForLevel(int level)
    {
      return entries.First(entry => entry.Level == level).XP;
    }

    /// <summary>
    /// Gets the current level for a player with the specified XP.
    /// </summary>
    /// <param name="xp">The amount of xp.</param>
    public int GetLevelFromXp(int xp)
    {
      int level = 1;
      foreach (Entry entry in entries)
      {
        if (entry.XP > xp)
        {
          break;
        }

        level = entry.Level;
      }

      return level;
    }

    void ITwoDimArray.DeserializeRow(int rowIndex, TwoDimEntry twoDimEntry)
    {
      // Use twoDimEntry(columnName) to get your serialized data, then convert it here.
      int level = int.Parse(twoDimEntry("Level"));
      uint xp = ParseXpColumn(twoDimEntry("XP"));

      if (xp > int.MaxValue)
      {
        return;
      }

      entries.Add(new Entry(level, (int) xp));
    }

    private uint ParseXpColumn(string value)
    {
      return uint.TryParse(value, out uint retVal) ? retVal : uint.Parse(value.Substring(2), NumberStyles.AllowHexSpecifier);
    }

    private readonly struct Entry
    {
      public readonly int Level;
      public readonly int XP;

      public Entry(int level, int xp)
      {
        Level = level;
        XP = xp;
      }
    }
  }

  [ServiceBinding(typeof(XPReportService))]
  public class XPReportService
  {
    private readonly ExpTable expTable;

    public XPReportService(NativeEventService eventService, TwoDimArrayFactory twoDimArrayFactory)
    {
      eventService.Subscribe<NwModule, ModuleEvents.OnClientEnter>(NwModule.Instance, OnClientEnter);
      expTable = twoDimArrayFactory.Get2DA<ExpTable>("exptable");
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter onClientEnter)
    {
      NwPlayer player = onClientEnter.Player;
      int nextLevel = expTable.GetLevelFromXp(player.Xp) + 1;
      if (nextLevel > expTable.MaxLevel)
      {
        return;
      }

      player.SendServerMessage($"Next level up: {expTable.GetXpForLevel(nextLevel) - player.Xp}");
    }
  }
```