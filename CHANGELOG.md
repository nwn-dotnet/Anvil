# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## Unreleased
https://github.com/nwn-dotnet/Anvil/compare/v8193.33.2...HEAD

### Added
- Implemented `PaketPluginSource` for installing and running NuGet-based plugins.
- Implemented support for custom plugin sources with the `IPluginSource` interface.
- Implemented `PluginStorageService` - a unified API for storing plugin data and configurations.
- Added `ANVIL_HOME` environment variable. This variable defines the root path where Anvil config files, plugins and plugin data are read from.
- Added additional properties to `NwEncounter`.
- Added IsStackable to 'NwItem'

### Package Updates
- NWN.Core -> 8193.33.4
- NWN.Native -> 8193.33.4

### Changed
- N/A

### Deprecated
- N/A

### Removed
- <u>**BREAKING CHANGE**</u> - Removed `ANVIL_NLOG_CONFIG` environment variable. The config path is now fixed to `{ANVIL_HOME}/nlog.config`
- <u>**BREAKING CHANGE**</u> - Removed `ANVIL_PLUGIN_PATH` environment variable. The plugin load path is now fixed to `{ANVIL_HOME}/Plugins`

### Fixed
- Fixed a server crash when preventing player connections in the `OnClientConnect` event.
- Fixed `OnClientConnect.CDKey` returning a type name instead of the client's public CD key.

## 8193.33.2
https://github.com/nwn-dotnet/Anvil/compare/v8193.33.1...v8193.33.2

### Added
- ResourceManager: Implemented GffResource API.
- [Inject]: Property-based dependencies can now be flagged as "Optional". Optional dependencies do not throw exceptions if the service could not be loaded from a missing plugin assembly.

### Changed
- `[ServiceBindingOptions]` - The `BindingPriority` property defines the initialization order of a service. (Higher priority = initialized first).
- When injecting a dependency of an interface/base class and multiple candidates are available, the service with the highest `BindingPriority` will be injected.
- `NwCreature.Age` can now be set.

### Deprecated
- `[ServiceBindingOptions]` - `Order` has been deprecated and replaced with the `BindingPriority` property.
- `[ServiceBindingOptions]` - `MissingPluginDependencies` has been deprecated as functionality is covered by the `BindingPriority` dependency resolve behaviour.
- `AttributeExtensions` - moved to `ReflectionExtensions`.
- `NwPlayer.NuiSetGroupLayout` - moved to `NuiGroup.SetLayout`.

### Fixed
- Properties injected into service classes with plugin dependency requirements will no-longer throw an exception when the assembly is missing.
- Fixed NuiGroup.SetLayout creating nested layout elements instead of updating the existing element (`NwPlayer.NuiSetGroupLayout` still has the old behaviour.)

## 8193.33.1
https://github.com/nwn-dotnet/Anvil/compare/v8193.33.0...v8193.33.1

### Changed
- Update to NWN.Core 8193.33.1

### Fixed
- Fixed an issue when retrieving bind values for certain class/structures (NuiRect, NuiVector, etc.) would return the default value.
- Fixed an incorrect API/object mapping for NuiDrawList.

## 8193.33.0
https://github.com/nwn-dotnet/Anvil/compare/v8193.26.3...v8193.33.0

### Added
- NUI: Implement API with classes, added methods to NwPlayer.
- Effect: Added `Effect.Icon()` factory method for creating Icon effects.
- Effect: Added `Effect.RunAction()` factory methods for creating effects that invoke C# actions.
- ScriptHandleFactory: New service for dynamically creating function callbacks at runtime that are bound to script names. The returned handle is currently used for script parameters in effects.
- ModuleEvents: Added `OnPlayerGuiEvent`, `OnNuiEvent` and `OnPlayerTarget` events.
- GUIPanel: Added new constants published with NWN 8193.31
- NwPlayer: Added `SetGuiPanelDisabled` for disabling built-in GUI elements.
- NwPlayer: Added `RestDurationOverride` property.
- VirtualMachine: Added `RecursionLevel` property.
- LocalVariableCassowary: Added to support cassowary local variables.
- ILateDisposable: Added a new service event interface that is invoked after the server is destroyed.
- ServiceBindingOptions: Added `PluginDependencies` and `MissingPluginDependencies` properties for setting up services with optional plugin dependencies.
- Added `PRELINK_ENABLED` setting for disabling native prelink checks.

### Changed
- Refactored various internal usages of NWN.Native to use collection/list accessors for native types.
- VirtualMachine: `IsInScriptContext` now checks the current executing thread, and now only returns true while on the main thread and inside of a VM script context.
- HookService: Hooks are now returned/disposed after the server has been destroyed.
- IScriptDispatcher: Custom Script Dispatchers must now define an execution order. This order is used when a script call is triggered from the VM, and determines which service/s implementing this interface get executed first.
- ObjectStorageService (**Breaking**): Persistent data written in Anvil can no-longer be accessed by NWNX. Anvil can still import NWNX persistent data.
- ObjectStorageSerive: Anvil no-longer writes a duplicate `NWNX_POS` serialized field and instead writes its own `ANVIL_POS` field for persistent object data.

### Deprecated
- Effect: Deprecated `Effect.AreaOfEffect` that uses strings for the script handlers. Use the overload that uses `ScriptCallbackHandle` parameters instead.

### Removed
- HookService: Removed the optional `shutdownDispose` parameter (superseded by `ILateDisposable`).
- AnvilCore: Removed custom `ITypeLoader` support, and hardcoded references to the updated `PluginManager`.

### Fixed
- Fixed an issue where the `ObjectStorageService` would cause errors when performing hot reloads with `AnvilCore.Reload()`
- Fixed an issue where the PluginLoader would attempt to unload plugins too early during server shutdown/hot reload.
- Fixed an issue where the `EnforceLegalCharacterService` would call the `ELCValidationBefore` event outside of a script context.
- Fixed `OnChatMessageSend.Target` always being null.
- Fixed Plugin Unloadability & Hot Reload.
- Fixed a native crash caused by a conflict with Anvil's `ObjectStorageService` and NWNX's Object Storage (POS).

## 8193.26.3
https://github.com/nwn-dotnet/Anvil/compare/v8193.26.2...v8193.26.3

### Fixed
- OnCreatureDamage: Fixed damage hook function returning void instead of an int. Resolves an issue where damage effects would persist on a character, and be applied again after login.

## 8193.26.2
https://github.com/nwn-dotnet/Anvil/compare/v8193.26.1...v8193.26.2

### Changed
- IEvent: Exposed "Context" property, allowing plugins to implement custom events.

## 8193.26.1
https://github.com/nwn-dotnet/Anvil/compare/v8193.26.0...v8193.26.1

### Fixed
- OnCreatureDamage: Fixed a server crash when a creature received damage from non-object sources. (EffectDamage from a module/area event, etc.)

## 8193.26.0
https://github.com/nwn-dotnet/Anvil/compare/ffd9cd6dd0d6626ebc325265a8a8b370dd74d66b...v8193.26.0

### Initial Release!
