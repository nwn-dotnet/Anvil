using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public static class ClientEvents
  {
    public sealed class OnClientDisconnect : IEvent
    {
      /// <summary>
      /// Gets the player that disconnected.
      /// </summary>
      public NwPlayer Player { get; init; }

      NwObject IEvent.Context => Player;

      internal delegate void RemovePCFromWorldHook(IntPtr pServerExoAppInternal, IntPtr pPlayer);

      internal class Factory : NativeEventFactory<RemovePCFromWorldHook>
      {
        public Factory(HookService hookService) : base(hookService) {}

        protected override HookInfo HookInfo { get; } = NWNXLib.Functions._ZN21CServerExoAppInternal17RemovePCFromWorldEP10CNWSPlayer
          .HookWithOrder(HookOrder.Earliest);

        protected override RemovePCFromWorldHook Handler => OnRemovePCFromWorld;

        private void OnRemovePCFromWorld(IntPtr pServerExoAppInternal, IntPtr pPlayer)
        {
          ProcessEvent(new OnClientDisconnect
          {
            Player = new CNWSPlayer(pPlayer, false).m_oidPCObject.ToNwObject<NwPlayer>()
          });

          Hook.Original.Invoke(pServerExoAppInternal, pPlayer);
        }
      }
    }

    public sealed class OnServerCharacterSave : IEvent
    {
      /// <summary>
      /// Gets the player that is being saved.
      /// </summary>
      public NwPlayer Player { get; init; }

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;

      internal delegate int SaveServerCharacterHook(IntPtr pPlayer, int bBackupPlayer);

      internal class Factory : NativeEventFactory<SaveServerCharacterHook>
      {
        public Factory(HookService hookService) : base(hookService) {}

        protected override HookInfo HookInfo { get; } = NWNXLib.Functions._ZN10CNWSPlayer19SaveServerCharacterEi
          .HookWithOrder(HookOrder.Early);

        protected override SaveServerCharacterHook Handler => OnSaveServerCharacter;

        private int OnSaveServerCharacter(IntPtr pPlayer, int bBackupPlayer)
        {
          OnServerCharacterSave evt = ProcessEvent(new OnServerCharacterSave
          {
            Player = new CNWSPlayer(pPlayer, false).m_oidPCObject.ToNwObject<NwPlayer>()
          });

          if (!evt.Skip)
          {
            return Hook.Original.Invoke(pPlayer, bBackupPlayer);
          }

          return 0;
        }
      }
    }
  }
}
