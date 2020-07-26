using System.Collections.Generic;
using NWN.API.Constants;
using NWN.Core;
using NWNX.API.Constants;

namespace NWN.API
{
  [NativeObjectInfo(0, InternalObjectType.Module)]
  public sealed class NwModule : NwObject
  {
    internal NwModule(uint objectId) : base(objectId) {}

    public static readonly NwModule Instance = new NwModule(NWScript.GetModule());

    public NwPlayer PCSpeaker => NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();

    public Spell SpellId => (Spell) NWScript.GetSpellId();

    /// <summary>
    /// Finds the specified waypoint with the given tag.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public NwWaypoint GetWaypointByTag(string tag) => NWScript.GetWaypointByTag(tag).ToNwObject<NwWaypoint>();

    /// <summary>
    /// Gets the starting location for new players.
    /// </summary>
    public Location StartingLocation => NWScript.GetStartingLocation();

    /// <summary>
    /// Gets all active areas in the module.
    /// </summary>
    public IEnumerable<NwArea> Areas
    {
      get
      {
        for (uint area = NWScript.GetFirstArea(); area != INVALID; area = NWScript.GetNextArea())
        {
          yield return area.ToNwObject<NwArea>();
        }
      }
    }

    /// <summary>
    /// Gets all current online players.
    /// </summary>
    public IEnumerable<NwPlayer> Players
    {
      get
      {
        for (uint player = NWScript.GetFirstPC(); player != INVALID; player = NWScript.GetNextPC())
        {
          yield return player.ToNwObject<NwPlayer>();
        }
      }
    }

    /// <summary>
    /// Gets all objects in the module with the specified tag.
    /// </summary>
    /// <param name="tag">The object tag to search.</param>
    public IEnumerable<NwGameObject> GetObjectsByTag(string tag)
    {
      int i;
      uint obj;
      for (i = 0, obj = NWScript.GetObjectByTag(tag, i); obj != INVALID; i++, obj = NWScript.GetObjectByTag(tag, i))
      {
        yield return obj.ToNwObject<NwGameObject>();
      }
    }

    /// <summary>
    /// Broadcasts a message to the DM channel with the given color, sending a message to all DMs on the server.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="color">The color of the message.</param>
    public void SendMessageToAllDMs(string message, Color color)
    {
      NWScript.SendMessageToAllDMs(message.ColorString(color));
    }

    /// <summary>
    /// Broadcasts a message to the DM channel sending a message to all DMs on the server.
    /// </summary>
    /// <param name="message">The message to send.</param>
    public void SendMessageToAllDMs(string message)
    {
      NWScript.SendMessageToAllDMs(message);
    }

    /// <summary>
    /// Makes all online PCs load a new texture instead of another.
    /// </summary>
    /// <param name="oldTexName">The existing texture to replace.</param>
    /// <param name="newName">The new override texture.</param>
    public void SetTextureOverride(string oldTexName, string newName) => NWScript.SetTextureOverride(oldTexName, newName);

    /// <summary>
    /// Removes the override for the specified texture, reverting to the original texture.
    /// </summary>
    /// <param name="texName">The name of the original texture.</param>
    public void ClearTextureOverride(string texName) => NWScript.SetTextureOverride(texName, "");
  }
}