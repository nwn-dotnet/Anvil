using System;
using NWN.API;
using NWN.API.Events;
using NWN.Core;

namespace NWNX.API.Events
{
  public static class StealthEvents
  {
    [NWNXEvent("NWNX_ON_ENTER_STEALTH_BEFORE")]
    [Obsolete("Use NwModule/NwCreature.OnStealthModeUpdate instead.")]
    public sealed class OnEnterStealthBefore : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_ENTER_STEALTH_AFTER")]
    [Obsolete("Use NwModule/NwCreature.OnStealthModeUpdate instead.")]
    public sealed class OnEnterStealthAfter : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_EXIT_STEALTH_BEFORE")]
    [Obsolete("Use NwModule/NwCreature.OnStealthModeUpdate instead.")]
    public sealed class OnExitStealthBefore : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }

    [NWNXEvent("NWNX_ON_EXIT_STEALTH_AFTER")]
    [Obsolete("Use NwModule/NwCreature.OnStealthModeUpdate instead.")]
    public sealed class OnExitStealthAfter : IEventSkippable
    {
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwObject<NwPlayer>();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player;
    }
  }
}
