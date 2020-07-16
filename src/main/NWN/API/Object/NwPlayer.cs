using System;
using System.Threading.Tasks;
using NWN.API.Constants;
using NWN.Core;

namespace NWN.API
{
  public sealed class NwPlayer : NwCreature
  {
    internal NwPlayer(uint objectId) : base(objectId) {}

    /// <summary>
    /// Gets if this Player is a Dungeon Master.
    /// </summary>
    public bool IsDM => NWScript.GetIsDM(ObjectId).ToBool();

    /// <summary>
    /// Gets the player's login name.
    /// </summary>
    public string PlayerName => NWScript.GetPCPlayerName(this);

    /// <summary>
    /// Gets this player's client version (Major + Minor)
    /// </summary>
    public Version ClientVersion => new Version(NWScript.GetPlayerBuildVersionMajor(this), NWScript.GetPlayerBuildVersionMinor(this));

    /// <summary>
    /// Sends a server message to this player.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="color">A color to apply to the message.</param>
    public void SendServerMessage(string message, Color color) => NWScript.SendMessageToPC(this, message.ColorString(color));

    /// <summary>
    /// Sends a server message to this player.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void SendServerMessage(string message) => NWScript.SendMessageToPC(this, message);

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
    /// Forces this player's character to saved and exported to its respective directory (LocalVault, ServerVault, etc)
    /// </summary>
    public void ExportCharacter() => NWScript.ExportSingleCharacter(this);

    /// <summary>
    /// Vibrates the player's device or controller. Does nothing if vibration is not supported.
    /// </summary>
    /// <param name="motor">Which motors to vibrate.</param>
    /// <param name="strength">The intensity of the vibration.</param>
    /// <param name="duration">How long to vibrate for.</param>
    public void Vibrate(VibratorMotor motor, float strength, TimeSpan duration) => NWScript.Vibrate(this, (int) motor, strength, (float) duration.TotalSeconds);

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
      => NWScript.SetTextureOverride(texName, "", this);

    /// <summary>
    /// Displays a message on this player's screen. <br/>
    /// The message is always displayed on top of whatever is on the screen, including UI elements.
    /// </summary>
    /// <param name="message">The message to print.</param>
    /// <param name="xPos">The x coordinate relative to anchor.</param>
    /// <param name="yPos">The y coordinate relative to anchor.</param>
    /// <param name="anchor">The screen anchor/origin point.</param>
    /// <param name="life">Duration to show this string in seconds.</param>
    /// <param name="start">The starting color of this text.</param>
    /// <param name="end">The color of the text to fade to as it nears the end of the lifetime.</param>
    /// <param name="id">An optional numeric ID for this string. If not set to 0, subsequent calls to PostString will remove the text with the same ID.</param>
    /// <param name="font">If specified, the message will be rendered with the specified font instead of the default console font.</param>
    public void PostString(string message, int xPos, int yPos, ScreenAnchor anchor, float life, Color? start = null, Color? end = null, int id = 0, string font = "")
    {
      if (start == null)
      {
        start = Color.WHITE;
      }
      if (end == null)
      {
        end = Color.WHITE;
      }

      NWScript.PostString(this, message, xPos, yPos, (int) anchor, life, start.Value.ToHex(), end.Value.ToHex(), id, font);
    }
  }
}