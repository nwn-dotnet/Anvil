using NWN.API;

namespace NWNX.API.Events
{
  /// <summary>
  /// Events called by the Enforce Legal Character system.<br/>
  /// The ELC Plugin (NWNX_ELC) must be loaded for these events to be fired.
  /// </summary>
  public static class ELCEvents
  {
    [NWNXEvent("NWNX_ON_ELC_VALIDATE_CHARACTER_BEFORE")]
    public class OnValidateBefore : NWNXEventSkippable<OnValidateBefore>
    {
      /// <summary>
      /// Gets the player being validated.
      /// </summary>
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
      }
    }

    /// <summary>
    /// Called if the character successfully completes validation.
    /// </summary>
    [NWNXEvent("NWNX_ON_ELC_VALIDATE_CHARACTER_AFTER")]
    public class OnValidateAfter : NWNXEventSkippable<OnValidateAfter>
    {
      /// <summary>
      /// Gets the player being validated.
      /// </summary>
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
      }
    }
  }
}
