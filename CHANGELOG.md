# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## Unreleased
https://github.com/nwn-dotnet/Anvil/compare/v8193.26.3...HEAD

### Added
- Effect: Added `Effect.Icon()` factory method for creating Icon effects.
- Effect: Added `Effect.RunAction()` factory methods for creating effects that invoke C# actions.
- ScriptHandleFactory: New service for dynamically creating function callbacks at runtime that are bound to script names. The returned handle is currently used for script parameters in effects.
- ModuleEvents: Added `OnPlayerGuiEvent` and `OnPlayerTarget` events.
- GUIPanel: Added new constants published with NWN 8193.31
- NwPlayer: Added `SetGuiPanelDisabled` for disabling built-in GUI elements.
- VirtualMachine: Added `RecursionLevel` property.
- LocalVariableCassowary: Added to support cassowary local variables.
- ILateDisposable: Added a new service event interface that is invoked after the server is destroyed.
- ServiceBindingOptions: Added `PluginDependencies` and `MissingPluginDependencies` properties for setting up services with optional plugin dependencies.

### Changed
- Refactored various internal usages of NWN.Native to use collection/list accessors for native types.
- VirtualMachine: `IsInScriptContext` now checks the current executing thread, and now only returns true while on the main thread and inside of a VM script context.
- HookService: Hooks are now returned/disposed after the server has been destroyed.
- IScriptDispatcher: Custom Script Dispatchers must now define an execution order. This order is used when a script call is triggered from the VM, and determines which service/s implementing this interface get executed first.

### Deprecated
- Effect: Deprecated `Effect.AreaOfEffect` that uses strings for the script handlers. Use the overload that uses `ScriptCallbackHandle` parameters instead.

### Removed
- HookService: Removed the optional `shutdownDispose` parameter.
- AnvilCore: Removed custom `ITypeLoader` support, and hardcoded references to the updated PluginManager.

### Fixed
- Fixed an issue where the `ObjectStorageService` would cause errors when performing hot reloads with `AnvilCore.Reload()`
- Fixed an issue where the PluginLoader would attempt to unload plugins too early during server shutdown/hot reload.
- Fixed an issue where the `EnforceLegalCharacterService` would call the `ELCValidationBefore` event outside of a script context.
- Fixed `OnChatMessageSend.Target` always being null.

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
