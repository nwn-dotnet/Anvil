# Function Hooking
Function hooking is the act of intercepting/"hooking" native engine function calls that are made by the base server to inject additional behaviours, add new events that can be subscribed to and customized by plugins, or replace existing functionality entirely.

Anvil offers two ways of hooking functions that are used depending on the context.

## Delegate Marshalling
Delegate marshalling is the general type we use for services that provided extended functions, and is the easiest to setup.

To start, you will want to declare a delegate that matches the associated C++ function EXACTLY. See the [Type Map](#type-map) on how these values should be mapped.

If the function signature is incorrect, the server will likely crash immediately when the hook is called due to corrupting the stack. In worse cases, the server can become a zombie and crash in an unrelated context.

```csharp
  [ServiceBinding(typeof(PlayerRestDurationOverrideService))]
  // API binding priority ensures that this service is available before plugin services are initialized.
  [ServiceBindingOptions(InternalBindingPriority.API)]
  internal sealed unsafe class PlayerRestDurationOverrideService
  {
    // This represents the base game function we intend to hook: uint32_t CNWSCreature::AIActionRest(CNWSObjectActionNode * pNode);
    // In C#, our first parameter is always a pointer to the class (CNWSCreature) that the method is contained in, followed by the method's other parameters.
    // In this case, uint32_t becomes uint, CNWSCreature* becomes void* and CNWSObjectActionNode* becomes void*
    // See the type map table below for converting between C++ and C# types.
    // The NativeFunction attribute contains the mangled symboled names exported by the base game. 1st one being linux/mac, the 2nd being windows.
    // You can find these mangled names by running "nm -g" and "dumpbin /exports" on the base game binaries.
    // You can verify you have the correct address by demangling the name here: https://demangler.com/
    [NativeFunction("_ZN12CNWSCreature12AIActionRestEP20CNWSObjectActionNode", "?AIActionRest@CNWSCreature@@QEAAIPEAVCNWSObjectActionNode@@@Z")]
    private delegate uint AIActionRestHook(void* pCreature, void* pNode);
  }
```

Now that the delegate is defined, we can declare Anvil's `HookService` as a dependency in our service constructor, and request a function hook:

```csharp
  [ServiceBinding(typeof(PlayerRestDurationOverrideService))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  internal sealed unsafe class PlayerRestDurationOverrideService
  {
    [NativeFunction("_ZN12CNWSCreature12AIActionRestEP20CNWSObjectActionNode", "?AIActionRest@CNWSCreature@@QEAAIPEAVCNWSObjectActionNode@@@Z")]
    private delegate uint AIActionRestHook(void* pCreature, void* pNode);

    // FunctionHook holds a reference to the original engine function - we store it as a field so we can call it from our hook.
    private readonly FunctionHook<AIActionRestHook> aiActionRestHook;

    public PlayerRestDurationOverrideService(HookService hookService)
    {
      // For RequestHook, the first parameter is the name of the C# method that should be called when this hook is called by the base game.
      // The second parameter is the order in which the function hook should be established. The rule for ordering should be roughly:
      // Earliest for pure event notification hooks
      // Early for skippable events, or events that modify state in before/after
      // Late for things that provide alternative implementation in some cases, like the POS getters in creature
      // Latest for things that almost never call the original
      // Final for things that fully reimplement base game functions
      aiActionRestHook = hookService.RequestHook<AIActionRestHook>(OnAIActionRest, HookOrder.Late);
    }

    // With the function hooked, the original method will no-longer be called.
    // This function will now be called instead.
    private uint OnAIActionRest(void* pCreature, void* pNode)
    {
      // To get the original behaviour, we can call the original function:
      return aiActionRestHook.CallOriginal(pCreature, pNode);
    }
  }
```

Finally, we can implement our custom behaviours in the hooked function.

In this example, we add rest duration overrides by exposing methods that add durations to a `Dictionary`.

In the hooked method, we use the durations added to this dictionary and temporarily set the duration table values before calling the base game function.

Once the base game function has been called, we restore the table value to the original defined value.

```csharp
  [ServiceBinding(typeof(PlayerRestDurationOverrideService))]
  [ServiceBindingOptions(InternalBindingPriority.API)]
  internal sealed unsafe class PlayerRestDurationOverrideService
  {
    private static readonly CExoString DurationTableKey = "Duration".ToExoString();

    private readonly FunctionHook<AIActionRestHook> aiActionRestHook;
    private readonly Dictionary<NwCreature, int> restDurationOverrides = new Dictionary<NwCreature, int>();

    public PlayerRestDurationOverrideService(HookService hookService)
    {
      aiActionRestHook = hookService.RequestHook<AIActionRestHook>(OnAIActionRest, HookOrder.Late);
    }

    [NativeFunction("_ZN12CNWSCreature12AIActionRestEP20CNWSObjectActionNode", "?AIActionRest@CNWSCreature@@QEAAIPEAVCNWSObjectActionNode@@@Z")]
    private delegate uint AIActionRestHook(void* pCreature, void* pNode);

    public void ClearDurationOverride(NwCreature creature)
    {
      restDurationOverrides.Remove(creature);
    }

    public TimeSpan? GetDurationOverride(NwCreature creature)
    {
      return restDurationOverrides.TryGetValue(creature, out int retVal) ? TimeSpan.FromMilliseconds(retVal) : null;
    }

    public void SetDurationOverride(NwCreature creature, TimeSpan duration)
    {
      restDurationOverrides[creature] = (int)Math.Round(duration.TotalMilliseconds);
    }

    private uint OnAIActionRest(void* pCreature, void* pNode)
    {
      CNWSCreature creature = CNWSCreature.FromPointer(pCreature);
      NwCreature nwCreature = creature.ToNwObject<NwCreature>();

      if (nwCreature != null && restDurationOverrides.TryGetValue(nwCreature, out int durationOverride))
      {
        byte creatureLevel = creature.m_pStats.GetLevel(0);
        int originalValue;

        C2DA durationTable = NWNXLib.Rules().m_p2DArrays.m_pRestDurationTable;

        durationTable.GetINTEntry(creatureLevel, DurationTableKey, &originalValue);
        durationTable.SetINTEntry(creatureLevel, DurationTableKey, durationOverride);
        uint retVal = aiActionRestHook.CallOriginal(pCreature, pNode);
        durationTable.SetINTEntry(creatureLevel, DurationTableKey, originalValue);

        return retVal;
      }

      return aiActionRestHook.CallOriginal(pCreature, pNode);
    }
  }
```

## Function Pointers
Function pointers are more performant than delegate marshalling, but are a bit more tricky to setup. Anvil uses function pointers for events as they are called frequent enough to see a performance benefit in their use.

Unlike delegate marshalling, they require a static function to map to, which can be awkward to setup with the service pattern Anvil is designed around.

To help with writing events with this type of hook, a base class has been setup to manage their lifecycle.

To start, define the event class which will hold our event data:
```csharp
// Note the lack of [ServiceBinding] - events are created at runtime and should not be defined as as service.
public sealed class OnServerCharacterSave : IEvent
{
}
```

Next, add a child `Factory` class. This is where we will hook our function and generate events
```csharp
public sealed class OnServerCharacterSave : IEvent
{
    // No service binding needed here, as the `HookEventFactory` base class already defines the attribute
    internal sealed unsafe class Factory : HookEventFactory
    {
    }
}
```

Now, we declare a delegate the same way as we would in delegate marshalling. The delegate must match the associated C++ function EXACTLY.

See the [Type Map](#type-map) on how these values should be mapped.

```csharp
public sealed class OnServerCharacterSave : IEvent
{
    internal sealed unsafe class Factory : HookEventFactory
    {
      // This represents the base game function we intend to hook: BOOL CNWSPlayer::SaveServerCharacter(BOOL bBackupPlayer = false)
      // In C#, our first parameter is always a pointer to the class (CNWSPlayer) that the method is contained in, followed by the method's other parameters.
      // In this case, BOOL becomes int, CNWSPlayer* becomes void* and BOOL becomes int
      // See the type map table below for converting between C++ and C# types.
      internal delegate int SaveServerCharacterHook(void* pPlayer, int bBackupPlayer);
    }
}
```

To hook the function, we implement the `RequestHooks` function which is declared abstract in the `HookEventFactory` class, and assign the function hook to a static field:

```csharp
public sealed class OnServerCharacterSave : IEvent
{
    internal sealed unsafe class Factory : HookEventFactory
    {
      internal delegate int SaveServerCharacterHook(void* pPlayer, int bBackupPlayer);

      // FunctionHook holds a reference to the original engine function - we store it as a static field so we can call it in our hook.
      private static FunctionHook<SaveServerCharacterHook> SaveServerCharacterFunctionHook { get; set; }

      protected override IDisposable[] RequestHooks()
      {
        // Here, we create a function pointer that we can send to the HookService for hooking the native function.
        // The type arguments <void*, int, int> for the delegate* are the function parameters (void*, int), followed by the return value (int).
        delegate* unmanaged<void*, int, int> pHook = &OnSaveServerCharacter;

        // For RequestHook, the first parameter is the name of the C# method that should be called when this hook is called by the base game.
        // The second parameter is the order in which the function hook should be established. The rule for ordering should be roughly:
        // Earliest for pure event notification hooks
        // Early for skippable events, or events that modify state in before/after
        // Late for things that provide alternative implementation in some cases, like the POS getters in creature
        // Latest for things that almost never call the original
        // Final for things that fully reimplement base game functions
        SaveServerCharacterFunctionHook = HookService.RequestHook<SaveServerCharacterHook>(pHook, HookOrder.Early);

        // Finally, we return our created hook so it is correctly disposed when the server shuts down.
        return new IDisposable[] { SaveServerCharacterFunctionHook };
      }

      [UnmanagedCallersOnly]
      private static int OnSaveServerCharacter(void* pPlayer, int bBackupPlayer)
      {
        // To get the original behaviour, we can call the original function:
        return SaveServerCharacterFunctionHook.CallOriginal(pPlayer, bBackupPlayer);
      }
    }
}
```

Next, we add public properties (with documentation!) that represent the data that we want accessible when the event fires.

```csharp
  public sealed class OnServerCharacterSave : IEvent
  {
    /// <summary>
    /// Gets the player that is being saved.
    /// </summary>
    public NwPlayer Player { get; private init; }

    /// <summary>
    /// Gets or sets a value indicating whether the character should be prevented from being saved.
    /// </summary>
    public bool PreventSave { get; set; }

    // Context is a special member that defines filtering for events (e.g. if you subscribe to save events for a specific player)
    // It is a required implementation for all events. Use null if there's no associated context for the event.
    NwObject IEvent.Context => Player.ControlledCreature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private delegate int SaveServerCharacterHook(void* pPlayer, int bBackupPlayer);

      private static FunctionHook<SaveServerCharacterHook> SaveServerCharacterFunctionHook { get; set; }

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, int> pHook = &OnSaveServerCharacter;
        SaveServerCharacterFunctionHook = HookService.RequestHook<SaveServerCharacterHook>(pHook, HookOrder.Early);

        return new IDisposable[] { SaveServerCharacterFunctionHook };
      }

      [UnmanagedCallersOnly]
      private static int OnSaveServerCharacter(void* pPlayer, int bBackupPlayer)
      {
        return SaveServerCharacterFunctionHook.CallOriginal(pPlayer, bBackupPlayer);
      }
    }
  }
```

Finally, we generate the event call and handle it in our hooked function:

```csharp
  public sealed class OnServerCharacterSave : IEvent
  {
    /// <summary>
    /// Gets the player that is being saved.
    /// </summary>
    public NwPlayer Player { get; private init; }

    /// <summary>
    /// Gets or sets a value indicating whether the character should be prevented from being saved.
    /// </summary>
    public bool PreventSave { get; set; }

    NwObject IEvent.Context => Player.ControlledCreature;

    internal sealed unsafe class Factory : HookEventFactory
    {
      private delegate int SaveServerCharacterHook(void* pPlayer, int bBackupPlayer);

      private static FunctionHook<SaveServerCharacterHook> SaveServerCharacterFunctionHook { get; set; }

      protected override IDisposable[] RequestHooks()
      {
        delegate* unmanaged<void*, int, int> pHook = &OnSaveServerCharacter;
        SaveServerCharacterFunctionHook = HookService.RequestHook<SaveServerCharacterHook>(pHook, HookOrder.Early);

        return new IDisposable[] { SaveServerCharacterFunctionHook };
      }

      [UnmanagedCallersOnly]
      private static int OnSaveServerCharacter(void* pPlayer, int bBackupPlayer)
      {
        // Here, we create a new instance of our event class (OnServerCharacterSave), and call the base "ProcessEvent" function.
        // Process event will send the event via the EventService in a safe script context, doing all the heavy lifting for you.
        OnServerCharacterSave eventData = ProcessEvent(new OnServerCharacterSave
        {
          Player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer(),
        });

        // At this point, the event has run and all subscribers have been called.
        // We check the "PreventSave" property to see if we should call the original function or not.
        return !eventData.PreventSave ? SaveServerCharacterFunctionHook.CallOriginal(pPlayer, bBackupPlayer) : 0;
      }
    }
  }
```

## Type Map
Use these types when translating C++ types to C# types in Anvil:

| C++ Type | C# Type | Notes |
| ------------- | ------------- | ------------- |
bool | int ||
uint8_t | byte ||
uint8_t* | byte* ||
char | byte ||
char* | byte* | Char arrays needed to be converted to avoid code page issues. See the StringHelper class. |
int32_t | int ||
int32_t* | int* ||
uint32_t | uint ||
uint32_t* | uint* ||
int64_t | long ||
int64_t* | long* ||
uint64_t | ulong ||
uint64_t* | ulong* ||
float | float ||
float* | float* ||
void | void ||
void* | void* ||
BOOL | int ||
RESTYPE | ushort ||
ObjectID | uint ||
OBJECT_ID | uint ||
STRREF | uint ||
PlayerID | uint ||
class/struct pointers (*) | void* | Resolve the original type with `XXX.FromPointer(void*)`.
class/struct references (&) | void* | Resolve the original type with `XXX.FromPointer(void*)`.
class/struct values | Special | Function parameters that copy a value instead of a reference are more complex to hook, and required a matching struct in C# as the parameter.<br/>See the `CExoLocStringStruct` example [here](https://github.com/nwn-dotnet/Anvil/blob/development/NWN.Anvil/src/main/Services/Feedback/FeedbackService.cs#L42) in the Feedback Service.
