using NWN.API;
using NWN.API.Events;
using NWN.Core;

namespace NWNX.API.Events
{
  public static class LevelEvents
  {
    [NWNXEvent("NWNX_ON_LEVEL_UP_BEFORE")]
    public sealed class OnLevelUpBefore : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_LEVEL_UP_AFTER")]
    public sealed class OnLevelUpAfter : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_LEVEL_UP_AUTOMATIC_BEFORE")]
    public sealed class OnLevelUpAutomaticBefore : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_LEVEL_UP_AUTOMATIC_AFTER")]
    public sealed class OnLevelUpAutomaticAfter : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_LEVEL_DOWN_BEFORE")]
    public sealed class OnLevelDownBefore : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_LEVEL_DOWN_AFTER")]
    public sealed class OnLevelDownAfter : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }
  }
}
