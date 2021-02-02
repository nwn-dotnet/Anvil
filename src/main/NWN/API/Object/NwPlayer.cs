using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public sealed class NwPlayer : NwCreature
  {
    internal NwPlayer(uint objectId) : base(objectId) {}

    /// <summary>
    /// Gets a value indicating whether this Player is a Dungeon Master.
    /// </summary>
    public bool IsDM
    {
      get => NWScript.GetIsDM(this).ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether this player has DM privileges gained through a player login (as opposed to the DM client).
    /// </summary>
    public bool IsPlayerDM
    {
      get => NWScript.GetIsPlayerDM(this).ToBool();
    }

    /// <summary>
    /// Gets the player's login name.
    /// </summary>
    public string PlayerName
    {
      get => NWScript.GetPCPlayerName(this);
    }

    /// <summary>
    /// Gets this player's client version (Major + Minor).
    /// </summary>
    public Version ClientVersion
    {
      get => new Version(NWScript.GetPlayerBuildVersionMajor(this), NWScript.GetPlayerBuildVersionMinor(this));
    }

    /// <summary>
    /// Gets the public part of the CD key that this player used when logging in.
    /// </summary>
    public string CDKey
    {
      get => NWScript.GetPCPublicCDKey(this, true.ToInt());
    }

    /// <summary>
    /// Gets the connecting IP address for this player.
    /// </summary>
    public string IPAddress
    {
      get => NWScript.GetPCIPAddress(this);
    }

    /// <summary>
    /// Gets a value indicating whether this player has connected to the server over a relay (instead of directly).
    /// </summary>
    public bool IsConnectionRelayed
    {
      get => NWScript.GetIsPlayerConnectionRelayed(this).ToBool();
    }

    /// <summary>
    /// Gets or sets the movement rate factor for the cutscene camera following this 'camera man'.
    /// </summary>
    public float CutsceneCameraMoveRate
    {
      get => NWScript.GetCutsceneCameraMoveRate(this);
      set => NWScript.SetCutsceneCameraMoveRate(this, value);
    }

    /// <summary>
    /// Gets a value indicating whether this creature is currently in "Cutscene" mode.
    /// </summary>
    public bool IsInCutsceneMode
    {
      get => NWScript.GetCutsceneMode(this).ToBool();
    }

    /// <summary>
    /// Gets the members in this player's party.
    /// </summary>
    public IEnumerable<NwPlayer> PartyMembers
    {
      get
      {
        for (uint member = NWScript.GetFirstFactionMember(this); member != INVALID; member = NWScript.GetNextFactionMember(this))
        {
          yield return member.ToNwObject<NwPlayer>();
        }
      }
    }

    /// <summary>
    /// Boots this player from the server.
    /// </summary>
    /// <param name="reason">An optional message to show to the player.</param>
    public void BootPlayer(string reason = "")
      => NWScript.BootPC(this, reason);

    /// <summary>
    /// Adds this player to the specified party leader's party.
    /// </summary>
    /// <param name="partyLeader">The party leader of the party to join.</param>
    public void AddToParty(NwPlayer partyLeader)
      => NWScript.AddToParty(this, partyLeader);

    /// <summary>
    /// Removes this player from their current party.
    /// </summary>
    public void RemoveFromCurrentParty()
      => NWScript.RemoveFromParty(this);

    /// <summary>
    /// Attaches the specified creature to this player as a henchmen.
    /// </summary>
    /// <param name="henchmen">The henchmen to attach to this player.</param>
    public void AddHenchmen(NwCreature henchmen)
      => NWScript.AddHenchman(this, henchmen);

    /// <summary>
    /// Adds an entry to this player's journal.
    /// </summary>
    /// <param name="categoryTag">The tag of the Journal category (case-sensitive).</param>
    /// <param name="entryId">The ID of the Journal entry.</param>
    /// <param name="allPartyMembers">If true, this entry is added to all players in this player's party.</param>
    /// <param name="allowOverrideHigher">If true, disables the default restriction that requires journal entry numbers to increase.</param>
    public void AddJournalQuestEntry(string categoryTag, int entryId, bool allPartyMembers = true, bool allowOverrideHigher = false)
      => NWScript.AddJournalQuestEntry(categoryTag, entryId, this, allPartyMembers.ToInt(), false.ToInt(), allowOverrideHigher.ToInt());

    /// <summary>
    /// Instructs this player to open their inventory.
    /// </summary>
    public void OpenInventory()
      => NWScript.OpenInventory(this, this);

    /// <summary>
    /// Opens the specified creatures inventory, and shows it to this player.<br/>
    /// </summary>
    /// <remarks>
    /// DMs can see any player or creature's inventory. Players can only view their own inventory, or that of a henchmen.
    /// </remarks>
    /// <param name="target">The target creature's inventory to view.</param>
    public void OpenInventory(NwCreature target)
      => NWScript.OpenInventory(target, this);

      /// <summary>
    /// Gets the specified campaign variable for this player.
    /// </summary>
    /// <param name="campaign">The name of the campaign.</param>
    /// <param name="name">The variable name.</param>
    /// <typeparam name="T">The variable type.</typeparam>
    /// <returns>A CampaignVariable instance for getting/setting the variable's value.</returns>
    public CampaignVariable<T> GetCampaignVariable<T>(string campaign, string name)
      => CampaignVariable<T>.Create(campaign, name, this);

    /// <summary>
    /// Sends a server message to this player.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="color">A color to apply to the message.</param>
    public void SendServerMessage(string message, Color color)
      => NWScript.SendMessageToPC(this, message.ColorString(color));

    /// <inheritdoc cref="SendServerMessage(string,NWN.API.Color)"/>
    public void SendServerMessage(string message)
      => NWScript.SendMessageToPC(this, message);

    /// <summary>
    /// Sets if this player should like, or unlike the specified player.
    /// </summary>
    /// <param name="like">true if this player should like the target, false if this player should dislike the target.</param>
    /// <param name="target">The target to like/dislike.</param>
    public void SetPCReputation(bool like, NwPlayer target)
    {
      if (like)
      {
        NWScript.SetPCLike(this, target);
      }
      else
      {
        NWScript.SetPCDislike(this, target);
      }
    }

    /// <summary>
    /// Starts a conversation with another object, typically a creature.
    /// </summary>
    /// <param name="converseWith">The target object to converse with.</param>
    /// <param name="dialogResRef">The dialogue to start. If this is unset, the target's own dialogue file will be used.</param>
    /// <param name="isPrivate">Whether this dialogue should be visible to all nearby players, or visible to this player only.</param>
    /// <param name="playHello">Whether the hello/greeting should be played once the dialogue starts.</param>
    public async Task ActionStartConversation(NwGameObject converseWith, string dialogResRef = "", bool isPrivate = false, bool playHello = true)
    {
      await WaitForObjectContext();
      NWScript.ActionStartConversation(converseWith, dialogResRef, isPrivate.ToInt(), playHello.ToInt());
    }

    /// <summary>
    /// Shows an examine dialog for the specified target.
    /// </summary>
    /// <param name="target">The target to examine.</param>
    public async Task ActionExamine(NwGameObject target)
    {
      await WaitForObjectContext();
      NWScript.ActionExamine(target);
    }

    /// <summary>
    /// Restores the camera mode and facing to what they where when StoreCameraFacing was last called.<br/>
    /// RestoreCameraFacing can only be used once, and must correspond to a previous call to StoreCameraFacing.
    /// </summary>
    public async Task RestoreCameraFacing()
    {
      await WaitForObjectContext();
      NWScript.RestoreCameraFacing();
    }

    /// <summary>
    /// Stores (bookmarks) the camera's facing and position so it can be restored later with RestoreCameraFacing.<br/>
    /// </summary>
    public async Task StoreCameraFacing()
    {
      await WaitForObjectContext();
      NWScript.StoreCameraFacing();
    }

    /// <summary>
    /// Changes the direction this player's camera is facing.
    /// </summary>
    /// <param name="direction">Horizontal angle from East in degrees. -1 to leave the angle unmodified.</param>
    /// <param name="pitch">Vertical angle of the camera in degrees. -1 to leave the angle unmodified.</param>
    /// <param name="distance">Distance (zoom) of the camera. -1 to leave the distance unmodified.</param>
    /// <param name="transitionType">The transition to use for moving the camera.</param>
    public async Task SetCameraFacing(float direction, float pitch = -1.0f, float distance = -1.0f, CameraTransitionType transitionType = CameraTransitionType.Snap)
    {
      await WaitForObjectContext();
      NWScript.SetCameraFacing(direction, distance, pitch, (int) transitionType);
    }

    /// <summary>
    /// Forces this player's character to saved and exported to its respective directory (LocalVault, ServerVault, etc).
    /// </summary>
    public void ExportCharacter()
      => NWScript.ExportSingleCharacter(this);

    /// <summary>
    /// Sends this player to a new server, where the player's character will connect and log in.
    /// </summary>
    /// <param name="ipAddress">DNS name or the IP address + port of the destination server.</param>
    /// <param name="password">The player password to connect to the destination server.</param>
    /// <param name="waypointTag">The custom waypoint tag on the destination server for this player to jump to. Defaults to the module's start location.</param>
    /// <param name="seamless">If true, the player will not be prompted with information about the new server, and they will not be allowed to save a copy of their character (if it is a local vault character).</param>
    public void SendToServer(string ipAddress = "", string password = "", string waypointTag = "", bool seamless = false)
      => NWScript.ActivatePortal(this, ipAddress, password, waypointTag, seamless.ToInt());

    /// <summary>
    /// Sets whether this player has explored an area.
    /// </summary>
    /// <param name="area">The area to explore.</param>
    /// <param name="explored">true if this area has been explored, otherwise false to (re)hide the map.</param>
    public void SetAreaExploreState(NwArea area, bool explored)
      => NWScript.ExploreAreaForPlayer(area, this, explored.ToInt());

    /// <summary>
    /// Vibrates the player's device or controller. Does nothing if vibration is not supported.
    /// </summary>
    /// <param name="motor">Which motors to vibrate.</param>
    /// <param name="strength">The intensity of the vibration.</param>
    /// <param name="duration">How long to vibrate for.</param>
    public void Vibrate(VibratorMotor motor, float strength, TimeSpan duration)
      => NWScript.Vibrate(this, (int) motor, strength, (float) duration.TotalSeconds);

    /// <summary>
    /// Unlock an achievement for this player who must be logged in.
    /// </summary>
    /// <param name="achievementId">The achievement ID on the remote server.</param>
    /// <param name="lastValue">The previous value of the associated achievement stat.</param>
    /// <param name="currentValue">The current value of the associated achievement stat.</param>
    /// <param name="maxValue">The maximum value of the associate achievement stat.</param>
    public void UnlockAchievement(string achievementId, int lastValue = 0, int currentValue = 0, int maxValue = 0)
      => NWScript.UnlockAchievement(this, achievementId, lastValue, currentValue, maxValue);

    /// <summary>
    /// Makes this PC load a new texture instead of another.
    /// </summary>
    /// <param name="oldTexName">The existing texture to replace.</param>
    /// <param name="newTexName">The new override texture.</param>
    public void SetTextureOverride(string oldTexName, string newTexName)
      => NWScript.SetTextureOverride(oldTexName, newTexName, this);

    /// <summary>
    /// Removes the override for the specified texture, reverting to the original texture.
    /// </summary>
    /// <param name="texName">The name of the original texture.</param>
    public void ClearTextureOverride(string texName)
      => NWScript.SetTextureOverride(texName, string.Empty, this);

    /// <summary>
    /// Toggles the CutsceneMode state for this player.
    /// </summary>
    /// <param name="inCutscene">True if cutscene mode should be enabled, otherwise false.</param>
    /// <param name="leftClickEnabled">True if this user should be allowed to interact with the game with the left mouse button. False to prevent interaction.</param>
    public void SetCutsceneMode(bool inCutscene = true, bool leftClickEnabled = false)
      => NWScript.SetCutsceneMode(this, inCutscene.ToInt(), leftClickEnabled.ToInt());

    /// <summary>
    /// Displays a message on this player's screen.<br/>
    /// The message is always displayed on top of whatever is on the screen, including UI elements.
    /// </summary>
    /// <param name="message">The message to print.</param>
    /// <param name="xPos">The x coordinate relative to anchor.</param>
    /// <param name="yPos">The y coordinate relative to anchor.</param>
    /// <param name="anchor">The screen anchor/origin point.</param>
    /// <param name="life">Duration to show this string in seconds.</param>
    /// <param name="start">The starting color of this text (default: white).</param>
    /// <param name="end">The color of the text to fade to as it nears the end of the lifetime (default: white).</param>
    /// <param name="id">An optional numeric ID for this string. If not set to 0, subsequent calls to PostString will remove the text with the same ID.</param>
    /// <param name="font">If specified, the message will be rendered with the specified font instead of the default console font.</param>
    public void PostString(string message, int xPos, int yPos, ScreenAnchor anchor, float life, Color? start = null, Color? end = null, int id = 0, string font = "")
    {
      start ??= Color.WHITE;
      end ??= Color.WHITE;

      NWScript.PostString(this, message, xPos, yPos, (int) anchor, life, start.Value.ToInt(), end.Value.ToInt(), id, font);
    }

    /// <summary>
    /// Briefly displays a floating text message above this player's head using the specified string reference.
    /// </summary>
    /// <param name="strRef">The string ref index to use.</param>
    /// <param name="broadcastToParty">If true, shows the floating message to all players in the same party.</param>
    public void FloatingTextStrRef(int strRef, bool broadcastToParty = true)
        => NWScript.FloatingTextStrRefOnCreature(strRef, this, broadcastToParty.ToInt());

    /// <summary>
    /// Briefly displays a floating text message above this player's head.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="broadcastToParty">If true, shows the floating message to all players in the same party.</param>
    public void FloatingTextString(string message, bool broadcastToParty = true)
        => NWScript.FloatingTextStringOnCreature(message, this, broadcastToParty.ToInt());

    /// <summary>
    /// Enters "Cutscene" mode, disabling GUI and camera controls for the player and marking them as plot object (invulnerable).<br/>
    /// See <see cref="Effect.CutsceneGhost"/>, and other Cutscene* effects for hiding and controlling the player creature during cutscene mode.
    /// </summary>
    /// <param name="allowLeftClick">If true, allows the player to interact with the game world using the left mouse button only. Otherwise, prevents all interactions.</param>
    public void EnterCutsceneMode(bool allowLeftClick = false)
    {
      // Prevent permanent invulnerability.
      if (IsInCutsceneMode)
      {
        return;
      }

      NWScript.SetCutsceneMode(this, true.ToInt(), allowLeftClick.ToInt());
    }

    /// <summary>
    /// Exits "Cutscene" mode, restoring standard GUI and camera controls to the player, and restoring their plot flag.
    /// </summary>
    public void ExitCutsceneMode()
    {
      if (!IsInCutsceneMode)
      {
        return;
      }

      NWScript.SetCutsceneMode(this, false.ToInt(), true.ToInt());
    }

    /// <summary>
    /// Gives the specified XP to this player, adjusted by any multiclass penalty.
    /// </summary>
    /// <param name="xPAmount">Amount of experience to give.</param>
    public void GiveXp(int xPAmount)
      => NWScript.GiveXPToCreature(this, xPAmount);

    /// <summary>
    /// Locks the player's camera direction to its current direction,
    /// or unlocks the player's camera direction to enable it to move freely again.
    /// </summary>
    public void LockCameraDirection(bool isLocked = true)
      => NWScript.LockCameraDirection(this, isLocked.ToInt());

    /// <summary>
    /// Locks the player's camera pitch to its current pitch setting,
    /// or unlocks the player's camera pitch.
    /// </summary>
    public void LockCameraPitch(bool isLocked = true)
      => NWScript.LockCameraPitch(this, isLocked.ToInt());

    /// <summary>
    /// Locks the player's camera distance to its current distance setting,
    /// or unlocks the player's camera distance.
    /// </summary>
    public void LockCameraDistance(bool isLocked = true)
      => NWScript.LockCameraDistance(this, isLocked.ToInt());

    /// <summary>
    /// Changes the current Day/Night cycle for this player to daylight.
    /// </summary>
    /// <param name="delayTransitionTime">Time it takes for the daylight to fade in.</param>
    public void NightToDay(TimeSpan delayTransitionTime = default)
      => NWScript.NightToDay(this, (float)delayTransitionTime.TotalSeconds);

    /// <summary>
    /// Displays a death panel that can turn off the "Respawn" or "Wait for Help" buttons.<br/>
    /// The "Wait for Help" button is only enabled when the game is running in multiplayer mode.<br/>
    ///  By default if helpString isn't specified, the value used is "Choose an option below.<br/>
    /// Respawning will incur a penalty of 50 XP per level of your character and the loss of 10% of your gold."<br/>
    /// (strref 66219 single player, 6600 for multiplayer).<br/>
    /// </summary>
    /// <param name="respawnButton">If true the "Respawn" button will be enabled.</param>
    /// <param name="waitForHelp">If true the "Wait For Help" button will be enabled.</param>
    /// <param name="helpStringRef">String reference to display for hel.</param>
    /// <param name="helpString">String to display for help which appears in the top of the panel.</param>
    public void PopUpDeathPanel(bool respawnButton = true, bool waitForHelp = true, int helpStringRef = 0, string helpString = "")
    => NWScript.PopUpDeathGUIPanel(this, respawnButton.ToInt(), waitForHelp.ToInt(), helpStringRef, helpString);

    /// <summary>
    /// Displays a GUI panel to a player.
    /// </summary>
    /// <param name="panel">The panel type to display.</param>
    public void PopUpGUIPanel(GUIPanel panel = GUIPanel.Death)
      => NWScript.PopUpGUIPanel(this, (int)panel);

    /// <summary>
    /// Fades the screen for a given player from black to regular screen.
    /// </summary>
    /// <param name="fadeSpeed">Determines how fast the fade occurs.</param>
    public void FadeFromBlack(float fadeSpeed) => NWScript.FadeFromBlack(this, fadeSpeed);

    /// <summary>
    /// Fades the screen for a given player from a regular screen to black.
    /// </summary>
    /// <param name="fadeSpeed">Determines how fast the fade occurs.</param>
    public void FadeToBlack(float fadeSpeed) => NWScript.FadeToBlack(this, fadeSpeed);

    /// <summary>
    /// Removes any current fading effects or black screen from the monitor of the player.
    /// </summary>
    public void StopFade() => NWScript.StopFade(this);
  }
}
