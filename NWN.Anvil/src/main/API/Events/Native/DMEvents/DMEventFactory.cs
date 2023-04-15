using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.Internal;
using Anvil.Native;
using Anvil.Services;
using NLog;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed unsafe class DMEventFactory : HookEventFactory
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static FunctionHook<Functions.CNWSMessage.HandlePlayerToServerDungeonMasterMessage> Hook { get; set; } = null!;

    protected override IDisposable[] RequestHooks()
    {
      delegate* unmanaged<void*, void*, byte, int, int> pHook = &OnHandleDMMessage;
      Hook = HookService.RequestHook<Functions.CNWSMessage.HandlePlayerToServerDungeonMasterMessage>(pHook, HookOrder.Early);
      return new IDisposable[] { Hook };
    }

    private static OnDMChangeDifficulty HandleChangeDifficultyEvent(NwPlayer dungeonMaster, CNWSMessage message)
    {
      GameDifficulty difficulty = (GameDifficulty)message.PeekMessage<int>(0);
      OnDMChangeDifficulty eventData = ProcessEvent(EventCallbackType.Before, new OnDMChangeDifficulty
      {
        DungeonMaster = dungeonMaster,
        NewDifficulty = difficulty,
      });

      return eventData;
    }

    private static OnDMDumpLocals HandleDumpLocalsEvent(NwPlayer dungeonMaster, CNWSMessage message)
    {
      DumpLocalsType type = (DumpLocalsType)message.PeekMessage<int>(0);

      NwObject target = type switch
      {
        DumpLocalsType.DumpLocals => (message.PeekMessage<uint>(4) & 0x7FFFFFFF).ToNwObject()!,
        DumpLocalsType.DumpAreaLocals => dungeonMaster.ControlledCreature!.Area!,
        DumpLocalsType.DumpModuleLocals => NwModule.Instance,
        _ => throw new ArgumentOutOfRangeException(),
      };

      OnDMDumpLocals eventData = ProcessEvent(EventCallbackType.Before, new OnDMDumpLocals
      {
        DungeonMaster = dungeonMaster,
        Type = type,
        Target = target,
      });

      return eventData;
    }

    private static OnDMGiveAlignment HandleGiveAlignmentEvent(NwPlayer dungeonMaster, CNWSMessage message, MessageDungeonMasterMinor alignmentType)
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
      NwGameObject target = (message.PeekMessage<uint>(4) & 0x7FFFFFFF).ToNwObject<NwGameObject>()!;

      OnDMGiveAlignment eventData = ProcessEvent(EventCallbackType.Before, new OnDMGiveAlignment
      {
        DungeonMaster = dungeonMaster,
        Alignment = alignment,
        Amount = amount,
        Target = target,
      });

      return eventData;
    }

    private static TEvent HandleGiveEvent<TEvent>(NwPlayer dungeonMaster, CNWSMessage message) where TEvent : DMGiveEvent, new()
    {
      int amount = message.PeekMessage<int>(0);
      NwGameObject target = (message.PeekMessage<uint>(4) & 0x7FFFFFFF).ToNwObject<NwGameObject>()!;

      TEvent eventData = ProcessEvent(EventCallbackType.Before, new TEvent
      {
        DungeonMaster = dungeonMaster,
        Amount = amount,
        Target = target,
      });

      return eventData;
    }

    private static int HandleGiveItemEvent(void* pMessage, void* pPlayer, byte nMinor, int bGroup, NwPlayer dungeonMaster, CNWSMessage message)
    {
      NwGameObject target = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwObject<NwGameObject>()!;
      uint itemId = LowLevel.ServerExoApp.GetObjectArray().m_nNextObjectArrayID[0];

      OnDMGiveItem eventData = ProcessEvent(EventCallbackType.Before, new OnDMGiveItem
      {
        DungeonMaster = dungeonMaster,
        Target = target,
      });

      bool skipped = eventData.Skip || !Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup).ToBool();
      if (!skipped)
      {
        eventData.Item = itemId.ToNwObject<NwItem>()!;
      }

      ProcessEvent(EventCallbackType.After, eventData);

      return (!skipped).ToInt();
    }

    private static TEvent HandleGroupTargetEvent<TEvent>(NwPlayer dungeonMaster, CNWSMessage message, bool isGroup) where TEvent : DMGroupTargetEvent, new()
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
        targets[i] = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject()!;
        offset += sizeof(uint);
      }

      TEvent eventData = ProcessEvent(EventCallbackType.Before, new TEvent
      {
        DungeonMaster = dungeonMaster,
        Targets = targets,
      });

      return eventData;
    }

    private static OnDMJumpTargetToPoint HandleJumpTargetToPointEvent(NwPlayer dungeonMaster, CNWSMessage message, bool isGroup)
    {
      int offset = 0;
      int groupSize = 1;

      NwArea area = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject<NwArea>()!;
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
        targets[i] = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject<NwGameObject>()!;
        offset += sizeof(uint);
      }

      OnDMJumpTargetToPoint eventData = ProcessEvent(EventCallbackType.Before, new OnDMJumpTargetToPoint
      {
        DungeonMaster = dungeonMaster,
        Targets = targets,
        NewArea = area,
        NewPosition = position,
      });

      return eventData;
    }

    private static OnDMPlayerDMLogin HandlePlayerDMLoginEvent(NwPlayer dungeonMaster, CNWSMessage message)
    {
      string password = message.PeekMessageString(0);

      OnDMPlayerDMLogin eventData = ProcessEvent(EventCallbackType.Before, new OnDMPlayerDMLogin
      {
        DungeonMaster = dungeonMaster,
        Password = password,
      });

      return eventData;
    }

    private static TEvent HandleSingleTargetEvent<TEvent>(NwPlayer dungeonMaster, CNWSMessage message) where TEvent : DMSingleTargetEvent, new()
    {
      NwObject target = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwObject()!;
      TEvent eventData = ProcessEvent(EventCallbackType.Before, new TEvent
      {
        DungeonMaster = dungeonMaster,
        Target = target,
      });

      return eventData;
    }

    private static int HandleSpawnEvent(void* pMessage, void* pPlayer, byte nMinor, int bGroup, NwPlayer dungeonMaster, CNWSMessage message, MessageDungeonMasterMinor spawnType)
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

      NwArea area = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject<NwArea>()!;
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

      OnDMSpawnObject eventData = ProcessEvent(EventCallbackType.Before, new OnDMSpawnObject
      {
        DungeonMaster = dungeonMaster,
        Area = area,
        Position = new Vector3(x, y, z),
        ResRef = resRef,
        ObjectType = objectType,
      });

      bool skipped = eventData.Skip || !Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup).ToBool();
      if (!skipped)
      {
        eventData.SpawnedObject = gameObjectId.ToNwObject<NwGameObject>();
      }

      ProcessEvent(EventCallbackType.After, eventData);

      return (!skipped).ToInt();
    }

    private static OnDMSpawnTrapOnObject HandleSpawnTrapOnObjectEvent(NwPlayer dungeonMaster, CNWSMessage message)
    {
      NwStationary target = (message.PeekMessage<uint>(4) & 0x7FFFFFFF).ToNwObject<NwStationary>()!;
      OnDMSpawnTrapOnObject eventData = ProcessEvent(EventCallbackType.Before, new OnDMSpawnTrapOnObject
      {
        DungeonMaster = dungeonMaster,
        Target = target,
      });

      return eventData;
    }

    private static TEvent HandleStandardEvent<TEvent>(NwPlayer dungeonMaster) where TEvent : DMEvent, new()
    {
      TEvent eventData = ProcessEvent(EventCallbackType.Before, new TEvent
      {
        DungeonMaster = dungeonMaster,
      });

      return eventData;
    }

    private static TEvent HandleTeleportEvent<TEvent>(NwPlayer dungeonMaster, CNWSMessage message) where TEvent : DMTeleportEvent, new()
    {
      int offset = 0;

      NwArea area = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject<NwArea>()!;
      offset += sizeof(uint);
      float x = message.PeekMessage<float>(offset);
      offset += sizeof(float);
      float y = message.PeekMessage<float>(offset);
      offset += sizeof(float);
      float z = message.PeekMessage<float>(offset);

      TEvent eventData = ProcessEvent(EventCallbackType.Before, new TEvent
      {
        DungeonMaster = dungeonMaster,
        TargetArea = area,
        TargetPosition = new Vector3(x, y, z),
      });

      return eventData;
    }

    private static OnDMViewInventory HandleViewInventoryEvent(NwPlayer dungeonMaster, CNWSMessage message)
    {
      bool isOpening = message.PeekMessage<int>(0).ToBool();
      NwGameObject target = (message.PeekMessage<uint>(4) & 0x7FFFFFFF).ToNwObject<NwGameObject>()!;

      OnDMViewInventory eventData = ProcessEvent(EventCallbackType.Before, new OnDMViewInventory
      {
        DungeonMaster = dungeonMaster,
        Target = target,
        IsOpening = isOpening,
      });

      return eventData;
    }

    [UnmanagedCallersOnly]
    private static int OnHandleDMMessage(void* pMessage, void* pPlayer, byte nMinor, int bGroup)
    {
      NwPlayer? dungeonMaster = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer();
      CNWSMessage message = CNWSMessage.FromPointer(pMessage);

      if (dungeonMaster == null || message == null)
      {
        return Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup);
      }

      try
      {
        DMEvent? eventData;
        switch ((MessageDungeonMasterMinor)nMinor)
        {
          case MessageDungeonMasterMinor.SpawnCreature:
          case MessageDungeonMasterMinor.SpawnItem:
          case MessageDungeonMasterMinor.SpawnTrigger:
          case MessageDungeonMasterMinor.SpawnWaypoint:
          case MessageDungeonMasterMinor.SpawnEncounter:
          case MessageDungeonMasterMinor.SpawnPortal:
          case MessageDungeonMasterMinor.SpawnPlaceable:
            return HandleSpawnEvent(pMessage, pPlayer, nMinor, bGroup, dungeonMaster, message, (MessageDungeonMasterMinor)nMinor);
          case MessageDungeonMasterMinor.Difficulty:
            eventData = HandleChangeDifficultyEvent(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.ViewInventory:
            eventData = HandleViewInventoryEvent(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.SpawnTrapOnObject:
            eventData = HandleSpawnTrapOnObjectEvent(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.Heal:
            eventData = HandleGroupTargetEvent<OnDMHeal>(dungeonMaster, message, bGroup.ToBool());
            break;
          case MessageDungeonMasterMinor.Kill:
            eventData = HandleGroupTargetEvent<OnDMKill>(dungeonMaster, message, bGroup.ToBool());
            break;
          case MessageDungeonMasterMinor.Invulnerable:
            eventData = HandleGroupTargetEvent<OnDMToggleInvulnerable>(dungeonMaster, message, bGroup.ToBool());
            break;
          case MessageDungeonMasterMinor.Rest:
            eventData = HandleGroupTargetEvent<OnDMForceRest>(dungeonMaster, message, bGroup.ToBool());
            break;
          case MessageDungeonMasterMinor.Limbo:
            eventData = HandleGroupTargetEvent<OnDMLimbo>(dungeonMaster, message, bGroup.ToBool());
            break;
          case MessageDungeonMasterMinor.ToggleAI:
            eventData = HandleGroupTargetEvent<OnDMToggleAI>(dungeonMaster, message, bGroup.ToBool());
            break;
          case MessageDungeonMasterMinor.Immortal:
            eventData = HandleGroupTargetEvent<OnDMToggleImmortal>(dungeonMaster, message, bGroup.ToBool());
            break;
          case MessageDungeonMasterMinor.Goto:
            eventData = HandleSingleTargetEvent<OnDMGoTo>(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.Possess:
            eventData = HandleSingleTargetEvent<OnDMPossess>(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.Impersonate:
            eventData = HandleSingleTargetEvent<OnDMPossessFullPower>(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.ToggleLock:
            eventData = HandleSingleTargetEvent<OnDMToggleLock>(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.DisableTrap:
            eventData = HandleSingleTargetEvent<OnDMDisableTrap>(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.Manifest:
            eventData = HandleStandardEvent<OnDMAppear>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.Unmanifest:
            eventData = HandleStandardEvent<OnDMDisappear>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.SetFaction:
          case MessageDungeonMasterMinor.SetFactionByName:
            eventData = HandleStandardEvent<OnDMSetFaction>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.TakeItem:
            eventData = HandleStandardEvent<OnDMTakeItem>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.SetStat:
            eventData = HandleStandardEvent<OnDMSetStat>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.GetVar:
            eventData = HandleStandardEvent<OnDMGetVariable>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.SetVar:
            eventData = HandleStandardEvent<OnDMSetVariable>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.SetTime:
            eventData = HandleStandardEvent<OnDMSetTime>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.SetDate:
            eventData = HandleStandardEvent<OnDMSetDate>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.SetFactionReputation:
            eventData = HandleStandardEvent<OnDMSetFactionReputation>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.GetFactionReputation:
            eventData = HandleStandardEvent<OnDMGetFactionReputation>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.Logout:
            eventData = HandleStandardEvent<OnDMPlayerDMLogout>(dungeonMaster);
            break;
          case MessageDungeonMasterMinor.GotoPoint:
            eventData = HandleTeleportEvent<OnDMJumpToPoint>(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.GotoPointAllPlayers:
            eventData = HandleTeleportEvent<OnDMJumpAllPlayersToPoint>(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.GotoPointTarget:
            eventData = HandleJumpTargetToPointEvent(dungeonMaster, message, bGroup.ToBool());
            break;
          case MessageDungeonMasterMinor.DumpLocals:
            eventData = HandleDumpLocalsEvent(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.GiveGoodAlignment:
          case MessageDungeonMasterMinor.GiveEvilAlignment:
          case MessageDungeonMasterMinor.GiveLawfulAlignment:
          case MessageDungeonMasterMinor.GiveChaoticAlignment:
            eventData = HandleGiveAlignmentEvent(dungeonMaster, message, (MessageDungeonMasterMinor)nMinor);
            break;
          case MessageDungeonMasterMinor.GiveXP:
            eventData = HandleGiveEvent<OnDMGiveXP>(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.GiveLevel:
            eventData = HandleGiveEvent<OnDMGiveLevel>(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.GiveGold:
            eventData = HandleGiveEvent<OnDMGiveGold>(dungeonMaster, message);
            break;
          case MessageDungeonMasterMinor.GiveItem:
            return HandleGiveItemEvent(pMessage, pPlayer, nMinor, bGroup, dungeonMaster, message);
          case MessageDungeonMasterMinor.Login:
            eventData = HandlePlayerDMLoginEvent(dungeonMaster, message);
            break;
          default:
            return Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup);
        }

        int retVal = !eventData.Skip ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
        ProcessEvent(EventCallbackType.After, eventData);

        return retVal;
      }
      catch (Exception e)
      {
        Log.Error(e);
      }

      return Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup);
    }
  }
}
