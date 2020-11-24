using NWN.API;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class DMEvents
  {
    [NWNXEvent("NWNX_ON_DM_PLAYERDM_LOGIN_BEFORE")]
    public class OnPlayerDMLoginBefore : NWNXEventSkippable<OnPlayerDMLoginBefore>
    {
      /// <summary>
      /// The player attempting to login as a DM.
      /// </summary>
      public NwPlayer Player { get; private set; }

      /// <summary>
      /// The password specified in this login attempt.
      /// </summary>
      public string Password { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
        Password = EventsPlugin.GetEventData("PASSWORD");
      }
    }

    [NWNXEvent("NWNX_ON_DM_PLAYERDM_LOGIN_AFTER")]
    public class OnPlayerDMLoginAfter : NWNXEventSkippable<OnPlayerDMLoginAfter>
    {
      /// <summary>
      /// The player/DM attempting to login as a DM.
      /// </summary>
      public NwPlayer Player { get; private set; }

      /// <summary>
      /// The password specified in this login attempt.
      /// </summary>
      public string Password { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
        Password = EventsPlugin.GetEventData("PASSWORD");
      }
    }

    [NWNXEvent("NWNX_ON_DM_PLAYERDM_LOGOUT_BEFORE")]
    public class OnPlayerDMLogoutBefore : NWNXEventSkippable<OnPlayerDMLogoutBefore>
    {
      /// <summary>
      /// The player/DM attempting to logout.
      /// </summary>
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
      }
    }

    [NWNXEvent("NWNX_ON_DM_PLAYERDM_LOGOUT_AFTER")]
    public class OnPlayerDMLogoutAfter : NWNXEventSkippable<OnPlayerDMLogoutAfter>
    {
      /// <summary>
      /// The player/DM attempting to logout.
      /// </summary>
      public NwPlayer Player { get; private set; }

      protected override void PrepareEvent(NwObject objSelf)
      {
        Player = (NwPlayer)objSelf;
      }
    }
  }
}
