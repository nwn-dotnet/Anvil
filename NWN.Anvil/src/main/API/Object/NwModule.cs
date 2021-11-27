using System;
using System.Collections.Generic;
using System.Linq;
using Anvil.Internal;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// The root container object for all areas and module entities.
  /// </summary>
  [NativeObjectInfo(0, ObjectType.Module)]
  public sealed partial class NwModule : NwObject
  {
    internal readonly CNWSModule Module;

    internal NwModule(CNWSModule module) : base(module)
    {
      Module = module;
    }

    public static implicit operator CNWSModule(NwModule module)
    {
      return module?.Module;
    }

    public static readonly NwModule Instance = new NwModule(LowLevel.ServerExoApp.GetModule());

    internal override CNWSScriptVarTable ScriptVarTable
    {
      get => Module.m_ScriptVars;
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
    /// Gets or sets the max possible attack bonus from temporary effects/items (Default: 20).
    /// </summary>
    public int AttackBonusLimit
    {
      get => NWScript.GetAttackBonusLimit();
      set => NWScript.SetAttackBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible damage bonus from temporary effects/items (Default: 100).
    /// </summary>
    public int DamageBonusLimit
    {
      get => NWScript.GetDamageBonusLimit();
      set => NWScript.SetDamageBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible saving throw bonus from temporary effects/items (Default: 20).
    /// </summary>
    public int SavingThrowBonusLimit
    {
      get => NWScript.GetSavingThrowBonusLimit();
      set => NWScript.SetSavingThrowBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible ability score bonus from temporary effects/items (Default: 12).
    /// </summary>
    public int GetAbilityBonusLimit
    {
      get => NWScript.GetAbilityBonusLimit();
      set => NWScript.SetAbilityBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible ability score penalty from temporary effects/items (Default: 30).
    /// </summary>
    public int AbilityPenaltyLimit
    {
      get => NWScript.GetAbilityPenaltyLimit();
      set => NWScript.SetAbilityPenaltyLimit(value);
    }

    /// <summary>
    /// Gets or sets the max possible skill bonus from temporary effects/items (Default: 50).
    /// </summary>
    public int SkillBonusLimit
    {
      get => NWScript.GetSkillBonusLimit();
      set => NWScript.SetSkillBonusLimit(value);
    }

    /// <summary>
    /// Gets or sets the maximum number of henchmen.
    /// </summary>
    public int MaxHenchmen
    {
      get => NWScript.GetMaxHenchmen();
      set => NWScript.SetMaxHenchmen(value);
    }

    /// <summary>
    /// Gets the current server difficulty setting.
    /// </summary>
    public GameDifficulty GameDifficulty
    {
      get => (GameDifficulty)NWScript.GetGameDifficulty();
    }

    /// <summary>
    /// Gets a value indicating whether it is currently dawn.
    /// </summary>
    public bool IsDawn
    {
      get => NWScript.GetIsDawn().ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether it is currently day.
    /// </summary>
    public bool IsDay
    {
      get => NWScript.GetIsDay().ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether it is currently dusk.
    /// </summary>
    public bool IsDusk
    {
      get => NWScript.GetIsDusk().ToBool();
    }

    /// <summary>
    /// Gets a value indicating whether it is currently night.
    /// </summary>
    public bool IsNight
    {
      get => NWScript.GetIsNight().ToBool();
    }

    /// <summary>
    /// Gets the starting location for new players.
    /// </summary>
    public Location StartingLocation
    {
      get => NWScript.GetStartingLocation();
    }

    /// <summary>
    /// Gets all active areas in the module.
    /// </summary>
    public IEnumerable<NwArea> Areas
    {
      get
      {
        for (uint area = NWScript.GetFirstArea(); area != Invalid; area = NWScript.GetNextArea())
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
        CExoLinkedListCNWSClient playerList = LowLevel.ServerExoApp.m_pcExoAppInternal.m_pNWSPlayerList;

        for (CExoLinkedListNode node = playerList.GetHeadPos(); node != null; node = node.pNext)
        {
          CNWSPlayer player = playerList.GetAtPos(node).AsNWSPlayer();
          yield return player.ToNwPlayer();
        }
      }
    }

    /// <summary>
    /// Gets all objects currently stored in limbo.
    /// </summary>
    public IEnumerable<NwGameObject> LimboGameObjects
    {
      get
      {
        return Module.m_aGameObjectsLimbo.Select(objectId => objectId.ToNwObject<NwGameObject>());
      }
    }

    /// <summary>
    /// Gets the current player count.
    /// </summary>
    public uint PlayerCount
    {
      get => LowLevel.ServerExoApp.m_pcExoAppInternal.m_pNWSPlayerList.Count();
    }

    public override Guid? PeekUUID()
    {
      return null;
    }

    /// <summary>
    /// Gets the specified global campaign variable.
    /// </summary>
    /// <param name="campaign">The name of the campaign.</param>
    /// <param name="name">The variable name.</param>
    /// <typeparam name="T">The variable type.</typeparam>
    /// <returns>A CampaignVariable instance for getting/setting the variable's value.</returns>
    public T GetCampaignVariable<T>(string campaign, string name) where T : CampaignVariable, new()
    {
      return CampaignVariable.Create<T>(campaign, name);
    }

    /// <summary>
    /// Deletes the entire campaign database, if it exists.
    /// </summary>
    /// <param name="campaign">The campaign DB to delete.</param>
    public void DestroyCampaignDatabase(string campaign)
    {
      NWScript.DestroyCampaignDatabase(campaign);
    }

    /// <summary>
    /// Broadcasts a message to the DM channel, sending a message to all DMs on the server.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="color">A color to apply to the message.</param>
    public void SendMessageToAllDMs(string message, Color color)
    {
      NWScript.SendMessageToAllDMs(message.ColorString(color));
    }

    /// <inheritdoc cref="SendMessageToAllDMs(string,Color)"/>
    public void SendMessageToAllDMs(string message)
    {
      NWScript.SendMessageToAllDMs(message);
    }

    /// <summary>
    /// Ends the current running game, plays the specified movie then returns all players to the main menu.
    /// </summary>
    /// <param name="endMovie">The filename of the movie to play, without file extension.</param>
    public void EndGame(string endMovie)
    {
      NWScript.EndGame(endMovie);
    }

    /// <summary>
    /// Forces all players who are currently game to have their characters
    /// exported to their respective directories i.e. LocalVault/ServerVault/ etc.
    /// </summary>
    public void ExportAllCharacters()
    {
      NWScript.ExportAllCharacters();
    }

    /// <summary>
    /// Makes all online PCs load a new texture instead of another.
    /// </summary>
    /// <param name="oldTexName">The existing texture to replace.</param>
    /// <param name="newName">The new override texture.</param>
    public void SetTextureOverride(string oldTexName, string newName)
    {
      NWScript.SetTextureOverride(oldTexName, newName);
    }

    /// <summary>
    /// Removes the override for the specified texture, reverting to the original texture.
    /// </summary>
    /// <param name="texName">The name of the original texture.</param>
    public void ClearTextureOverride(string texName)
    {
      NWScript.SetTextureOverride(texName, string.Empty);
    }

    /// <summary>
    /// Finds the specified waypoint with the given tag.
    /// </summary>
    public NwWaypoint GetWaypointByTag(string tag)
    {
      return NWScript.GetWaypointByTag(tag).ToNwObject<NwWaypoint>();
    }

    /// <summary>
    /// Adds an entry to the journal of all players in the module.<br/>
    /// See <see cref="NwPlayer.AddJournalQuestEntry"/> to add a journal entry to a specific player/party.
    /// </summary>
    /// <param name="categoryTag">The tag of the Journal category (case-sensitive).</param>
    /// <param name="entryId">The ID of the Journal entry.</param>
    /// <param name="allowOverrideHigher">If true, disables the default restriction that requires journal entry numbers to increase.</param>
    public void AddJournalQuestEntry(string categoryTag, int entryId, bool allowOverrideHigher = false)
    {
      NWScript.AddJournalQuestEntry(categoryTag, entryId, Invalid, true.ToInt(), true.ToInt(), allowOverrideHigher.ToInt());
    }

    /// <summary>
    /// Gets the last objects that were created in the module. Use LINQ to skip or limit the query.
    /// </summary>
    /// <returns>An enumerable containing the last created objects.</returns>
    public IEnumerable<NwObject> GetLastCreatedObjects()
    {
      CGameObjectArray gameObjectArray = LowLevel.ServerExoApp.GetObjectArray();

      for (uint objectId = gameObjectArray.m_nNextObjectArrayID[0] - 1; objectId > 0; objectId--)
      {
        if (TryGetObject(objectId, out NwObject gameObject))
        {
          yield return gameObject;
        }
      }

      unsafe bool TryGetObject(uint objectId, out NwObject gameObject)
      {
        void* pObject;

        bool retVal = gameObjectArray.GetGameObject(objectId, &pObject) == 0;
        gameObject = retVal ? objectId.ToNwObject() : default;

        return retVal;
      }
    }

    /// <summary>
    /// Sets up a SQL Query for this module.<br/>
    /// This will NOT run the query; only make it available for parameter binding.<br/>
    /// To run the query, you need to call <see cref="SQLQuery.Execute"/> even if you do not expect result data.<br/>
    /// </summary>
    /// <param name="query">The query to be prepared.</param>
    /// <returns>A <see cref="SQLQuery"/> object.</returns>
    public SQLQuery PrepareSQLQuery(string query)
    {
      return NWScript.SqlPrepareQueryObject(this, query);
    }

    /// <summary>
    /// Sets up a SQL Query for the specified campaign database.<br/>
    /// This will NOT run the query; only make it available for parameter binding.<br/>
    /// To run the query, you need to call <see cref="SQLQuery.Execute"/> even if you do not expect result data.<br/>
    /// </summary>
    /// <param name="database">The database to be queried.</param>
    /// <param name="query">The query to be prepared.</param>
    /// <returns>A <see cref="SQLQuery"/> object.</returns>
    public SQLQuery PrepareCampaignSQLQuery(string database, string query)
    {
      return NWScript.SqlPrepareQueryCampaign(database, query);
    }

    /// <summary>
    /// Moves the specified <see cref="NwGameObject"/> from its current location/owner to limbo.
    /// </summary>
    /// <param name="gameObject">The game object to move to limbo.</param>
    public void MoveObjectToLimbo(NwGameObject gameObject)
    {
      if (!gameObject.IsPlayerControlled(out _) && !gameObject.IsLoginPlayerCharacter(out _))
      {
        gameObject.RemoveFromArea();
        Module.AddObjectToLimbo(gameObject);
      }
    }
  }
}
