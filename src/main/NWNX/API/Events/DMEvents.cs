using NWN.API;
using NWN.API.Events;
using NWN.Core;
using NWN.Core.NWNX;

namespace NWNX.API.Events
{
  public static class DMEvents
  {
    [NWNXEvent("NWNX_ON_DM_PLAYERDM_LOGIN_BEFORE")]
    public sealed class OnPlayerDMLoginBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the player attempting to login as a DM.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwPlayer();

      /// <summary>
      /// Gets the password specified in this login attempt.
      /// </summary>
      public string Password { get; } = EventsPlugin.GetEventData("PASSWORD");

      public bool Skip { get; set; }

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }

    [NWNXEvent("NWNX_ON_DM_PLAYERDM_LOGIN_AFTER")]
    public sealed class OnPlayerDMLoginAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the player/DM attempting to login as a DM.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwPlayer();

      /// <summary>
      /// Gets the password specified in this login attempt.
      /// </summary>
      public string Password { get; } = EventsPlugin.GetEventData("PASSWORD");

      public bool Skip { get; set; }

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }

    [NWNXEvent("NWNX_ON_DM_PLAYERDM_LOGOUT_BEFORE")]
    public sealed class OnPlayerDMLogoutBefore : IEventSkippable
    {
      /// <summary>
      /// Gets the player/DM attempting to logout.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwPlayer();

      public bool Skip { get; set; }

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }

    [NWNXEvent("NWNX_ON_DM_PLAYERDM_LOGOUT_AFTER")]
    public sealed class OnPlayerDMLogoutAfter : IEventSkippable
    {
      /// <summary>
      /// Gets the player/DM attempting to logout.
      /// </summary>
      public NwPlayer Player { get; } = NWScript.OBJECT_SELF.ToNwPlayer();

      public bool Skip { get; set; }

      NwObject IEvent.Context
      {
        get => Player.ControlledCreature;
      }
    }
  }
}
