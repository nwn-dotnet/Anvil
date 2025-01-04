using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.Native;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  public sealed unsafe class DebugEventFactory : HookEventFactory
  {
    private static FunctionHook<Functions.CNWSMessage.HandlePlayerToServerCheatMessage> Hook { get; set; } = null!;

    protected override IDisposable[] RequestHooks()
    {
      delegate* unmanaged<void*, void*, byte, int> pHook = &OnHandlePlayerToServerCheatMessage;
      Hook = HookService.RequestHook<Functions.CNWSMessage.HandlePlayerToServerCheatMessage>(pHook, HookOrder.Early);
      return [Hook];
    }

    [UnmanagedCallersOnly]
    private static int OnHandlePlayerToServerCheatMessage(void* pMessage, void* pPlayer, byte nMinor)
    {
      CNWSMessage message = CNWSMessage.FromPointer(pMessage);
      if (message == null)
      {
        return Hook.CallOriginal(pMessage, pPlayer, nMinor);
      }

      NwPlayer? player = CNWSPlayer.FromPointer(pPlayer).ToNwPlayer();
      int retVal;

      switch ((MessageCheatMinor)nMinor)
      {
        case MessageCheatMinor.RunScript:
          OnDebugRunScript runScriptEventData = HandleRunScriptEvent(message, player);
          retVal = !runScriptEventData.Skip ? Hook.CallOriginal(pMessage, pPlayer, nMinor) : false.ToInt();
          ProcessEvent(EventCallbackType.After, runScriptEventData);
          return retVal;
        case MessageCheatMinor.RunScriptChunk:
          OnDebugRunScriptChunk runScriptChunkEventData = HandleRunScriptChunkEvent(message, player);
          retVal = !runScriptChunkEventData.Skip ? Hook.CallOriginal(pMessage, pPlayer, nMinor) : false.ToInt();
          ProcessEvent(EventCallbackType.After, runScriptChunkEventData);
          return retVal;
        case MessageCheatMinor.PlayVisualEffect:
          OnDebugPlayVisualEffect playVisualEffectEventData = HandleVisualEffectEvent(message, player);
          retVal = !playVisualEffectEventData.Skip ? Hook.CallOriginal(pMessage, pPlayer, nMinor) : false.ToInt();
          ProcessEvent(EventCallbackType.After, playVisualEffectEventData);
          return retVal;
        default:
          return Hook.CallOriginal(pMessage, pPlayer, nMinor);
      }
    }

    private static OnDebugRunScript HandleRunScriptEvent(CNWSMessage message, NwPlayer? player)
    {
      int offset = 0;
      string scriptName = message.PeekMessageString(offset);
      offset += scriptName.Length + 4;

      uint oidTarget = player?.Player.SatisfiesBuild(8193, 14, 0).ToBool() == true ? message.PeekMessage<uint>(offset) : NwObject.Invalid;
      if (oidTarget == NwObject.Invalid)
      {
        oidTarget = player?.ControlledCreature;
      }

      OnDebugRunScript eventData = ProcessEvent(EventCallbackType.Before, new OnDebugRunScript
      {
        Player = player,
        ScriptName = scriptName,
        Target = (oidTarget & 0x7FFFFFFF).ToNwObject(),
      });

      return eventData;
    }

    private static OnDebugRunScriptChunk HandleRunScriptChunkEvent(CNWSMessage message, NwPlayer? player)
    {
      int offset = 0;
      string scriptChunk = message.PeekMessageString(offset);
      offset += scriptChunk.Length + 4;

      uint oidTarget = message.PeekMessage<uint>(offset);
      if (oidTarget == NwObject.Invalid)
      {
        oidTarget = player?.ControlledCreature;
      }

      offset += sizeof(uint);
      bool wrapIntoMain = (message.PeekMessage<int>(offset) & 0x10).ToBool();

      OnDebugRunScriptChunk eventData = ProcessEvent(EventCallbackType.Before, new OnDebugRunScriptChunk
      {
        Player = player,
        ScriptChunk = scriptChunk,
        Target = (oidTarget & 0x7FFFFFFF).ToNwObject(),
        WrapIntoMain = wrapIntoMain,
      });

      return eventData;
    }

    private static OnDebugPlayVisualEffect HandleVisualEffectEvent(CNWSMessage message, NwPlayer? player)
    {
      int offset = 0;

      uint target = message.PeekMessage<uint>(offset) & 0x7FFFFFFF;
      offset += sizeof(uint);

      ushort visualEffect = message.PeekMessage<ushort>(offset);
      offset += sizeof(ushort);

      float duration = message.PeekMessage<float>(offset);
      offset += sizeof(float);

      Vector3 position = message.PeekMessage<Vector3>(offset);

      OnDebugPlayVisualEffect eventData = ProcessEvent(EventCallbackType.Before, new OnDebugPlayVisualEffect
      {
        Player = player,
        TargetObject = target.ToNwObject(),
        Effect = NwGameTables.VisualEffectTable.ElementAtOrDefault(visualEffect),
        Duration = TimeSpan.FromSeconds(duration),
        TargetPosition = position,
      });

      return eventData;
    }
  }
}
