using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Internal;
using Anvil.Services;
using NWN.API.Constants;
using NWN.Native.API;
using Alignment = Anvil.API.Alignment;

namespace NWN.API.Events
{
  public sealed unsafe class DMEventFactory : SingleHookEventFactory<DMEventFactory.HandleDMMessageHook>
  {
    public delegate int HandleDMMessageHook(void* pMessage, void* pPlayer, byte nMinor, int bGroup);

    protected override FunctionHook<HandleDMMessageHook> RequestHook()
    {
      delegate* unmanaged<void*, void*, byte, int, int> pHook = &OnHandleDMMessage;
      return HookService.RequestHook<HandleDMMessageHook>(pHook, FunctionsLinux._ZN11CNWSMessage40HandlePlayerToServerDungeonMasterMessageEP10CNWSPlayerhi, HookOrder.Early);
    }

    [UnmanagedCallersOnly]
    private static int OnHandleDMMessage(void* pMessage, void* pPlayer, byte nMinor, int bGroup)
    {
      NwPlayer dungeonMaster = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer();
      CNWSMessage message = CNWSMessage.FromPointer(pMessage);

      switch ((MessageDungeonMasterMinor)nMinor)
      {
        case MessageDungeonMasterMinor.SpawnCreature:
        case MessageDungeonMasterMinor.SpawnItem:
        case MessageDungeonMasterMinor.SpawnTrigger:
        case MessageDungeonMasterMinor.SpawnWaypoint:
        case MessageDungeonMasterMinor.SpawnEncounter:
        case MessageDungeonMasterMinor.SpawnPortal:
        case MessageDungeonMasterMinor.SpawnPlaceable:
          return HandleSpawnEvent(pMessage, pPlayer, nMinor, bGroup, dungeonMaster, message, (MessageDungeonMasterMinor)nMinor).ToInt();
        case MessageDungeonMasterMinor.Difficulty:
          return HandleChangeDifficultyEvent(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.ViewInventory:
          return HandleViewInventoryEvent(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.SpawnTrapOnObject:
          return HandleSpawnTrapOnObjectEvent(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Heal:
          return HandleGroupTargetEvent<OnDMHeal>(dungeonMaster, message, bGroup.ToBool()) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Kill:
          return HandleGroupTargetEvent<OnDMKill>(dungeonMaster, message, bGroup.ToBool()) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Invulnerable:
          return HandleGroupTargetEvent<OnDMToggleInvulnerable>(dungeonMaster, message, bGroup.ToBool()) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Rest:
          return HandleGroupTargetEvent<OnDMForceRest>(dungeonMaster, message, bGroup.ToBool()) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Limbo:
          return HandleGroupTargetEvent<OnDMLimbo>(dungeonMaster, message, bGroup.ToBool()) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.ToggleAI:
          return HandleGroupTargetEvent<OnDMToggleAI>(dungeonMaster, message, bGroup.ToBool()) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Immortal:
          return HandleGroupTargetEvent<OnDMToggleImmortal>(dungeonMaster, message, bGroup.ToBool()) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Goto:
          return HandleSingleTargetEvent<OnDMGoTo>(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Possess:
          return HandleSingleTargetEvent<OnDMPossess>(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Impersonate:
          return HandleSingleTargetEvent<OnDMPossessFullPower>(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.ToggleLock:
          return HandleSingleTargetEvent<OnDMToggleLock>(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.DisableTrap:
          return HandleSingleTargetEvent<OnDMDisableTrap>(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Manifest:
          return HandleStandardEvent<OnDMAppear>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Unmanifest:
          return HandleStandardEvent<OnDMDisappear>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.SetFaction:
        case MessageDungeonMasterMinor.SetFactionByName:
          return HandleStandardEvent<OnDMSetFaction>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.TakeItem:
          return HandleStandardEvent<OnDMTakeItem>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.SetStat:
          return HandleStandardEvent<OnDMSetStat>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.GetVar:
          return HandleStandardEvent<OnDMGetVariable>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.SetVar:
          return HandleStandardEvent<OnDMSetVariable>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.SetTime:
          return HandleStandardEvent<OnDMSetTime>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.SetDate:
          return HandleStandardEvent<OnDMSetDate>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.SetFactionReputation:
          return HandleStandardEvent<OnDMSetFactionReputation>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.GetFactionReputation:
          return HandleStandardEvent<OnDMGetFactionReputation>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.Logout:
          return HandleStandardEvent<OnDMPlayerDMLogout>(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.GotoPoint:
          return HandleTeleportEvent<OnDMJumpToPoint>(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.GotoPointAllPlayers:
          return HandleTeleportEvent<OnDMJumpAllPlayersToPoint>(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.GotoPointTarget:
          return HandleJumpTargetToPointEvent(dungeonMaster, message, bGroup.ToBool()) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.DumpLocals:
          return HandleDumpLocalsEvent(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.GiveGoodAlignment:
        case MessageDungeonMasterMinor.GiveEvilAlignment:
        case MessageDungeonMasterMinor.GiveLawfulAlignment:
        case MessageDungeonMasterMinor.GiveChaoticAlignment:
          return HandleGiveAlignmentEvent(dungeonMaster, message, (MessageDungeonMasterMinor)nMinor) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.GiveXP:
          return HandleGiveEvent<OnDMGiveXP>(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.GiveLevel:
          return HandleGiveEvent<OnDMGiveLevel>(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.GiveGold:
          return HandleGiveEvent<OnDMGiveGold>(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        case MessageDungeonMasterMinor.GiveItem:
          return HandleGiveItemEvent(pMessage, pPlayer, nMinor, bGroup, dungeonMaster, message).ToInt();
        case MessageDungeonMasterMinor.Login:
          return HandlePlayerDMLoginEvent(dungeonMaster, message) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        default:
          return Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup);
      }
    }

    private static bool HandleStandardEvent<TEvent>(NwPlayer dungeonMaster) where TEvent : DMStandardEvent, new()
    {
      TEvent eventData = ProcessEvent(new TEvent
      {
        DungeonMaster = dungeonMaster,
      });

      return !eventData.Skip;
    }

    private static bool HandleSingleTargetEvent<TEvent>(NwPlayer dungeonMaster, CNWSMessage message) where TEvent : DMSingleTargetEvent, new()
    {
      NwObject target = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwObject();
      TEvent eventData = ProcessEvent(new TEvent
      {
        DungeonMaster = dungeonMaster,
        Target = target,
      });

      return !eventData.Skip;
    }

    private static bool HandleGroupTargetEvent<TEvent>(NwPlayer dungeonMaster, CNWSMessage message, bool isGroup) where TEvent : DMGroupTargetEvent, new()
    {
      int offset = 0;
      int groupSize = 1;

      if (isGroup)
      {
        groupSize = message.PeekMessage<int>(offset);
        offset += sizeof(int);
      }

      NwObject[] targets = new NwObject[groupSize];
      for (int i = 0; i < groupSize; i++)
      {
        targets[i] = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject();
        offset += sizeof(uint);
      }

      TEvent eventData = ProcessEvent(new TEvent
      {
        DungeonMaster = dungeonMaster,
        Targets = targets,
      });

      return !eventData.Skip;
    }

    private static bool HandleTeleportEvent<TEvent>(NwPlayer dungeonMaster, CNWSMessage message) where TEvent : DMTeleportEvent, new()
    {
      int offset = 0;

      NwArea area = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject<NwArea>();
      offset += sizeof(uint);
      float x = message.PeekMessage<float>(offset);
      offset += sizeof(float);
      float y = message.PeekMessage<float>(offset);
      offset += sizeof(float);
      float z = message.PeekMessage<float>(offset);

      TEvent eventData = ProcessEvent(new TEvent
      {
        DungeonMaster = dungeonMaster,
        TargetArea = area,
        TargetPosition = new Vector3(x, y, z),
      });

      return !eventData.Skip;
    }

    private static bool HandleGiveEvent<TEvent>(NwPlayer dungeonMaster, CNWSMessage message) where TEvent : DMGiveEvent, new()
    {
      int amount = message.PeekMessage<int>(0);
      NwGameObject target = (message.PeekMessage<uint>(4) & 0x7FFFFFFF).ToNwObject<NwGameObject>();

      TEvent eventData = ProcessEvent(new TEvent
      {
        DungeonMaster = dungeonMaster,
        Amount = amount,
        Target = target,
      });

      return !eventData.Skip;
    }

    private static bool HandleSpawnEvent(void* pMessage, void* pPlayer, byte nMinor, int bGroup, NwPlayer dungeonMaster, CNWSMessage message, MessageDungeonMasterMinor spawnType)
    {
      ObjectTypes objectType = spawnType switch
      {
        MessageDungeonMasterMinor.SpawnCreature => ObjectTypes.Creature,
        MessageDungeonMasterMinor.SpawnItem => ObjectTypes.Item,
        MessageDungeonMasterMinor.SpawnPlaceable => ObjectTypes.Placeable,
        MessageDungeonMasterMinor.SpawnWaypoint => ObjectTypes.Waypoint,
        MessageDungeonMasterMinor.SpawnTrigger => ObjectTypes.Trigger,
        MessageDungeonMasterMinor.SpawnEncounter => ObjectTypes.Encounter,
        MessageDungeonMasterMinor.SpawnPortal => ObjectTypes.Invalid,
        _ => throw new ArgumentOutOfRangeException(),
      };

      int offset = 0;

      NwArea area = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject<NwArea>();
      offset += sizeof(uint);

      float x = message.PeekMessage<float>(offset);
      offset += sizeof(float);
      float y = message.PeekMessage<float>(offset);
      offset += sizeof(float);
      float z = message.PeekMessage<float>(offset);
      offset += sizeof(float);

      if (objectType == ObjectTypes.Placeable)
      {
        // Placeables have extra orientation data
        offset += sizeof(float) + sizeof(float) + sizeof(float);
      }

      string resRef = message.PeekMessageResRef(offset);
      uint gameObjectId = LowLevel.ServerExoApp.GetObjectArray().m_nNextObjectArrayID[0];

      OnDMSpawnObjectBefore beforeEventData = ProcessEvent(new OnDMSpawnObjectBefore
      {
        DungeonMaster = dungeonMaster,
        Area = area,
        Position = new Vector3(x, y, z),
        ResRef = resRef,
      });

      bool skipped = beforeEventData.Skip || !Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup).ToBool();

      if (!skipped)
      {
        ProcessEvent(new OnDMSpawnObjectAfter
        {
          DungeonMaster = dungeonMaster,
          SpawnedObject = gameObjectId.ToNwObject<NwGameObject>(),
        });
      }

      return !skipped;
    }

    private static bool HandleChangeDifficultyEvent(NwPlayer dungeonMaster, CNWSMessage message)
    {
      GameDifficulty difficulty = (GameDifficulty)message.PeekMessage<int>(0);
      OnDMChangeDifficulty eventData = ProcessEvent(new OnDMChangeDifficulty
      {
        DungeonMaster = dungeonMaster,
        NewDifficulty = difficulty,
      });

      return !eventData.Skip;
    }

    private static bool HandleViewInventoryEvent(NwPlayer dungeonMaster, CNWSMessage message)
    {
      bool isOpening = message.PeekMessage<int>(0).ToBool();
      NwGameObject target = (message.PeekMessage<uint>(4) & 0x7FFFFFFF).ToNwObject<NwGameObject>();

      OnDMViewInventory eventData = ProcessEvent(new OnDMViewInventory
      {
        DungeonMaster = dungeonMaster,
        Target = target,
        IsOpening = isOpening,
      });

      return !eventData.Skip;
    }

    private static bool HandleSpawnTrapOnObjectEvent(NwPlayer dungeonMaster, CNWSMessage message)
    {
      NwStationary target = (message.PeekMessage<uint>(4) & 0x7FFFFFFF).ToNwObject<NwStationary>();
      OnDMSpawnTrapOnObject eventData = ProcessEvent(new OnDMSpawnTrapOnObject
      {
        DungeonMaster = dungeonMaster,
        Target = target,
      });

      return !eventData.Skip;
    }

    private static bool HandleJumpTargetToPointEvent(NwPlayer dungeonMaster, CNWSMessage message, bool isGroup)
    {
      int offset = 0;
      int groupSize = 1;

      NwArea area = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject<NwArea>();
      offset += sizeof(uint);
      float x = message.PeekMessage<float>(offset);
      offset += sizeof(float);
      float y = message.PeekMessage<float>(offset);
      offset += sizeof(float);
      float z = message.PeekMessage<float>(offset);
      offset += sizeof(float);

      Vector3 position = new Vector3(x, y, z);
      if (isGroup)
      {
        groupSize = message.PeekMessage<int>(offset);
        offset += sizeof(int);
      }

      NwGameObject[] targets = new NwGameObject[groupSize];
      for (int i = 0; i < targets.Length; i++)
      {
        targets[i] = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject<NwGameObject>();
        offset += sizeof(uint);
      }

      OnDMJumpTargetToPoint eventData = ProcessEvent(new OnDMJumpTargetToPoint
      {
        DungeonMaster = dungeonMaster,
        Targets = targets,
        NewArea = area,
        NewPosition = position,
      });

      return !eventData.Skip;
    }

    private static bool HandleDumpLocalsEvent(NwPlayer dungeonMaster, CNWSMessage message)
    {
      DumpLocalsType type = (DumpLocalsType)message.PeekMessage<int>(0);

      NwObject target = type switch
      {
        DumpLocalsType.DumpLocals => (message.PeekMessage<uint>(4) & 0x7FFFFFFF).ToNwObject(),
        DumpLocalsType.DumpAreaLocals => dungeonMaster.ControlledCreature.Area,
        DumpLocalsType.DumpModuleLocals => NwModule.Instance,
        _ => throw new ArgumentOutOfRangeException(),
      };

      OnDMDumpLocals eventData = ProcessEvent(new OnDMDumpLocals
      {
        DungeonMaster = dungeonMaster,
        Type = type,
        Target = target,
      });

      return !eventData.Skip;
    }

    private static bool HandleGiveAlignmentEvent(NwPlayer dungeonMaster, CNWSMessage message, MessageDungeonMasterMinor alignmentType)
    {
      Alignment alignment = alignmentType switch
      {
        MessageDungeonMasterMinor.GiveGoodAlignment => Alignment.Good,
        MessageDungeonMasterMinor.GiveEvilAlignment => Alignment.Evil,
        MessageDungeonMasterMinor.GiveLawfulAlignment => Alignment.Lawful,
        MessageDungeonMasterMinor.GiveChaoticAlignment => Alignment.Chaotic,
        _ => throw new ArgumentOutOfRangeException(),
      };

      int amount = message.PeekMessage<int>(0);
      NwGameObject target = (message.PeekMessage<uint>(4) & 0x7FFFFFFF).ToNwObject<NwGameObject>();

      OnDMGiveAlignment eventData = ProcessEvent(new OnDMGiveAlignment
      {
        DungeonMaster = dungeonMaster,
        Alignment = alignment,
        Amount = amount,
        Target = target,
      });

      return !eventData.Skip;
    }

    private static bool HandleGiveItemEvent(void* pMessage, void* pPlayer, byte nMinor, int bGroup, NwPlayer dungeonMaster, CNWSMessage message)
    {
      NwGameObject target = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwObject<NwGameObject>();
      uint itemId = LowLevel.ServerExoApp.GetObjectArray().m_nNextObjectArrayID[0];

      OnDMGiveItemBefore beforeEventData = ProcessEvent(new OnDMGiveItemBefore
      {
        DungeonMaster = dungeonMaster,
        Target = target,
      });

      bool skipped = beforeEventData.Skip || !Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup).ToBool();

      if (!skipped)
      {
        ProcessEvent(new OnDMGiveItemAfter
        {
          DungeonMaster = dungeonMaster,
          Target = target,
          Item = itemId.ToNwObject<NwItem>(),
        });
      }

      return !skipped;
    }

    private static bool HandlePlayerDMLoginEvent(NwPlayer dungeonMaster, CNWSMessage message)
    {
      string password = message.PeekMessageString(0);

      OnDMPlayerDMLogin eventData = ProcessEvent(new OnDMPlayerDMLogin
      {
        DungeonMaster = dungeonMaster,
        Password = password,
      });

      return !eventData.Skip;
    }
  }
}
