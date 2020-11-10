using NWN.API;

namespace NWNX.API.Events
{
  public class LevelEvents
  {
    [NWNXEvent("NWNX_ON_LEVEL_UP_BEFORE")]
    public class OnLevelUpBefore : NWNXEventSkippable<OnLevelUpBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }

    [NWNXEvent("NWNX_ON_LEVEL_UP_AFTER")]
    public class OnLevelUpAfter : NWNXEventSkippable<OnLevelUpAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }

    [NWNXEvent("NWNX_ON_LEVEL_UP_AUTOMATIC_BEFORE")]
    public class OnLevelUpAutomaticBefore : NWNXEventSkippable<OnLevelUpAutomaticBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }

    [NWNXEvent("NWNX_ON_LEVEL_UP_AUTOMATIC_AFTER")]
    public class OnLevelUpAutomaticAfter : NWNXEventSkippable<OnLevelUpAutomaticAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }

    [NWNXEvent("NWNX_ON_LEVEL_DOWN_BEFORE")]
    public class OnLevelDownBefore : NWNXEventSkippable<OnLevelDownBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }

    [NWNXEvent("NWNX_ON_LEVEL_DOWN_AFTER")]
    public class OnLevelDownAfter : NWNXEventSkippable<OnLevelDownAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }
  }
}
