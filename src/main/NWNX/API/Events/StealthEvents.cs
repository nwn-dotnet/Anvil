using NWN.API;

namespace NWNX.API.Events
{
  public static class StealthEvents
  {
    [NWNXEvent("NWNX_ON_ENTER_STEALTH_BEFORE")]
    public class OnEnterStealthBefore : NWNXEvent<OnEnterStealthBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }

    [NWNXEvent("NWNX_ON_ENTER_STEALTH_AFTER")]
    public class OnEnterStealthAfter : NWNXEvent<OnEnterStealthAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }

    [NWNXEvent("NWNX_ON_EXIT_STEALTH_BEFORE")]
    public class OnExitStealthBefore : NWNXEvent<OnExitStealthBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }

    [NWNXEvent("NWNX_ON_EXIT_STEALTH_AFTER")]
    public class OnExitStealthAfter : NWNXEvent<OnExitStealthAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
        => Player = (NwPlayer) objSelf;
    }
  }
}
