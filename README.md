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
      await NwTask.Delay(GameTimeSpan.FromRounds(2));
      await NwTask.Delay(GameTimeSpan.FromTurns(3));
      await NwTask.Delay(GameTimeSpan.FromHours(1));

      // Wait for an expression to evaluate to true
      await NwTask.WaitUntil(() => NwModule.Instance.Players.Count() > 5);

      // Wait for a value to change.
      await NwTask.WaitUntilValueChanged(() => NwModule.Instance.Players.Count());
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

    public XPReportService(EventService eventService, TwoDimArrayFactory twoDimArrayFactory)
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