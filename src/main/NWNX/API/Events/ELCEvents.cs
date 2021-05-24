using NWN.API;
using NWN.API.Events;
using NWN.Core;

namespace NWNX.API.Events
{
  /// <summary>
  /// Events called by the Enforce Legal Character system.<br/>
  /// The ELC Plugin (NWNX_ELC) must be loaded for these events to be fired.
  /// </summary>
  public static class ELCEvents
  {
    [NWNXEvent("NWNX_ON_ELC_VALIDATE_CHARACTER_BEFORE")]
    public sealed class OnValidateBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the player being validated.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwPlayer();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player.ControlledCreature;
    }

    /// <summary>
    /// Called if the character successfully completes validation.
    /// </summary>
    [NWNXEvent("NWNX_ON_ELC_VALIDATE_CHARACTER_AFTER")]
    public sealed class OnValidateAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the player being validated.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwPlayer();

      public bool Skip { get; set; }

      NwObject IEvent.Context => Player.ControlledCreature;
    }
  }
}
