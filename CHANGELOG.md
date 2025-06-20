# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## Unreleased
https://github.com/nwn-dotnet/Anvil/compare/v8193.37.1...HEAD

### Added
- N/A

### Package Updates
- N/A

### Changed
- Improved invalid object detection in some edge cases.

### Deprecated
- N/A

### Removed
- N/A

### Fixed
- N/A

## 8193.37.1
https://github.com/nwn-dotnet/Anvil/compare/v8193.37.0...v8193.37.1

### Added
- Events: Added `OnItemDestroy` event.
- Events: Added `OnItemDecrementStackSize` event.
- Events: Added `OnCreatureAcquireItem` event.
- Events: Added `OnTrapDisarm` event.
- Events: Added `OnTrapEnter` event.
- Events: Added `OnTrapExamine` event.
- Events: Added `OnTrapFlag` event.
- Events: Added `OnTrapRecover` event.
- Events: Added `OnTrapSet` event.
- NwArea: Added `GetLocalizedName` method.
- NwArea: Added `SetLocalizedName` method.
- NwCreature: Added `InitiativeRoll` property getter/setter.
- NwGameObject: Added `GetLocalizedName` method.
- NwGameObject: Added `SetLocalizedName` method.
- NwPlayer: Added `PlayerPossessCreature` method.
- Added support for different [ScriptHandler] callback method signatures. The following signatures are now supported:
```
  void Handler();
  bool Handler();
  ScriptHandleResult Handler();
  void Handler(CallInfo callInfo);
  bool Handler(CallInfo callInfo);
  ScriptHandleResult Handler(CallInfo callInfo);
```

### Package Updates
- NWNX: 7fc892a -> 292a2c0
- NWN.Core: 8193.37.2 -> 8193.37.3

### Changed
- Effect: `Damage` now supports optional `damagePower` parameter.
- Events: `OnUnacquireItem.Item` is now nullable.

### Fixed
- Fixed some assertion failure messages from skipped events using `CNWSMessage`.
- Fixed `DMPossessCreature` exception when player was a DM.
- Fixed undefined behaviour when disposing function hooks/reloading anvil.

## 8193.37.0
https://github.com/nwn-dotnet/Anvil/compare/v8193.36.1...v8193.37.0

### Added
- Plugins: Added support to disable loading plugins on startup with new environment variable, `ANVIL_PLUGINNAME_SKIP`
- Plugins: Implemented new API for loading/unloading isolated plugins at runtime.
  - To mark a plugin as "isolated", add the `PluginInfo` assembly attribute with `Isolated` = `true`.
  - To load the isolated plugin, call the `PluginManager.LoadPlugin` method.
  - To unload the isolated plugin, call the `PluginManager.UnloadPlugin` method.
- PlayerDeviceProperty: Added new 8193.37 constants.
- PlayerPlatform: Added new 8193.37 constants.
- SavingThrowType: Added new 8193.37 constants.
- SpellFailureType: Added new 8193.37 constants.
- Effect: Added `DamageIncrease` overload with correct constants.
- Effect: Added `SummonCreature` overload with `NwCreature` parameter instead of string ResRef.
- ItemProperty: Added `OnHitCastSpell` overload with correct casterLevel parameter.
- CreatureEvents.OnDamaged: Added `GetDamageDealtByType` method.
- Events: Added `OnPolymorphApply` event.
- Events: Added `OnPolymorphRemove` event.
- OnSpellBroadcast: Added `TargetObject`, `TargetPosition` properties.
- CreatureClassInfo: Added `School` property setter.
- NwAreaOfEffect: Added `SetRadius` method.
- NwCreature: Added `AnimalCompanionName` property setter.
- NwCreature: Added `AnimalCompanionType` property setter.
- NwCreature: Added `FamiliarName` property setter.
- NwCreature: Added `FamiliarType` property setter.
- NwCreature: Added `ActionCloseDoor` method.
- NwCreature: Added `ForceLevelUp` method.
- NwCreature: Added `ActionOpenDoor` method.
- NwCreature: Added `SpellResistanceCheck` method.
- NwCreature: Added `SpellImmunityCheck` method.
- NwCreature: Added `SpellAbsorptionLimitedCheck` method.
- NwCreature: Added `SpellAbsorptionUnlimitedCheck` method.
- NwCreature: Added `SummonAnimalCompanion` method.
- NwCreature: Added `UnsummonAnimalCompanion` method.
- NwCreature: Added `SummonFamiliar` method.
- NwCreature: Added `UnsummonFamiliar` method.
- NwCreature: Added `Unsummon` method.
- NwGameObject: Added `IsDestroyable` property.
- NwGameObject: Added `IsRaiseable` property.
- NwGameObject: Added `IsSelectableWhenDead` property.
- NwItem: Added `GetMinEquipLevelOverride`, `SetMinEquipLevelOverride`, `ClearMinEquipLevelOverride` methods.
- NwPlayer: Added `Latency` property.
- NwPlayer: Added `LatencyAverage` property.
- NwPlayer: Added `CancelTargetMode` method.
- NwPlayer: Added `UpdateCharacterSheet` method.
- NwPlaceable/NwTrigger: Added `LockedBy` property.
- NwPlaceable/NwTrigger: Added `UnlockedBy` property.
- NwRace: Added `GetFavoredEnemyFeat` method.
- EnforceLegalCharacterService: Added failure event for too many ability score increases.

### Package Updates
- NWNX: 9865013 -> 7fc892a
- NWN.Core: 8193.36.1 -> 8193.37.2
- NWN.Native: 8193.36.2 -> 8193.37.3
- NLog: 5.2.8 -> 5.4.0
- System.Reflection.MetadataLoadContext: 8.0.1

### Changed
- Changed NWNX_DotNET interop to use `NWNX.NET` plugin.
- Use `NWNX.NET` for encoding strings between UTF-16 and CP-1252.
- Effect: `Polymorph` now supports optional `unPolymorphVfx`, `spellAbilityModifier`, `spellAbilityCasterLevel` parameters.
- Effect: `SpellFailure` now supports an optional `failureType` parameter.
- Effect/ItemProperty: `Spell` property is now correctly marked as nullable.
- NwCreature: `TouchAttackMelee` method is no-longer async.
- NwCreature: `TouchAttackRanged` method is no-longer async.
- NwGameObject: `EndConversation` method is now awaitable.
- NwGameObject: `FaceToObject` method is no-longer async.
- NwGameObject: `FaceToPoint` method is no-longer async.
- NwGameObject: `SetFacing` method is no-longer async.
- NwModule: `PlayerCount` property is now an int.
- NwObject: `ClearActionQueue` method is no-longer async.
- ClassFeatListTypes: Renamed to match flags enum naming convention.

### Deprecated
- `ItemProperty.OnHitCastSpell(IPCastSpell, IPSpellLevel)` - use the `ItemProperty.OnHitCastSpell(IPCastSpell, int)` overload instead.
- `NwCreature.CheckResistSpell(NwGameObject)` - use the `NwCreature.SpellResistanceCheck` method instead.
- `NwGameObject.SetIsDestroyable(bool,bool,bool)` - use the IsDestroyable/IsRaiseable/IsSelectableWhenDead properties instead.

### Fixed
- ItemProperty: `OnMonsterHitProperties` now correctly uses the "special" parameter.
- OnItemUnequip: Fixed a function hook issue.
- SchedulerService: Fixed an issue where scheduling events immediately after reloading anvil would delay the event for the total uptime of the server.
- NwCreature: `BaseAttackBonus` now returns the correct value if the value was not overriden with a custom value.
- Paket: Fixed an issue where logs would be printed multiple times after reloading anvil.
- NwCreature: Fixed an issue where setting `AlwaysWalk` to `false` would not correctly update the encumbrance state of the player.
- HookService: Re-implemented as core service, and added new API to persist some function hooks after reloading anvil.
- ObjectStorageService: Re-implemented as core service. Persistent variables should now persist after reloading anvil.
- EnforceLegalCharacterService: Fixed an IndexOutOfRangeException for characters with too many ability score increases.

## 8193.36.1
https://github.com/nwn-dotnet/Anvil/compare/v8193.35.3...v8193.36.1

### Added
- EffectType: Added new 8193.36 constants
- GuiEventType: Added new 8193.36 constants
- PlayerDeviceProperty: Added new 8193.36 constants
- ResRefType: Added new 8193.36 constants and missing constants for 8193.35
- Effect: Added `EnemyAttackBonus` method.
- SQLQuery: Added `Columns` property.
- SQLResult: Added overloads with column name parameters.
- OnPlayerGuiEvent: Added missing documentation for `EventObject` event types.
- NwArea: Added `SetAreaTileBorderDisabled` method.
- NwArea: Added `SetAreaGrassOverride` method.
- NwArea: Added `RemoveAreaGrassOverride` method.
- NwArea: Added `SetAreaDefaultGrassDisabled` method.
- NwGameObject: Added `CasterLevel` property. This replaces `LastSpellCasterLevel` from NwCreature.
- NwCreature: Added `SpellAbilities` property for managing creature spell-like abilities.
- NwPlayer: Added `StartAudioStream` method.
- NwPlayer: Added `StopAudioStream` method.
- NwPlayer: Added `SetAudioStreamPaused` method.
- NwPlayer: Added `SetAudioStreamVolume` method.
- NwPlayer: Added `SeekAudioStream` method.
- CRulesKeyHash: New structure to support working with the new hash-based ruleset key labels.
- RulesetKeys: New constants class with hashed keys for all known ruleset definitions.
- NwGameTables: Added `SurfaceMaterialTable`.

### Package Updates
- NWNX: 51162c5 -> 9865013
- NWN.Core: 8193.35.21 -> 8193.36.1
- NWN.Native: 8193.35.9 -> 8193.36.2
- NLog: 5.2.5 -> 5.2.8

### Changed
- Effect: `DamageReduction` now supports an optional `rangedOnly` parameter.
- Effect: `DamageResistance` now supports an optional `rangedOnly` parameter.
- Effect: `EffectType` will return the new types introduced in 8193.36.
- NwDoor: `Clone` now uses the shared `CloneInternal` behaviour.
- NwEncounter: `Clone` is now supported.
- NwGameObject: `ActionCastSpellAt` now supports optional `spellClass` and `spontaneousCast` parameters.
- NwPlayer: `FloatingTextString` now supports an optional `chatWindow` parameter.
- NwPlayer: `FloatingTextStrRef` now supports an optional `chatWindow` parameter.
- CampaignVariableObject: Tightened generic constraint to only allow `NwGameObject` instead of `NwObject` to match base game behaviour.

### Deprecated
- ItemProperty: `LimitUseByClass(IPClass)` - Use the `LimitUseByClass(NwClass)` overload instead.

### Fixed
- Fixed `Possessor` not correctly returning bags/containers.

## 8193.35.3
https://github.com/nwn-dotnet/Anvil/compare/v8193.35.2...v8193.35.3

### Removed
- Removed custom crash handler due to preventing dotnet from collecting a crash dump.

## 8193.35.2
https://github.com/nwn-dotnet/Anvil/compare/v8193.35.1...v8193.35.2

### Added
- Effect: Added `AreaOfEffect` overload with `PersistentVfxTableEntry` support.
- Effect: Added `Polymorph` overload with `PolymorphTableEntry` support.
- Location: Added `TileInfo` property.
- Events: Added `OnDispelMagicApply` event.
- NuiDrawList: Added `NuiDrawListItemType.Line` support.
- NuiDrawList: Added support for item order and render options.
- NuiDrawList: Added `ImageRegion` property.
- NuiImage: Added `ImageRegion` property.
- NuiImage: Added `WordWrap` property.
- Nui: Added `NuiToggles` widget.
- NuiElement: Added `DisabledTooltip`, `Encouraged` properties.
- NuiWindow: Added `AcceptsInput` property.
- CreatureLevelInfo: Added `AbilityGained` property.
- NwArea: Added `TileInfo` property.
- NwCreature: Added `RemainingSkillPoints` property.
- NwCreature: Added `BroadcastSkillRoll` method.
- NwCreature: Added `CalculateAbilityModifierFromScore` method.
- NwCreature: Added `GetArmorClassVersus` method.
- NwCreature: Added parameter to `RemoveFeat` to optionally remove the feat from the level stat list (for ELC).
- NwItem: Extended `AddItemProperty` to support additional behaviours for managing existing item properties.
- NwItem: Added `CompareItem` method.
- NwItem: Added `HasItemProperty` method.
- NwItem: Added `RemoveItemProperties` method.
- NwModule: Added `RefreshClientObjects` method.
- NwPlayer: Added `RefreshClientObject` method.
- NwPlayer: Added `RefreshPlayerClientObject` method.
- NwServer: Added `DebugMode`, `DebugCombat`, `DebugSaveThrows`, `DebugHitDie`, `DebugMoveSpeed` properties.
- NwGameTables: Added `PlaceableTypeTable`
- NwGameTables: Added `PolymorphTable`
- NwGameTables: Added `PortraitTable`
- NwGameTables: Added `PersistentEffectTable`
- PluginManager: Added support for specifying additional plugin directories with a new environment variable, `ADD_PLUGIN_PATHS`
- UnobservedTaskExceptionLogger: Added new service for logging and handling uncaught `async Task` exceptions.

### Package Updates
- NWNX: b419e42 -> 51162c5
- NWN.Core: 8193.35.6 -> 8193.35.21
- NWN.Native: 8193.34.6 -> 8193.35.9
- NLog: 5.1.4 -> 5.2.5

### Changed
- Effect: `Tag` property is now correctly marked as nullable.
- ItemProperty: `Tag` property is now correctly marked as nullable.
- OnClientConnect: `PlayerName`, `CDKey` properties are no longer nullable.
- CreatureLevelInfo: `Feats` property is now a mutable list.
- EncounterListEntry: `CreatureResRef` is now correctly marked as nullable.
- NwCreature: `DialogResRef` is now correctly marked as nullable.
- NwCreature: `TalentBest`, `TalentRandom` now returns the correct talent class type.
- NwDoor: `DialogResRef` is now correctly marked as nullable.
- NwGameObject: `PortraitId` now uses the 2da PortraitTableEntry type.
- NwPlaceable: `DialogResRef` is now correctly marked as nullable.
- NwBaseItem: `DefaultIcon` is now correctly marked as nullable.
- NwBaseItem: `DefaultModel` is now correctly marked as nullable.
- NwClass: `IconResRef`, `SpellTableColumn` properties are now correctly marked as nullable.
- NwClass: `PreReqTable` is now a nullable reference to a TwoDimArray wrapper of the associated class prereq table.
- NwDomain: `Icon` is now correctly marked as nullable.
- NwFeat: `IconResRef` is now correctly marked as nullable.
- NwRace: `DefaultCharacterDescription`, `Description`, `Name`, `PluralName` properties are now correctly marked as nullable.
- NwSkill: `IconResRef` is now correctly marked as nullable.
- NwSpell: `CastGroundVisual`, `CastHandVisual`, `CastHeadVisual`, `CastSound`, `ConjureGroundVisual`, `ConjureHandVisual`, `ConjureHeadVisual`, `ConjureSound`, `IconResRef`, `ImpactScript`, `ProjectileModel`, `ProjectileSound` properties are now correctly marked as nullable.
- NativeObjectExtensions: Moved to `Anvil.Native` to prevent missing method errors when using other extension overloads, and made public again.
- ArrayWrapper: Added additional error checking.
- EnforceLegalCharacterService: Added 8 multiclass support.
- ServerLogRedirectorService: Don't log empty or null messages.
- ScriptDispatchService: Remove redundant try/catch, optimization.
- AnvilCore: Merged `VirtualMachineFunctionHandler` into AnvilCore.
- AnvilCore: Use better performant unmanaged function pointers for handling low level events from NWNX.
- AnvilCore: Implement custom crash handler with managed stacktrace.

### Deprecated
- `IPRacialType` - use `NwRace` instead.
- `HitEffect.SlayRace` - use `NwRace` overload instead.
- `ItemProperty.ACBonusVsRace` - use `NwRace` overload instead.
- `ItemProperty.AttackBonusVsRace` - use `NwRace` overload instead.
- `ItemProperty.DamageBonusVsRace` - use `NwRace` overload instead.
- `ItemProperty.EnhancementBonusVsRace` - use `NwRace` overload instead.
- `ItemProperty.LimitUseByRace` - use `NwRace` overload instead.
- `DamageData` properties - use GetDamageByType, SetDamageByType instead.

### Fixed
- EventService: Built-in game events subscribed in anvil will now preserve the event script identifier when calling the original script.
- ItemAppearance: Now correctly supports the extended appearance types added in 8193.35.
- NwArea: `GetTileInfo` now correctly returns the correct tile when specifying a tile coordinate.
- NwCreature: Fixed `ActionUseFeat` subFeat parameter not working.
- EnforceLegalCharacterService: Fixed an erroneous calculation when calculating ability scores with a level stat bonus.
- WeaponService: Fixed a rare server crash when calculating levels for a specific class.

## 8193.35.1
https://github.com/nwn-dotnet/Anvil/compare/v8193.35.0...v8193.35.1

### Fixed
- (Docker) Fixed an issue with libssl dependency.

## 8193.35.0
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.28...v8193.35.0

### Added
- DamageType: Added `CustomXX` damage type constants
- EffectSubType: Added `Unyielding` subtype.
- EffectType: Added `Pacify`, `BonusFeat`, `TimeStopImmunity`, `ForceWalk` effects.
- GuiEventType: Added `RadialOpen`
- GuiPanel: Added `Tile`, `Trigger`, `Creature`, `Item`, `Placeable`, `Door`, `Quickbar` panels.
- PlayerDeviceProperty: Added various graphics properties.
- Effect: Added `BonusFeat`, `ForceWalk`, `Pacified`, `TimeStopImmunity` effect methods.
- Effect: Added `LinkId`, `IgnoreImmunity` properties.
- Location: Added `TileId`, `TileRotation`, `TileHeight` properties.
- Location: Added `SetTile`, `SetTileAnimationLoops` methods.
- SQLQuery: Added `Reset` method.
- ModuleEvents.OnPlayerEquipItem: Added `Slot` property.
- ModuleEvents.OnPlayerGuiEvent: Added `Vector` property.
- ModuleEvents.OnPlayerUnequipItem: Added `Slot` property.
- SpellEvents.OnSpellCast: Added `IsSpontaneousCast`, `SpellLevel` properties.
- NwArea: Added `ReloadAreaGrass`, `ReloadAreaBorder`, `SetTiles`, `GetAreaLightColor`, `SetAreaLightColor`, `GetAreaLightDirection`, `SetAreaLightDirection`, `SetFogColor` methods.
- NwCreature: Added `ControllingPlayer` setter.
- NwCreature: Added `ActionUseFeat`, `GetSpellUsesLeft`, `SetEffectIconFlashing` methods
- NwGameObject: Added `Usable`, `VisibleDistance`, `VisualTransform`, `UiDiscoveryFlags` properties.
- NwGameObject: Added `GetVisualTransform` method with scope option.
- NwGameObject: Added `ReplaceObjectAnimation`, `ClearObjectAnimationOverride`, `SetTextBubbleOverride` methods.
- NwPlayer: Added `CameraFlags`, `ClientVersionCommitSha1` properties.
- NwPlayer: Added `AttachCamera`, `SetCameraLimits`, `SetShaderUniform`, `SetSpellTargetingData` methods.
- VisualTransform: Added `Clear` method.
- VisualTransformLerpSettings: Added `BehaviorFlags`, `Repeats` properties.
- DevastatingCriticalData: Added `Attacker` property.

### Package Updates
- NWNX: 2692ecb -> d44d373
- NWN.Core: 8193.34.15 -> 8193.35.6
- NWN.Native: 8193.34.5 -> 8193.35.6
- LightInject: 6.6.3 -> 6.6.4
- NLog: 5.1.3 -> 5.1.4

### Changed
- !! Anvil now targets .NET 7. You may need to update your plugin's target framework to successfully compile against Anvil.
- !! HookService: NWNX and NWN.Native no-longer exposes a list of function addresses, and the address parameter has been removed from `RequestHook`. The RequestHook now expects a delegate with the `NativeFunction` attribute. See [here for an example](https://github.com/nwn-dotnet/Anvil/blob/development/NWN.Anvil/src/main/Native/Functions/Functions.CNWSCreature.cs).
- ItemAppearance: Update return values to support the extended part ranges introduced in 8193.35.
- NwPlayer: `ClientVersion` now includes the revision value of the client version.
- NwPlayer: `EnterTargetMode` now specifies a setting object that contains all new options added in 8193.35.

### Removed
- ItemAppearanceArmorModel: Removed deprecated class.
- DoorEvents.OnDialogue: Removed deprecated class.
- PlaceableEvents.OnDialogue: Removed deprecated class.
- ItemAppearanceArmorModel: Removed deprecated class.
- Effect: Removed deprecated `EffectIcon` overload.
- ItemProperty: Removed deprecated `PropertyType` property.
- CreatureEvents.OnConversation: Removed deprecated `CurrentSpeaker` property.
- ModuleEvents.OnNuiEvent: Removed deprecated `WindowToken` property.
- CreatureClassInfo: Removed deprecated `AddKnownSpell`, `GetKnownSpellCountByLevel`, `GetKnownSpells`, `RemoveKnownSpell` methods.
- ItemAppearance: Removed deprecated `ClearArmorPieceColor`, `GetArmorModel`, `GetArmorPieceColor`, `SetArmorModel`, `SetArmorPieceColor` overloads.
- NativeObjectInfoAttribute: Removed unused class.
- NwArea: Removed deprecated `GetFogAmount`, `GetFogColor` methods.
- NwGameObject: Removed deprecated `CreatureAppearanceType` property.
- NwGameObject: Remove `Destroy`, `PlaySoundByStrRef` overloads.
- NwPlayer: Remove `ClearTlkOverride`, `CreateNuiWindow`, `NuiDestroy`, `NuiGetUserData`, `NuiGetWindowId`, `NuiSetUserData`, `SetTlkOverride` methods.
- Color: Removed `ToInt` method.

### Fixed
- (NWNX) Fixed an issue where nested VM scopes would cause an invalid stack and assertion error.

## 8193.34.28
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.27...v8193.34.28

### Fixed
- NwCreature: Fixed an issue where GetAssociates would return an empty list for certain associate types.
- VirtualMachine: Fix an issue where the context object would be incorrectly flagged as invalid.
- Creature.OnDeath: Support Area/Module as the killer of the creature.
- EventService: More reliable handling of game events.

## 8193.34.27
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.26...v8193.34.27

### Added
- NwCreature: Added `BodyBag`, `BodyBagTemplate` properties.
- NwPlaceable: Added `IsBodyBag` property.

### Changed
- OnPlayerGuiEvent: `EffectIcon` property is now nullable.
- OnDebugPlayVisualEffect: `Effect` property is now nullable.
- APIs that accept a `TableEntry` parameter now have implicit casts (e.g. `EffectIconTableEntry` & `EffectIcon`).

### Fixed
- NwCreature: Fixed an infinite loop caused by the `Associates` property when having dominated creature associates.
- Added some index/range checks for some usages of game table data.

## 8193.34.26
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.25...v8193.34.26

### Added
- NwGameTables: Added `EffectIconTable`.
- CreatureClassInfo: Added `School`, `KnownSpells` properties.
- CreatureLevelInfo: Added `AddedKnownSpells`, `RemovedKnownSpells` properties.

### Package Updates
- NWNX: 2692ecb -> d44d373
- NWN.Core: 8193.34.12 -> 8193.34.15
- NWN.Native: 8193.34.5 -> 8193.34.7
- Newtonsoft.Json: 13.0.2 -> 13.0.3
- NLog: 5.1.2 -> 5.1.3
- Paket.Core: 7.2.0 -> 7.2.1

### Changed
- CollectionExtensions: `InsertOrdered` now returns the index in which the item was inserted.
- Anvil will now log a managed stack trace during an assertion failure. We're hoping this will help track down issues where the nwscript VM reports an invalid stack state.

### Deprecated
- `CreatureClassInfo.AddKnownSpell` - use `CreatureClassInfo.KnownSpells[].Add` instead.
- `CreatureClassInfo.RemoveKnownSpell` - use `CreatureClassInfo.KnownSpells[].Remove` instead.
- `CreatureClassInfo.GetKnownSpellCountByLevel` - use `CreatureClassInfo.KnownSpells[].Count` instead.
- `CreatureClassInfo.GetKnownSpells` - use `CreatureClassInfo.KnownSpells` instead.

### Fixed
- Fixed null/empty script names not clearing object event scripts.
- Fixed an issue where an invalid script name could be assigned to an object event.
- Fixed a NRE in the `ModuleEvents.OnAcquireItem` event caused by characters failing ELC.
- Fixed a cast exception in the `PlaceableEvents.OnDisturbed` event when the last inventory event was not caused by a creature.
- NwStore: Fixed `WillNotBuyItems`, `WillOnlyBuyItems` lists not removing items, and LINQ functions (ToList/ToArray) not working.
- CreatureLevelInfo: `ClassInfo` now returns the correct creature class.
- ItemPropertyItemMapTable: Fixed some item property values returning valid when they shouldn't be.

## 8193.34.25
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.24...v8193.34.25

### Added
- Exposed `HomeStorage` class for accessing paths in anvil home.
- Added support for creating `Cassowary` solvers through `new Cassowary()`
- NwGameObject: Added `ActionJumpToLocation` method.
- Events: Added `OnPlayerQuickChat` event.
- NwPlayer: Added object name override support (SetObjectNameOverride)
- NwPlayer: Added player-specific looping vfx support (AddLoopingVisualEffect)
- NwCreature: Added `LevelUp` method that bypasses validation.

### Package Updates
- NLog: 5.1.1 -> 5.1.2
- Microsoft.CodeAnalysis.CSharp: 4.4.0 -> 4.5.0

### Changed
- Optional anvil services will now log a message when they are used.

### Fixed
- !! Fixed a memory leak when not using `Dispose()` on engine structures. (Effect, Location, ItemProperty, Json, SQLQuery, Talent)

## 8193.34.24
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.23...v8193.34.24

### Added
- Implemented AI message and listen system.
  - NwGameObject: Added `IsListening` property.
  - NwGameObject: Added `SetListenPattern` method.
  - NwCreature: Added `SetAssociateListenPatterns` method.
  - OnConversation events: Added `ListenPattern`, `AssociateCommand` properties.
- OnTrapTriggered: Added `TriggeredBy` property to door/placeable traps.
- CreatureClassInfo: Added `Domains` array for reading/modifying creature domains.

### Package Updates
- NWN.Core: 8193.34.10 -> 8193.34.12

### Deprecated
- `DoorEvents.OnDialogue` - use `DoorEvents.OnConversation` instead.
- `PlaceableEvents.OnDialogue` - use `PlaceableEvents.OnConversation` instead.

### Fixed
- Fixed an issue with events that caused script handlers not to be called after reloading anvil.

## 8193.34.23
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.22...v8193.34.23

### Added
- Added `ANVIL_ENCODING` environment variable for specifying a custom encoding when converting native strings from nwserver.
- Added `EncodingService` for changing the server encoding at runtime.
- NUI: Added StrRef support with `NuiBindStrRef` and `NuiValueStrRef` types.
- Events: Added `OnDoorSetOpenState` event.
- Events: Added `OnObjectUse` event.
- Extensions: Added `TryParseObject` extension for parsing object ID strings.
- NwCreature: Added `GetInitiativeModifier`,`SetInitiativeModifier`,`ClearInitiativeModifier` methods.
- NwCreature: Added `IsDMAvatar` property.
- NwCreature: Added `IsFlanking` method.
- NwDoor: Added `DoorOpenState` property.
- NwRuleset: Added NwDomain ruleset table and replaced constant usages with table references.
- NwServer: Added `IsActivePaused` property.
- NwServer: Added `IsTimestopPaused` property.

### Package Updates
- Microsoft.CodeAnalysis.CSharp: 4.3.1 -> 4.4.0
- NWN.Core: 8193.34.7 -> 8193.34.10
- NWN.Native: 8193.34.4 -> 8193.34.5
- LightInject: 6.6.1 -> 6.6.3
- Newtonsoft.Json: 13.0.1 -> 13.0.2
- NLog: 5.0.5 -> 5.1.1
- Paket.Core: 7.1.5 -> 7.2.0
- NWNX: fe195ec -> 2692ecb

### Changed
- Events: `OnSpellAction` Domain and Feat is now nullable.
- Events: `OnSpellInterrupt` Domain and Feat is now nullable.
- Events: `OnSpellSlotMemorize` Domain is now nullable.
- `System.Random` usages now use the `System.Random.Shared` instance, instead of individual instances.

### Fixed
- Fixed an issue where a GameObject or Player could become stuck in a hash-based collection when it became invalid.
- NwPlayer: `IsDM` now correctly returns true when a DM is possessing a creature. Use `ControlledCreature.IsDM` for the prior behaviour.

## 8193.34.22
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.21...v8193.34.22

### Package Updates
- NLog: 5.0.4 -> 5.0.5

### Changed
- OnSpellCast: Item, Spell and TargetObject can be null.
- NwItem: `Clone()` now preserves the item's `Droppable` flag. An optional parameter is provided to keep the old behaviour.

## 8193.34.21
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.20...v8193.34.21

### Added
- NwAreaOfEffect: Added `Radius` property.

### Package Updates
- NWNX: 8faa9d4 -> fe195ec

### Changed
- OnSpellCast: `Caster` and `TargetObject` now correctly use `NwObject` as the event data type.

### Fixed
- Fixed a server crash when a module or area attempted to cast a spell.
- Fixed an edge case where a deleted player's TURD would not be deleted.

## 8193.34.20
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.19...v8193.34.20

### Added
- CollectionExtensions: Added IList `AddRange` extension.
- NwCreature: Added `IsBartering` property.
- NwObject: Added `TryGetUUID` method.
- NwObject: Added `SerializeToJson` method.
- NwStore: Added `BuyStolenGoods`, `MarkDown`, `MarkDownStolen`, `MarkUp`, `WillNotBuyItems`, `WillOnlyBuyItems` properties.

### Package Updates
- LightInject: 6.5.1 -> 6.6.1

### Changed
- Exposed Json engine structure.

### Fixed
- VirtualMachine: Fixed an issue where ObjectSelf would not be correctly assigned.

## 8193.34.19
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.18...v8193.34.19

### Added
- Events: Added `OnLoadCharacterFinish` event.

## 8193.34.18
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.17...v8193.34.18

### Fixed
- Fix an InvalidOperationException being thrown when checking player for equality.

## 8193.34.17
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.16...v8193.34.17

### Added
- NwCreature: Added `SittingObject` property.
- NwArea: Added `LoadScreen` property.
- ResourceManager: Added `CreateResourceDirectory`.

### Fixed
- NwBaseItem: Fixed `ItemClass` returning a type name instead of the item class name.
- Fixed a rare compile issue when using `ToNwObject` caused by exposed native types.

## 8193.34.16
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.15...v8193.34.16

### Added
- Events: Added `OnDebugPlayVisualEffect`, `OnDebugRunScript`, `OnDebugRunScriptChunk` events.
- NwGameObject: Added `AnimationState`.
- Effect: Added `Effect.VisualEffect(VisualEffectTableEntry, bool, float, Vector3, Vector3)` overload.

### Package Updates
- NWN.Core 8193.34.6 -> 8193.34.7
- LightInject 6.4.1 -> 6.5.1
- NLog 5.0.1 -> 5.0.4
- NWNX 5ade7de -> 8faa9d4

### Changed
- SchedulerService: Use `PriorityQueue` to improve main server loop performance.

## 8193.34.15
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.14...v8193.34.15

### Added
- ItemProperty: Added `SubTypeTable`.

### Fixed
- Fixed `CalculateValidItemsForProperty` returning false for base items using column 0.

## 8193.34.14
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.13...v8193.34.14

### Added
- NuiText: Added `Border` and `Scrollbars` properties.
- ItemProperty: Added properties for referencing 2da data.
- ItemProperty: Added Create() factory method overload using the new table class types.
- NwGameTables: Added `ItemPropertyTable`, `ItemPropertyItemMapTable`, `ItemPropertyCostTables`, `ItemPropertyParamTables`

### Changed
- **BREAKING CHANGE:** TwoDimArrays must now be initialized via `NwGameTables.GetTable`.

### Fixed
- Fixed a server crash when using TwoDimArray after the native array structure was evicted from the 2da cache.
- Fixed updating the weight of a equipped item with `NwItem.Weight` not correctly updating creature weight and encumbrance.
- Fixed `NwBaseItem.ArmorCheckPenalty` returning an unsigned integer.

## 8193.34.13
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.12...v8193.34.13

### Package Updates
- NLog 5.0.0 -> 5.0.1
- NWN.Core 8193.34.5 -> 8193.34.6
- NWNX 95e700a -> 5ade7de

### Changed
- NUI: DrawList is now supported on all NUI elements, instead of just layout elements.
- NUI: NuiGroup now supports a non-layout element.

## 8193.34.12
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.11...v8193.34.12

### Added
- NwGameTables: Exposed armor (armor.2da) and parts (parts_*.2da) tables.

## 8193.34.11
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.10...v8193.34.11

### Added
- NwCreature: Added `GetSlotFromItem`
- NwCreature: Added `RunEquip(item, EquipmentSlots)`
- ItemAppearance: Added `ChangeAppearance`
- NuiWindowToken: Added `Get/SetUserData()`
- Effect: Added `SkillIncreaseAll`, `SkillDecreaseAll`

### Package Updates
- NWNX 4842f60 -> 95e700a
- NWN.Native 8193.34.3 -> 8193.34.4
- NWN.Core 8193.34.4 -> 8193.34.5
- NLog 4.7.15 -> 5.0.0

### Deprecated
- `ItemAppearanceArmorModel` enum. Use `CreaturePart` instead.
- `NwPlayer.NuiDestroy`. Use `NuiWindowToken.Close` instead.
- `NwPlayer.NuiGetUserData`. Use `NuiWindowToken.NuiGetUserData` instead.
- `NwPlayer.NuiSetUserData`. Use `NuiWindowToken.NuiSetUserData` instead.
- `NwPlayer.NuiGetWindowId`. Use `NuiWindowToken.NuiGetWindowId` instead.

### Fixed
- Fixed `NwGameObject.Location` setter not working for newly deserialized game objects.
- Fixed a crash when an equipped item was sent to limbo with `NwModule.MoveObjectToLimbo`.
- Fixed `NuiWindowToken.WindowId` returning an empty string from events.
- Speculative fix for a rare native crash when fetching 2da strings.

## 8193.34.10
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.9...v8193.34.10

### Added
- Added ClientVersion/ClientPlatform properties to OnClientConnect event.

### Package Updates
- NWNX 3227d60 -> 4842f60
- NWN.Core 8193.34.3 -> 8193.34.4

## 8193.34.9
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.8...v8193.34.9

### Added
- NwCreature - Added setters for base armor & shield arcane spell failure
- `NuiWindowToken` - small structure with helper methods for controlling NuiWindow instances. Can be created from `NwPlayer.TryCreateNuiWindow`
- NwCreature - Added getter for BaseAttackCount
- NwCreature - Added setter for ChallengeRating.

### Package Updates
- Paket 7.1.4 -> 7.1.5

### Changed
- `NwCreature.Position` will no-longer block the action queue from being modified.
  - This fixes inconsistencies with actions queued after setting the position, but the new position can sometimes not apply for players using drive mode.
  - For players, it is recommended to immobilize them before setting their position.

### Deprecated
- `NwPlayer.TryCreateNuiWindow` - use the `NuiWindowToken` overload instead.
- `NwPlayer.CreateNuiWindow` - use `TryCreateNuiWindow` instead.
- `ModuleEvents.OnNuiEvent.WindowToken` - use `ModuleEvents.OnNuiEvent.Token` instead.

### Fixed
- Fixed `NwGameObject.Location` setter re-triggering Area Enter events.
- Fixed `NwPlayer.PartyMembers` throwing a NRE when a party contained associate creatures.
- Fixed a typo in PlayOptions.OnePartyOnly.

## 8193.34.8
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.7...v8193.34.8

### Deprecated
- `NwObject.Destroy(float delay)` - Use the non-delay overload instead, with the `SchedulerService`.

### Fixed
- Fixed `NwEncounter.Destroy()` not destroying the encounter object.
- Fixed `PlaceableEvents.OnPhysicalAttacked` event always returning no data.

## 8193.34.7
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.6...v8193.34.7

### Package Updates
- NWNX 6a552d9 -> 3227d60

### Fixed
- Fixed an edge case issue that caused some servers to enter an infinite crash loop when shutting down.
- Fixed `NWNX_CORE_SHUTDOWN_SCRIPT` throwing a NRE during shutdown.

## 8193.34.6
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.5...v8193.34.6

### Added
- Added `Color` equality members `Equals` and operators `==` `!=`
- `NwGameTables`: Added `ExpTable`
- `NwGameTables`: Added `SkillItemCostTable`

### Package Updates
- Paket.Core 7.0.2 -> 7.1.4
- NWNX d15bc22 -> 6a552d9

### Fixed
- Fixed an issue that prevented .NET 6 plugins from being loaded with Paket.

## 8193.34.5
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.4...v8193.34.5

### Added
- `NwCreature.Encounter`: Gets the encounter that spawned the creature.
- `NwAreaOfEffect.Spell`: Gets the spell that created the area of effect.
- `NwAreaOfEffect.RemainingDuration`: Gets the remaining duration on the area of effect.

## 8193.34.4
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.3...v8193.34.4

### Fixed
- Fixed an issue where API services would not be constructed in the expected order.

## 8193.34.3
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.2...v8193.34.3

### Added
- `StrRef.ToParsedString()`: Gets the string associated with a StrRef and parses any tokens (e.g. \<CUSTOM0\>)
- `StrTokenCustom`: New structure for resolving/setting custom token values.
- `NwGameTables`: Added new 2da tables
  - `BodyBagTable`
  - `LightColorTable`
  - `PlaceableSoundTable`
  - `PlaceableTable`
  - `ProgrammedEffectTable`
  - `VisualEffectTable`
- `NwArea`: Added `GetTileInfo()`
- Added `InjectionService` & `ServiceBinding` tests.
- `NwPlaceable.Appearance`: Gets or sets the appearance for the placeable.
- `ModuleLoadTracker`: Added core service for tracking module load progress as debug log messages.
  - If a module fails to load due to an error with an area, the area is logged as an error instead.
- `NwPlayer`: Added player name override methods.
  - `PlayerNameOverrideService` contains configuration options.
- `NwCreature`: Added `GetFeatRemainingUses` `GetFeatTotalUses` `SetFeatRemainingUses`
- Added `OnMapPinAddPin`, `OnMapPinChangePin`, `OnMapPinDestroyPin` events.
- `NwCreature`: Added damage level override functions (`ClearDamageLevelOverride`, `GetDamageLevelOverride`, `SetDamageLevelOverride`)
- `NwCreature`: Added `DamageLevel` property.
- `NwGameTables`: Added `DamageLevelTable`

### Package Updates
- NWNX c51d233 -> d15bc22
- NWN.Core 8193.34.2 -> 8193.34.3
- LightInject 6.4.0 -> 6.4.1
- NLog 4.7.13 -> 4.7.15
- Paket.Core 6.2.1 -> 7.0.2

### Changed
- Rewrote core services and initialisation logic for easier extensibility, and reduced coupling with AnvilCore.
  - All core services now implement `ICoreService`, an interface containing specific event functions that are called at specific times in the server lifecycle.
  - Core services are executed in the order defined by `ServiceBindingOptions`.
  - The CoreService composition root is defined in `AnvilServiceManager`.
  - AnvilCore is now "dumber", and simply passes signals to `AnvilServiceManager` and `VirtualMachineFunctionHandler`.
  - `AnvilServiceManager` merges service initialization in `AnvilCore`, with the container/composition root setup from `IContainerFactory`
- `OnPlayerDeath.Killer` now tries to `GetLastDamager` when `GetLastHostileActor` is invalid.

### Deprecated
- `NwGameObject.CreatureAppearanceType`. Use `NwCreature.Appearance` instead.
- APIs using int-based StrRef parameters have been deprecated. Please use the StrRef overloads:
  - `NwGameObject.PlaySoundByStrRef()`
  - `NwPlayer.ClearTlkOverride()`
  - `NwPlayer.SetTlkOverride()`
  - `NwBaseItem.BaseItemStatsText`
  - `NwBaseItem.Description`
  - `NwBaseItem.Name`
  - `NwClass.Description`
  - `NwClass.Name`
  - `NwClass.NameLower`
  - `NwClass.NamePlural`
  - `NwFeat.Description`
  - `NwFeat.Name`
  - `NwSkill.Description`
  - `NwSkill.Name`
  - `NwSpell.AltMessage`
  - `NwSpell.Description`
  - `NwSpell.Name`
  - `OnELCValidationFailure.StrRef`

### Fixed
- Fixed a stack overflow when injecting the `InjectionService` as a property dependency.
- Unload is now triggered on all plugins before waiting for the assemblies to be unloaded. This fixes some edge cases where assemblies would not unload.

## 8193.34.2
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.1...v8193.34.2

### Added
- Added net6.0 target framework. Since multiple frameworks are now being targeted, there is a small change to the binary output paths.
  - When building locally, binaries are now located in `NWN.Anvil/bin/Release/<framework>`. Release binaries on github will now have a folder for each framework.
- `NwColor`: Added `FromRGBA`, `ToRGBA` `ToUnsignedRGBA` methods. `ToString` is now more explicit.
- Added various tests for `Color` conversion.
- Added `ResourceManager` tests.
- `NwArea`: Added various properties for environment visual options and metadata.
- `NwArea`: Added `CreateEnvironmentPreset`, `ApplyEnvironmentPreset` for saving and loading preset visual options.
- `NwWaypoint.Create`: Added overload without template parameter for creating a general waypoint.
- `PlayOptions`: Added PlayerPartyControl option.
- `StrRef`: Added value structure for string references (StrRefs). The associated talk table string can be resolved by invoking `ToString`.
- `NwGameTables`: Contains static members for commonly used 2das (internally cached by `CTwoDimArrays`).
  - Implemented `AppearanceTable` & `EnvironmentPresetTable`
- Implemented new `TwoDimArray`, `TwoDimArrayEntry` and `ITwoDimArrayEntry` APIs.
  - Supports general usage through `TwoDimArray`, and a generic `TwoDimArray<T>` type for specifying a custom row format. See the docs for more info.
- `ResourceManager`: Added `DeleteTempResource` and `GetResourceText`.
- `ResourceManager`: Added string overload for `WriteTempResource`.
- `LocalVariableStruct`: Added local variable type for serializing any C# type to JSON.
- `PersistentVariableStruct`: Added persistent variable type for serializing any C# type to JSON.
- Added `OnTriggerEnter` event.

### Package Updates
- NWNX: 790a54b -> c51d233
- NWN.Core: 8193.34.1 -> 8193.34.2
- NWN.Native: 8193.34.2 -> 8193.34.3

### Changed
- Change test assertion pattern to use NUnit constraints: https://docs.nunit.org/artcles/nunit/writing-tests/assertions/assertion-models/constraint.html
- Code samples are now built as a separate plugin project, and included in CI analysis.
- Improved path validation for `Delete/WriteTempResource`. It should no-longer be possible to navigate outside of the resource folder.

### Deprecated
- `NwServer.ReloadRules()`. Use `NwRuleset.ReloadRules()` instead.
- `NwColor.ToInt()`. Use `NwColor.ToRGBA()` instead.
- `ITwoDimArray`/`TwoDimArrayFactory`: The 2da APIs have been superseded by a simpler API. See the `ITwoDimArrayEntry` example for more info.
- `NwArea.GetFogAmount`: Use `SunFogAmount` and `MoonFogAmount` instead.
- `NwArea.GetFogColor`: Use `SunFogColor` and `MoonFogColor` instead.

### Removed
- NuiColor was removed and functionality replaced with the standard `Color` class. The intention is to remove confusion and conversion issues when interacting with both types.

### Fixed
- Fixed a NRE when using visibility properties.
- Fixed a NRE when using `PersistentVariableEnum`.

## 8193.34.1
https://github.com/nwn-dotnet/Anvil/compare/v8193.34.0...v8193.34.1

### Added
- NwPlayer: `IsConnected` boolean added. Should be checked when enumerating `NwModule.Players`
- NwPlayer: Added `DMPossessCreature` and `UnpossessCreature` for controlling player creature.
- NwPlayer: `ForceExamine` now supports creatures, placeables, items and doors.
- Implemented `NWN.Anvil.TestRunner` for running automated tests.
- NwPlayer: Added `Get/SetPersonalVisibilityOverride` methods for customizing object visibility per player.
- NwGameObject: Added `VisibilityOverride` property for customizing object visibility globally.
- Creature Events: Added `OnCreatureCheckProficiencies` event.
- Added `Local/Campaign/PersistentVariableEnum<T>` object variable type for user enum types.  The underlying type must be an integer.
- NwGameObject: Added `Clone` method for cloning non-creature and item objects.
- NwDoor: Added `Create` method for creating doors from ResRefs.
- NwEncounter: Added `Create` method for creating encounters from ResRefs.
- NwSound: Added `Create` method for creating sound objects from ResRefs.
- NwTrigger: Added `Create` method for creating triggers from ResRefs.
- NWN.Anvil.TestRunner: Added generator for generating ResRef constants from the standard creator palette.

### Package Updates
- NWN.Core: 8193.34.0 -> 8193.34.1

### Changed
- `NwCreature.WalkRateCap` and `NwCreature.AlwaysWalk` properties are no-longer persistent. Additionally, the services and functions are not hooked until the associated property is used for the first time.
- `NwObject.ObjectId` is now public.

### Fixed
- `AnvilCore.Reload()` now uses the scheduler service to schedule the reload. This should fix some edge cases where async methods would hold a reference preventing unload.
- Fixed an issue where the `SchedulerService` would throw an exception if the server was shutdown/reloaded during a schedule callback.
- Fixed a rare crash when subscribed to effect events.

## 8193.34.0
https://github.com/nwn-dotnet/Anvil/compare/v8193.33.5...v8193.34.0

### Added
- `NwBaseItem`: Added various properties.
- `NwSpell`: Added various properties.
- `NwClass`: Added various properties.
- Added support for `NuiList` & `NuiListTemplateCell`.
- `Effect.DurationType`: Added setter.
- Added assembly attribute `PluginInfo` for defining optional dependencies.
- `PluginManager`: Added check for missing types from optional plugins.

### Package Updates
- (Docker) NWNX: 16b2c88 -> 790a54b
- NWN.Core: 8193.33.4 -> 8193.34.0
- NWN.Native: 8193.33.4 -> 8193.34.2

### Changed
- Services implementing `IInitializable` are now executed in deterministic order based on the service binding order defined in `ServiceBindingOptions`.
- Updated APIs to use ruleset classes
  - `Effect.SkillDecrease(NwSkill,int)`
  - `Effect.SkillIncrease(NwSkill,int)`
  - `Effect.Spell`
  - `ItemProperty.DecreaseSkill(NwSkill,int)`
  - `ItemProperty.SkillBonus(NwSkill,int)`
  - `Talent.Feat`
  - `Talent.Skill`
  - `Talent.Spell`
  - `NwSkill.ToTalent(this NwSkill)`
  - `NwSkill.ToTalent(this NwSpell)`
  - `NwSkill.ToTalent(this NwFeat)`
  - `CreatureEvents.OnSpellCastAt.Spell`
  - `CreatureEvents.OnSpellCastAt.Signal(NwObject,NwCreature,NwSpell,bool)`
  - `DoorEvents.OnSpellCastAt.Spell`
  - `DoorEvents.OnSpellCastAt.Signal(NwObject,NwDoor,NwSpell,bool)`
  - `ModuleEvents.OnPlayerGuiEvent.FeatSelection`
  - `ModuleEvents.OnPlayerGuiEvent.SkillSelection`
  - `PlaceableEvents.OnSpellCastAt.Spell`
  - `PlaceableEvents.OnSpellCastAt.Signal(NwObject,NwPlaceable,NwSpell,bool)`
  - `OnDisarmWeapon.Feat`
  - `OnUseFeat.Feat`
  - `OnUseSkill.Skill`
  - `OnSpellAction.Feat`
  - `OnSpellAction.Spell`
  - `OnSpellBroadcast.Feat`
  - `OnSpellBroadcast.Spell`
  - `OnSpellCast.Spell`
  - `OnSpellInterrupt.Feat`
  - `OnSpellInterrupt.Spell`
  - `OnSpellSlotMemorize.Spell`
  - `SpellEvents.OnSpellCast.Spell`
  - `SpellEvents.OnSpellCast.SpellCastClass`
  - `CreatureClassInfo.Class`
  - `CreatureClassInfo.AddKnownSpell(NwSpell,byte)`
  - `CreatureClassInfo.ClearMemorizedKnownSpells(NwSpell)`
  - `CreatureClassInfo.GetKnownSpells(byte)`
  - `CreatureClassInfo.RemoveKnownSpell(byte,NwSpell)`
  - `CreatureLevelInfo.Class`
  - `CreatureLevelInfo.Feats`
  - `CreatureLevelInfo.GetSkillRank(NwSkill)`
  - `CreatureLevelInfo.SetSkillRank(NwSkill,sbyte)`
  - `CreatureTypeFilter.Class(NwClass)`
  - `CreatureTypeFilter.DoesNotHaveSpellEffect(NwSpell)`
  - `CreatureTypeFilter.HasSpellEffect(NwSpell)`
  - `MemorizedSpellSlot.Spell`
  - `NwCreature.Classes`
  - `NwCreature.Feats`
  - `NwCreature.ActionCastFakeSpellAt(NwSpell,Location,ProjectilePathType)`
  - `NwCreature.ActionCastFakeSpellAt(NwSpell,NwGameObject,ProjectilePathType)`
  - `NwCreature.ActionUseFeat(NwFeat,NwGameObject)`
  - `NwCreature.ActionUseSkill(NwSkill,NwGameObject,SubSkill,NwItem)`
  - `NwCreature.AddFeat(NwFeat)`
  - `NwCreature.AddFeat(NwFeat,int)`
  - `NwCreature.DecrementRemainingFeatUses(NwFeat,int)`
  - `NwCreature.DoSkillCheck(NwSkill,int)`
  - `NwCreature.GetClassDomains(NwClass)`
  - `NwCreature.GetClassInfo(NwClass)`
  - `NwCreature.GetFeatGainLevel(NwFeat)`
  - `NwCreature.GetSkillRank(NwSkill,bool)`
  - `NwCreature.GetSpecialization(NwClass)`
  - `NwCreature.HasFeatEffect(NwFeat)`
  - `NwCreature.HasFeatPrepared(NwFeat)`
  - `NwCreature.HasSkill(NwSkill)`
  - `NwCreature.HasSpellEffect(NwSpell)`
  - `NwCreature.HasSpellUse(NwSpell)`
  - `NwCreature.IncrementRemainingFeatUses(NwFeat,int)`
  - `NwCreature.KnowsFeat(NwFeat)`
  - `NwCreature.LevelUpHenchman(NwClass,PackageType,bool)`
  - `NwCreature.MeetsFeatRequirements(NwFeat)`
  - `NwCreature.RemoveFeat(NwFeat)`
  - `NwCreature.SetSkillRank(NwSkill)`
  - `NwGameObject.ActionCastSpellAt(NwSpell,NwGameObject,MetaMagic,bool,int,ProjectilePathType,bool)`
  - `NwGameObject.ActionCastSpellAt(NwSpell,Location,MetaMagic,bool,int,ProjectilePathType,bool)`
  - `SpecialAbility(NwSpell,byte,bool)`
  - `SpecialAbility.Spell`
  - `OnELCSkillValidationFailure.Skill`
  - `OnELCSkillValidationFailure.Feat`
  - `OnELCSkillValidationFailure.Spell`
  - `WeaponService.Add***Feat(NwBaseItem,NwFeat)`

### Deprecated
- `CursorTargetService.EnterTargetMode` - Use NwPlayer.EnterTargetMode/NwPlayer.TryEnterTargetMode instead.

### Removed
- `Effect.AreaOfEffect(int,string,string,string)`
- `AttributeExtensions`
- `StandardFactionExtensions`
- `CreatureTypeFilter.RacialType`
- `Inventory.CheckFit(BaseItemType)`
- `NwCreature.RacialType`
- `NwCreature.ChangeToStandardFaction`
- `NwItem.BaseItemType`
- `NwItem.CanStack`
- `NwItem.IsStackable`
- `NwPlayer.NuiSetGroupLayout(int,string,NuiGroup)`
- `NwPlayer.NuiSetGroupLayout(int,string,NuiWindow)`
- `NwFaction(int)`
- `Anvil.Services.NwDateTime`
- `LoopTimeService`
- `ServiceBindingOptions.MissingPluginDependencies`
- `ServiceBindingOptions.Order`

## 8193.33.5
https://github.com/nwn-dotnet/Anvil/compare/v8193.33.4...v8193.33.5

### Added
- NwModule: Added `LimboGameObjects` property to list all GameObjects currently stored in limbo.
- NwModule: Added `MoveObjectToLimbo` method to remove a GameObject from an area and store it in limbo.
- NwCreature: Added `AlwaysWalk` and `WalkRateCap` for restricting creature & player movement.
- Added `OnCheckEffectImmunity` event for bypassing effect immunity checks.
- Added `OnEffectApply` and `OnEffectRemove` events.
- Added ruleset APIs: `NwRuleset`, `NwBaseItem`, `NwClass`, `NwFeat`, `NwRace` `NwSkill` and `NwSpell`

### Changed
- Migrated LoopTimeService properties to static class `Anvil.API.Time`.
- Exposed `ScheduledTask` to Scheduler Service.
- Services implementing `IUpdateable` are now executed in deterministic order based on the service binding order defined in `ServiceBindingOptions`.

### Deprecated
- `LoopTimeService` - use `Anvil.API.Time` instead.
- Moved `NwDateTime` and `NwTimeSpan` to `Anvil.API` namespace.
- Duplicated APIs `NwItem.CanStack`/`NwItem.IsStackable`. Use `NwBaseItem.IsStackable` instead.

## 8193.33.4
https://github.com/nwn-dotnet/Anvil/compare/v8193.33.3...v8193.33.4

### Package Updates
- (Docker) Update NWNX to [16b2c88](https://github.com/nwnxee/unified/commit/16b2c88b1f3e0ff929a8cad759090ce98d7d0b3c).

### Changed
- Startup log now includes the git revision of the current running server binary.
- Server revision is now printed to stdout if startup fails.

## 8193.33.3
https://github.com/nwn-dotnet/Anvil/compare/v8193.33.2...v8193.33.3

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
