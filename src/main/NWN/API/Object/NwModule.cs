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

    public NwPlayer PCSpeaker
    {
      get => NWScript.GetPCSpeaker().ToNwObject<NwPlayer>();
    }

    public Spell SpellId
    {
      get => (Spell) NWScript.GetSpellId();
    }

    /// <summary>
    /// Gets or sets the XP scale for this module. Must be a value between 0-200.
    /// </summary>
    public int XPScale
    {
      get => NWScript.GetModuleXPScale();
      set => NWScript.SetModuleXPScale(value);
    }

    /// <summary>
    /// Gets or sets the max possible attack bonus from temporary effects/items. (Default: 20)
    /// </summary>
    public int AttackBonusLimit
    {
      get => NWScript.GetAttackBonusLimit();
      set => NWScript.SetAttackBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible damage bonus from temporary effects/items. (Default: 100)
    /// </summary>
    public int DamageBonusLimit
    {
      get => NWScript.GetDamageBonusLimit();
      set => NWScript.SetDamageBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible saving throw bonus from temporary effects/items. (Default: 20)
    /// </summary>
    public int SavingThrowBonusLimit
    {
      get => NWScript.GetSavingThrowBonusLimit();
      set => NWScript.SetSavingThrowBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible ability score bonus from temporary effects/items. (Default: 12)
    /// </summary>
    public int GetAbilityBonusLimit
    {
      get => NWScript.GetAbilityBonusLimit();
      set => NWScript.SetAbilityBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible ability score penalty from temporary effects/items. (Default: 30)
    /// </summary>
    public int AbilityPenaltyLimit
    {
      get => NWScript.GetAbilityPenaltyLimit();
      set => NWScript.SetAbilityPenaltyLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible skill bonus from temporary effects/items. (Default: 50)
    /// </summary>
    public int SkillBonusLimit
    {
      get => NWScript.GetSkillBonusLimit();
      set => NWScript.SetSkillBonusLimit(value);
    }

    /// <summary>
    /// Gets the current server difficulty setting.
    /// </summary>
    public GameDifficulty GameDifficulty
    {
      get => (GameDifficulty) NWScript.GetGameDifficulty();
    }

    /// <summary>
    /// Finds the specified waypoint with the given tag.
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public NwWaypoint GetWaypointByTag(string tag) => NWScript.GetWaypointByTag(tag).ToNwObject<NwWaypoint>();

    /// <summary>
    /// Gets the starting location for new players.
    /// </summary>
    public Location StartingLocation
    {
      get => NWScript.GetStartingLocation();
    }

    /// <summary>
    /// Gets the specified global campaign variable.
    /// </summary>
    /// <param name="campaign">The name of the campaign.</param>
    /// <param name="name">The variable name.</param>
    /// <typeparam name="T">The variable type.</typeparam>
    /// <returns>A CampaignVariable instance for getting/setting the variable's value.</returns>
    public CampaignVariable<T> GetCampaignVariable<T>(string campaign, string name)
      => CampaignVariable<T>.Create(campaign, name);

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
    /// Broadcasts a message to the DM channel, sending a message to all DMs on the server.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="color">A color to apply to the message.</param>
    public void SendMessageToAllDMs(string message, Color color)
      => NWScript.SendMessageToAllDMs(message.ColorString(color));

    /// <inheritdoc cref="SendMessageToAllDMs(string,NWN.API.Color)"/>
    public void SendMessageToAllDMs(string message)
      => NWScript.SendMessageToAllDMs(message);

    /// <summary>
    /// Ends the current running game, plays the specified movie then returns all players to the main menu.
    /// </summary>
    /// <param name="endMovie">The filename of the movie to play, without file extension.</param>
    public void EndGame(string endMovie)
      => NWScript.EndGame(endMovie);

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