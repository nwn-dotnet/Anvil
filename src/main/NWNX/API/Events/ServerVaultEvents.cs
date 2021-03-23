using System;
using NWN.API;
using NWN.API.Events;
using NWN.Core;

namespace NWNX.API.Events
{
  public static class ServerVaultEvents
  {
    [Obsolete("Use NwModule/NwPlayer.On instead.")]
    [NWNXEvent("NWNX_ON_SERVER_CHARACTER_SAVE_BEFORE")]
    public sealed class OnServerCharacterSaveBefore : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_SERVER_CHARACTER_SAVE_AFTER")]
    public sealed class OnServerCharacterSaveAfter : IEvent
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      NwObject IEvent.Context => Player;
    }
  }
}
