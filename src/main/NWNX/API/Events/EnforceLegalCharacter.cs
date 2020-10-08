using NWN.API;

namespace NWNX.API.Events
{
  /// <summary>
  /// Events called by elc.
  /// ELC nwnxee plugin must be enabled.
  /// </summary>
  public static class EnforceLegalCharacterEvents
  {
    [NWNXEvent("NWNX_ON_ELC_VALIDATE_CHARACTER_BEFORE")]
    public class OnEnforceLegalCharacterValidateBefore : EventSkippable<OnEnforceLegalCharacterValidateBefore>
    {
      /// <summary>
      /// Gets the player.
      /// </summary>
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
      }
    }

    [NWNXEvent("NWNX_ON_ELC_VALIDATE_CHARACTER_AFTER")]
    public class OnEnforceLegalCharacterValidateAfter : EventSkippable<OnEnforceLegalCharacterValidateAfter>
    {
      /// <summary>
      /// Gets the player.
      /// </summary>
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
      }
    }
  }
}
