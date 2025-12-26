## Prerequisites
This guide assumes you have completed the setup of your environment and server, have some basic understanding of C# terms (classes, constructors, fields and attributes) and have created and compiled an empty plugin.

Have a look at the earlier guides if you are not already at this step, or need an extra refresher.

## What is a Plugin Service/Service Class?

Plugin services are C# classes that compose the logic of your Plugin.

Typically, you would have at least 1 "service class" for each system/behaviour/feature that you develop, and perhaps even more for complex systems.

Here is an example of a very basic service, that simply logs a message when the server starts up:

```csharp
using NLog;
using Anvil.Services;

[ServiceBinding(typeof(ServiceA))]
public class ServiceA
{
  private static readonly Logger Log = LogManager.GetCurrentClassLogger();

  public ServiceA()
  {
    Log.Info("Service A Loaded!");
  }
}
```

How does this work? The important part here is the "ServiceBinding" attribute:

```csharp
[ServiceBinding(typeof(ServiceA))] <-- This
public class ServiceA
```
When a class is flagged with this attribute, it instructs Anvil that this class should be _**constructed**_ during startup.

This means that our constructor code gets called just after the server loads:
```csharp
  public ServiceA()
  {
    Log.Info("Service A Loaded!");
  }
```
```
nwnxee-server_1  | I [2021/09/01 19:53:15.620] [ServiceA] Service A Loaded!
```

***

Now let's make a second service called "ServiceB":
```csharp
[ServiceBinding(typeof(ServiceB))]
public class ServiceB
{
  private static readonly Logger Log = LogManager.GetCurrentClassLogger();

  public ServiceB()
  {
    Log.Info("Service B Loaded!");
  }
}
```

Now if I start the server, I will have two log messages! Wait, why are they logging in the wrong order?

```
nwnxee-server_1  | I [2021/09/01 20:01:57.689] [ServiceB] Service B Loaded!
nwnxee-server_1  | I [2021/09/01 20:01:57.689] [ServiceA] Service A Loaded!
```
_(They might show up in the right order for you! But it is not deterministic!)_

Since `ServiceB` has no knowledge about `ServiceA`, Anvil doesn't know that it should construct a certain service before another.

To solve this, we define a Service Dependency in `ServiceB`'s constructor:

```csharp
  public ServiceB()
  {
    Log.Info("Service B Loaded!");
  }
```

Becomes:
```csharp
  public ServiceB(ServiceA serviceA) <-- This tells Anvil that we need ServiceA before we run this!
  {
    Log.Info("Service B Loaded!");
  }
```

Full Code:
```csharp
using NLog;
using Anvil.Services;

[ServiceBinding(typeof(ServiceA))]
public class ServiceA
{
  private static readonly Logger Log = LogManager.GetCurrentClassLogger();

  public ServiceA()
  {
    Log.Info("Service A Loaded!");
  }
}

[ServiceBinding(typeof(ServiceB))]
public class ServiceB
{
  private static readonly Logger Log = LogManager.GetCurrentClassLogger();

  public ServiceB(ServiceA serviceA)
  {
    Log.Info("Service B Loaded!");
  }
}
```

Now if you run the program, the log messages output in the expected order:
```
nwnxee-server_1  | I [2021/09/01 20:01:57.689] [ServiceA] Service A Loaded!
nwnxee-server_1  | I [2021/09/01 20:01:57.689] [ServiceB] Service B Loaded!
```

This is a pattern called _**Dependency Injection**_, and is a fundamental core concept to learn to be able to interact with Anvil's own API and services.

Another way to visualise this is via property injection. This is functionally identical to the constructor method above:

```csharp
[ServiceBinding(typeof(ServiceB))]
public class ServiceB : IInitializable // Notifies anvil that we implement an Init() method, and it should be called during startup.
{
  private static readonly Logger Log = LogManager.GetCurrentClassLogger();

  [Inject] // Create, and Inject Service A before calling...
  private ServiceA ServiceA { get; set; }

  /// ...The Init() function.
  public void Init()
  {
    Log.Info("Service B Loaded!");
  }
}
```

**As an exercise, try making your own ServiceC class that instead logs a message before ServiceA and ServiceB!**

***

## Accessing other Services

There will inevitably be a point where you will want to call a function, or access data from another service.

Let's say you have the following two services. We want to log the message, but only if the player is cool.
But we do our "coolness" check in a separate service called `CoolUserService`:

```csharp
[ServiceBinding(typeof(CoolUserService))]
public class CoolUserService
{
  // Returns true if the specified player is cool or not.
  public bool IsCoolUser(NwPlayer player)
  {
    if (player.IsDM)
    {
      return true;
    }
    else
    {
      return false;
    }
  }
}
```

```csharp
[ServiceBinding(typeof(PlayerService))]
public class PlayerService
{
  private static readonly Logger Log = LogManager.GetCurrentClassLogger();

  public PlayerService()
  {
    NwModule.Instance.OnClientEnter += OnClientEnter;
  }

  // When a player connects, check if they are cool or not.
  private void OnClientEnter(ModuleEvents.OnClientEnter eventData)
  {
    Log.Info($"{eventData.Player.PlayerName} is cool!");
  }
}
```

How do we access this member? The immediate temptation would be to make the `IsCoolUser` function static, so it can then be accessed like so:

```csharp
    // (Don't do this!)
    public static bool IsCoolUser(NwPlayer player)
```

```csharp
    // (Don't do this!)
    private void OnClientEnter(ModuleEvents.OnClientEnter eventData)
    {
      if (CoolUserService.IsCoolUser(eventData.Player))
      {
        Log.Info($"{eventData.Player.PlayerName} is cool!");
      }
    }
```

If we do this, it is impossible to know if the service is initialized, and your code can quickly rot into spaghetti with these types of calls dotted across your code base.

The correct way to do it is to first define the `CoolUserService` as a dependency in our constructor:

```csharp
    public PlayerService()
    {
      NwModule.Instance.OnClientEnter += OnClientEnter;
    }
```

Becomes:

```csharp
    public PlayerService(CoolUserService coolUserService)
    {
      NwModule.Instance.OnClientEnter += OnClientEnter;
    }
```

Now we are guaranteed that the `CoolUserService` has been constructed for us. But how do we use the function?

Let's then store the `CoolUserService` in a field:

```csharp
    private readonly CoolUserService userService;

    public PlayerService(CoolUserService coolUserService)
    {
      userService = coolUserService; // Assign "coolUserService" specified in the constructor, to our class field "userService"
      NwModule.Instance.OnClientEnter += OnClientEnter;
    }
```

Then in the client event handler, we can use the service stored in the class field to call the function:

```csharp
    private void OnClientEnter(ModuleEvents.OnClientEnter eventData)
    {
      if (userService.IsCoolUser(eventData.Player))
      {
        Log.Info($"{eventData.Player.PlayerName} is cool!");
      }
    }
```

The complete code:
```csharp
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using NLog;

[ServiceBinding(typeof(CoolUserService))]
public class CoolUserService
{
  public bool IsCoolUser(NwPlayer player)
  {
    if (player.IsDM)
    {
      return true;
    }
    else
    {
      return false;
    }
  }
}

[ServiceBinding(typeof(PlayerService))]
public class PlayerService
{
  private static readonly Logger Log = LogManager.GetCurrentClassLogger();

  private readonly CoolUserService userService;

  public PlayerService(CoolUserService coolUserService)
  {
    userService = coolUserService;
    NwModule.Instance.OnClientEnter += OnClientEnter;
  }

  private void OnClientEnter(ModuleEvents.OnClientEnter eventData)
  {
    if (userService.IsCoolUser(eventData.Player))
    {
      Log.Info($"{eventData.Player.PlayerName} is cool!");
    }
  }
}
```

And that is it! We should now get a message when a DM connects to the server:

```
nwnxee-server_1  | I [2021/09/01 21:11:36.199] [PlayerService] SomeDM is cool!
```

Now that you know how to reference other services, try to push yourself to make smaller services with single responsibilities! You might find ways to reuse code that you never considered before, and save yourself a lot of headache from debugging monolithic service classes.

***

## Anvil Services

Anvil is bundled with a growing set of core services that you can use in your own services.

You can find a list of all available bundled services [HERE](~/api/Anvil.Services.yml).

Similar to the dependency setup in the last section, simply define the service as a dependency in your service constructor.

As a example, here is how to log the number of players every 10 minutes using Anvil's `ScheduleService`:

```csharp
using System;
using Anvil.API;
using Anvil.Services;
using NLog;

[ServiceBinding(typeof(PlayerCountTrackingService))]
public class PlayerCountTrackingService
{
  private static readonly Logger Log = LogManager.GetCurrentClassLogger();

  public PlayerCountTrackingService(SchedulerService schedulerService) // SchedulerService is a service bundled with Anvil.
  {
    schedulerService.ScheduleRepeating(LogPlayerCount, TimeSpan.FromMinutes(10));
  }

  private void LogPlayerCount()
  {
    Log.Info($"Online Players: {NwModule.Instance.PlayerCount}");
  }
}
```
