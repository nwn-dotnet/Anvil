using NWN.API;

namespace NWNX.API.Events
{
  public class LevelEvents
  {
    [NWNXEvent("NWNX_ON_LEVEL_UP_BEFORE")]
    public class OnLevelUpBefore : EventSkippable<OnLevelUpBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) =>
        Player = (NwPlayer)objSelf;

    }

    [NWNXEvent("NWNX_ON_LEVEL_UP_AFTER")]
    public class OnLevelUpAfter : EventSkippable<OnLevelUpAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) =>
        Player = (NwPlayer)objSelf;

    }

    [NWNXEvent("NWNX_ON_LEVEL_UP_AUTOMATIC_BEFORE")]
    public class OnLevelUpAutomaticBefore : EventSkippable<OnLevelUpAutomaticBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) =>
        Player = (NwPlayer)objSelf;

    }

    [NWNXEvent("NWNX_ON_LEVEL_UP_AUTOMATIC_AFTER")]
    public class OnLevelUpAutomaticAfter : EventSkippable<OnLevelUpAutomaticAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) =>
        Player = (NwPlayer)objSelf;

    }

    [NWNXEvent("NWNX_ON_LEVEL_DOWN_BEFORE")]
    public class OnLevelDownBefore : EventSkippable<OnLevelDownBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) =>
        Player = (NwPlayer)objSelf;

    }

    [NWNXEvent("NWNX_ON_LEVEL_DOWN_AFTER")]
    public class OnLevelDownAfter : EventSkippable<OnLevelDownAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) =>
        Player = (NwPlayer)objSelf;

    }

  }
}
