using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API.Events
{
  internal sealed unsafe class DebugEventFactory : HookEventFactory
  {
    private static FunctionHook<HandlePlayerToServerCheatMessageHook> Hook { get; set; } = null!;

    private delegate int HandlePlayerToServerCheatMessageHook(void* pMessage, void* pPlayer, byte nMinor);

    protected override IDisposable[] RequestHooks()
    {
      delegate* unmanaged<void*, void*, byte, int> pHook = &OnHandlePlayerToServerCheatMessage;
      Hook = HookService.RequestHook<HandlePlayerToServerCheatMessageHook>(pHook, FunctionsLinux._ZN11CNWSMessage32HandlePlayerToServerCheatMessageEP10CNWSPlayerh, HookOrder.Early);
      return new IDisposable[] { Hook };
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
      switch ((MessageCheatMinor)nMinor)
      {
        case MessageCheatMinor.RunScript:
          return HandleRunScriptEvent(message, player) ? Hook.CallOriginal(pMessage, pPlayer, nMinor) : false.ToInt();
        case MessageCheatMinor.RunScriptChunk:
          return HandleRunScriptChunkEvent(message, player) ? Hook.CallOriginal(pMessage, pPlayer, nMinor) : false.ToInt();
        case MessageCheatMinor.PlayVisualEffect:
          return HandleVisualEffectEvent(message, player) ? Hook.CallOriginal(pMessage, pPlayer, nMinor) : false.ToInt();
        default:
          return Hook.CallOriginal(pMessage, pPlayer, nMinor);
      }
    }

    private static bool HandleRunScriptEvent(CNWSMessage message, NwPlayer? player)
    {
      int offset = 0;
      string scriptName = message.PeekMessageString(offset);
      offset += scriptName.Length + 4;

      uint oidTarget = player?.Player.SatisfiesBuild(8193, 14).ToBool() == true ? message.PeekMessage<uint>(offset) : NwObject.Invalid;
      if (oidTarget == NwObject.Invalid)
      {
        oidTarget = player?.ControlledCreature;
      }

      OnDebugRunScript eventData = new OnDebugRunScript
      {
        ScriptName = scriptName,
        Target = (oidTarget & 0x7FFFFFFF).ToNwObject(),
      };

      return !eventData.Skip;
    }

    private static bool HandleRunScriptChunkEvent(CNWSMessage message, NwPlayer? player)
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

      OnDebugRunScriptChunk eventData = ProcessEvent(new OnDebugRunScriptChunk
      {
        ScriptChunk = scriptChunk,
        Target = (oidTarget & 0x7FFFFFFF).ToNwObject(),
        WrapIntoMain = wrapIntoMain,
      });

      return !eventData.Skip;
    }

    private static bool HandleVisualEffectEvent(CNWSMessage message, NwPlayer? player)
    {
      int offset = 0;

      uint target = (message.PeekMessage<uint>(offset) & 0x7FFFFFFF);
      offset += sizeof(uint);

      ushort visualEffect = message.PeekMessage<ushort>(offset);
      offset += sizeof(ushort);

      float duration = message.PeekMessage<float>(offset);
      offset += sizeof(float);

      Vector3 position = message.PeekMessage<Vector3>(offset);

      OnDebugPlayVisualEffect eventData = new OnDebugPlayVisualEffect
      {
        TargetObject = target.ToNwObject(),
        Effect = NwGameTables.VisualEffectTable[visualEffect],
        Duration = TimeSpan.FromSeconds(duration),
        TargetPosition = position,
      };

      return !eventData.Skip;
    }
  }
}
