using System.Runtime.InteropServices;
using Anvil.Internal;
using NWN.API.Constants;
using NWN.Native.API;
using NWN.Services;

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
          return HandleSpawnEvent(dungeonMaster) ? Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup) : false.ToInt();
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
        case MessageDungeonMasterMinor.RequestObjectList:
          // TODO
        case MessageDungeonMasterMinor.ObjectList:
          // TODO
        case MessageDungeonMasterMinor.SearchByTag:
          // TODO
        case MessageDungeonMasterMinor.SearchByTagResult:
          // TODO
        case MessageDungeonMasterMinor.SearchByTagResultNone:
          // TODO
        case MessageDungeonMasterMinor.CreatorList:
          // TODO
        case MessageDungeonMasterMinor.AreaList:
          // TODO
        case MessageDungeonMasterMinor.AreaListSorted:
          // TODO
        case MessageDungeonMasterMinor.PartyList:
          // TODO
        case MessageDungeonMasterMinor.Login:
          // TODO
        case MessageDungeonMasterMinor.Logout:
          // TODO
        case MessageDungeonMasterMinor.LoginState:
          // TODO
        case MessageDungeonMasterMinor.SearchNext:
          // TODO
        case MessageDungeonMasterMinor.SearchById:
          // TODO
        case MessageDungeonMasterMinor.Duplicate:
          // TODO
        case MessageDungeonMasterMinor.TriggerEntered:
          // TODO
        case MessageDungeonMasterMinor.TriggerExit:
          // TODO
        case MessageDungeonMasterMinor.Manifest:
          // TODO
        case MessageDungeonMasterMinor.Unmanifest:
          // TODO
        case MessageDungeonMasterMinor.GotoPoint:
          // TODO
        case MessageDungeonMasterMinor.GiveXP:
          // TODO
        case MessageDungeonMasterMinor.GiveLevel:
          // TODO
        case MessageDungeonMasterMinor.GiveGold:
          // TODO
        case MessageDungeonMasterMinor.SetFaction:
          // TODO
        case MessageDungeonMasterMinor.SetFactionByName:
          // TODO
        case MessageDungeonMasterMinor.GiveItem:
          // TODO
        case MessageDungeonMasterMinor.TakeItem:
          // TODO
        case MessageDungeonMasterMinor.GotoPointTarget:
          // TODO
        case MessageDungeonMasterMinor.GotoPointAllPlayers:
          // TODO
        case MessageDungeonMasterMinor.SetStat:
          // TODO
        case MessageDungeonMasterMinor.GetVar:
          // TODO
        case MessageDungeonMasterMinor.SetVar:
          // TODO
        case MessageDungeonMasterMinor.SetTime:
          // TODO
        case MessageDungeonMasterMinor.SetDate:
          // TODO
        case MessageDungeonMasterMinor.SetFactionReputation:
          // TODO
        case MessageDungeonMasterMinor.GetFactionReputation:
          // TODO
        case MessageDungeonMasterMinor.DumpLocals:
          // TODO
        case MessageDungeonMasterMinor.GiveGoodAlignment:
          // TODO
        case MessageDungeonMasterMinor.GiveEvilAlignment:
          // TODO
        case MessageDungeonMasterMinor.GiveLawfulAlignment:
          // TODO
        case MessageDungeonMasterMinor.GiveChaoticAlignment:
          // TODO
          return Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup);
        default:
          return Hook.CallOriginal(pMessage, pPlayer, nMinor, bGroup);
      }
    }

    private static bool HandleSingleTargetEvent<TEvent>(NwPlayer dungeonMaster, CNWSMessage message) where TEvent : IEvent, IDMSingleTargetEvent, new()
    {
      NwGameObject target = (message.PeekMessage<uint>(0) & 0x7FFFFFFF).ToNwObject<NwGameObject>();
      TEvent eventData = ProcessEvent(new TEvent
      {
        DungeonMaster = dungeonMaster,
        Target = target,
      });

      return !eventData.Skip;
    }

    private static bool HandleGroupTargetEvent<TEvent>(NwPlayer dungeonMaster, CNWSMessage message, bool isGroup) where TEvent : IEvent, IDMGroupTargetEvent, new()
    {
      int offset = 0;
      int groupSize = 1;

      if (isGroup)
      {
        groupSize = message.PeekMessage<int>(offset);
        offset += sizeof(int);
      }

      NwGameObject[] targets = new NwGameObject[groupSize];
      for (int i = 0; i < groupSize; i++)
      {
        targets[i] = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF).ToNwObject<NwGameObject>();
        offset += sizeof(uint);
      }

      TEvent eventData = ProcessEvent(new TEvent
      {
        DungeonMaster = dungeonMaster,
        Targets = targets,
      });

      return !eventData.Skip;
    }

    private static bool HandleSpawnEvent(NwPlayer dungeonMaster)
    {
      NwGameObject gameObject = LowLevel.ServerExoApp.GetObjectArray().m_nNextObjectArrayID[0].ToNwObject<NwGameObject>();
      OnDMSpawnObject eventData = ProcessEvent(new OnDMSpawnObject
      {
        DungeonMaster = dungeonMaster,
        SpawnedObject = gameObject,
      });

      return !eventData.Skip;
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
  }
}
