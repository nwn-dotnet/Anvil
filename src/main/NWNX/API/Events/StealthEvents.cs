using NWN.API;
using NWN.API.Events;

namespace NWNX.API.Events
{
  public class StealthEvents
  {
    [NWNXEvent("NWNX_ON_ENTER_STEALTH_BEFORE")]
    public class OnEnterStealthBefore : Event<OnEnterStealthBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) =>
        Player = (NwPlayer)objSelf;
    }

    [NWNXEvent("NWNX_ON_ENTER_STEALTH_AFTER")]
    public class OnEnterStealthAfter : Event<OnEnterStealthAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) =>
        Player = (NwPlayer)objSelf;
    }

    [NWNXEvent("NWNX_ON_EXIT_STEALTH_BEFORE")]
    public class OnExitStealthBefore : Event<OnExitStealthBefore>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) =>
        Player = (NwPlayer)objSelf;
    }

    [NWNXEvent("NWNX_ON_EXIT_STEALTH_AFTER")]
    public class OnExitStealthAfter : Event<OnExitStealthAfter>
    {
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf) =>
        Player = (NwPlayer)objSelf;
    }
  }
}
