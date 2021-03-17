using System;
using NWN.Native.API;
using NWN.Services;

namespace NWN.API.Events
{
  public static class ClientEvents
  {
    public sealed record OnClientDisconnect : IEvent
    {
      /// <summary>
      /// Gets the player that disconnected.
      /// </summary>
      public NwPlayer Player { get; init; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      internal delegate void RemovePCFromWorldHook(IntPtr pServerExoAppInternal, IntPtr pPlayer);

      [ServiceBinding(typeof(IEventFactory))]
      internal class Factory : NativeEventFactory<OnClientDisconnect, RemovePCFromWorldHook>
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

    public sealed record OnServerCharacterSave : IEvent
    {
      /// <summary>
      /// Gets the player that is being saved.
      /// </summary>
      public NwPlayer Player { get; init; }

      public bool Skip { get; set; }

      bool IEvent.HasContext => true;

      NwObject IEvent.Context => Player;

      internal delegate int SaveServerCharacterHook(IntPtr pPlayer, int bBackupPlayer);

      [ServiceBinding(typeof(IEventFactory))]
      internal class Factory : NativeEventFactory<OnServerCharacterSave, SaveServerCharacterHook>
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
