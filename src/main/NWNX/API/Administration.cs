using NWN.API;
using NWN.Core;
using NWN.Core.NWNX;
using NWNX.API.Constants;

namespace NWNX.API
{
  public static class Administration
  {
    static Administration()
    {
      PluginUtils.AssertPluginExists<AdminPlugin>();
    }

    /// <summary>
    /// Gets or sets the current player password.
    /// </summary>
    public static string PlayerPassword
    {
      get => AdminPlugin.GetPlayerPassword();
      set
      {
        if (value == null)
        {
          AdminPlugin.ClearPlayerPassword();
        }
        else
        {
          AdminPlugin.SetPlayerPassword(value);
        }
      }
    }

    /// <summary>
    /// Gets or sets the current DM password.
    /// </summary>
    public static string DMPassword
    {
      get => AdminPlugin.GetDMPassword();
      set => AdminPlugin.SetDMPassword(value);
    }

    /// <summary>
    /// Signals the server to immediately shutdown.
    /// </summary>
    public static void ShutdownServer() => AdminPlugin.ShutdownServer();

    /// <summary>
    /// Deletes the player character from the servervault.<br/>
    /// The PC will be immediately booted from the game with a "Delete Character" message.
    /// </summary>
    /// <param name="player">The player to delete.</param>
    /// <param name="preserveBackup">If true, it will leave the file on the server, only appending ".deleted0" to the bic filename.</param>
    public static void DeletePlayerCharacter(NwPlayer player, bool preserveBackup) => AdminPlugin.DeletePlayerCharacter(player, preserveBackup.ToInt());

    /// <summary>
    /// Bans the provided IP.
    /// </summary>
    /// <param name="ip">The IP Address to ban.</param>
    public static void AddBannedIP(string ip) => AdminPlugin.AddBannedIP(ip);

    /// <summary>
    /// Removes the ban on the provided IP.
    /// </summary>
    /// <param name="ip">The IP Address to unban.</param>
    public static void RemoveBannedIP(string ip) => AdminPlugin.RemoveBannedIP(ip);

    // / @brief Bans the provided Public CD Key.
    // / @param key The Public CD Key to ban.
    public static void AddBannedCDKey(string key) => AdminPlugin.AddBannedCDKey(key);

    // / @brief Removes the ban on the provided Public CD Key.
    // / @param key The Public CD Key to unban.
    public static void RemoveBannedCDKey(string key) => AdminPlugin.RemoveBannedCDKey(key);

    // / @brief Bans the provided playername.
    // / @param playerName The player name (community name) to ban.
    // / @warning A user can change their playername at will.
    public static void AddBannedPlayerName(string playerName) => AdminPlugin.AddBannedPlayerName(playerName);

    // / @brief Removes the ban on the provided playername.
    // / @param playerName The player name (community name) to unban.
    public static void RemoveBannedPlayerName(string playerName) => AdminPlugin.RemoveBannedPlayerName(playerName);

    // / @brief Get a list of all banned IPs/Keys/names as a string.
    // / @return A string with a listing of the banned IPs/Keys/names.
    public static string GetBannedList() => AdminPlugin.GetBannedList();

    /// <summary>
    /// Gets or sets the module's name as shown to the server list.
    /// </summary>
    public static string ModuleName
    {
      get => NWScript.GetModuleName();
      set => AdminPlugin.SetModuleName(value);
    }

    /// <summary>
    /// Gets or sets the server name as shown to the server list.
    /// </summary>
    public static string ServerName
    {
      get => AdminPlugin.GetServerName();
      set => AdminPlugin.SetServerName(value);
    }

    /// <summary>
    /// Delete the TURD of playerName + characterName.
    /// <para>At times a PC may get stuck in a permanent crash loop when attempting to login. This function allows administrators to delete their Temporary User
    /// Resource Data where the PC's current location is stored allowing them to log into the starting area.</para>
    /// </summary>
    /// <param name="playerName">The community (login name).</param>
    /// <param name="characterName">The character name.</param>
    /// <returns>true if the TURD was successfully deleted.</returns>
    public static bool DeleteTURD(string playerName, string characterName) => AdminPlugin.DeleteTURD(playerName, characterName).ToBool();

    /// <summary>
    /// Reloads all game rules (2da stuff, etc).<br/>
    /// @warning DANGER, DRAGONS. Bad things may or may not happen. Only use this if you know what you are doing.
    /// </summary>
    public static void ReloadRules() => AdminPlugin.ReloadRules();

    // TODO document
    public static class GameOptions
    {
      public static bool AllKillable
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_ALL_KILLABLE).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_ALL_KILLABLE, value.ToInt());
      }

      public static bool NonPartyKillable
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_NON_PARTY_KILLABLE).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_NON_PARTY_KILLABLE, value.ToInt());
      }

      public static bool RequireResurrection
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_REQUIRE_RESURRECTION).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_REQUIRE_RESURRECTION, value.ToInt());
      }

      public static bool LoseStolenItems
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_STOLEN_ITEMS).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_STOLEN_ITEMS, value.ToInt());
      }

      public static bool LoseItems
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_ITEMS).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_ITEMS, value.ToInt());
      }

      public static bool LoseExp
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_EXP).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_EXP, value.ToInt());
      }

      public static bool LoseGold
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_GOLD).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_GOLD, value.ToInt());
      }

      public static int LoseGoldNum
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_GOLD_NUM);
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_GOLD_NUM, value);
      }

      public static int LoseExpNum
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_EXP_NUM);
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_EXP_NUM, value);
      }

      public static int LoseItemsNum
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_ITEMS_NUM);
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_LOSE_ITEMS_NUM, value);
      }

      public static PvPSetting PvPSetting
      {
        get => (PvPSetting) AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_PVP_SETTING);
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_PVP_SETTING, (int) value);
      }

      public static bool PauseAndPlay
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_PAUSE_AND_PLAY).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_PAUSE_AND_PLAY, value.ToInt());
      }

      public static bool OnPartyOnly
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_ONE_PARTY_ONLY).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_ONE_PARTY_ONLY, value.ToInt());
      }

      public static bool EnforceLegalCharacters
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_ENFORCE_LEGAL_CHARACTERS).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_ENFORCE_LEGAL_CHARACTERS, value.ToInt());
      }

      public static bool ItemLevelRestrictions
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_ITEM_LEVEL_RESTRICTIONS).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_ITEM_LEVEL_RESTRICTIONS, value.ToInt());
      }

      public static bool CdKeyBanlistAllowlist
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_CDKEY_BANLIST_ALLOWLIST).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_CDKEY_BANLIST_ALLOWLIST, value.ToInt());
      }

      public static bool DisallowShouting
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_DISALLOW_SHOUTING).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_DISALLOW_SHOUTING, value.ToInt());
      }

      public static bool ShowDMJoinMessage
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_SHOW_DM_JOIN_MESSAGE).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_SHOW_DM_JOIN_MESSAGE, value.ToInt());
      }

      public static bool BackupSavedCharacters
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_BACKUP_SAVED_CHARACTERS).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_BACKUP_SAVED_CHARACTERS, value.ToInt());
      }

      public static bool AutoFailSaveOn1
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_AUTO_FAIL_SAVE_ON_1).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_AUTO_FAIL_SAVE_ON_1, value.ToInt());
      }

      public static bool ValidateSpells
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_VALIDATE_SPELLS).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_VALIDATE_SPELLS, value.ToInt());
      }

      public static bool ExamineEffects
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_EXAMINE_EFFECTS).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_EXAMINE_EFFECTS, value.ToInt());
      }

      public static bool ExamineChallengeRating
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_EXAMINE_CHALLENGE_RATING).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_EXAMINE_CHALLENGE_RATING, value.ToInt());
      }

      public static bool UseMaxHPOnLevelUp
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_USE_MAX_HITPOINTS).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_USE_MAX_HITPOINTS, value.ToInt());
      }

      public static bool RestoreSpellUses
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_RESTORE_SPELLS_USES).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_RESTORE_SPELLS_USES, value.ToInt());
      }

      public static bool ResetEncounterSpawnPool
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_RESET_ENCOUNTER_SPAWN_POOL).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_RESET_ENCOUNTER_SPAWN_POOL, value.ToInt());
      }

      public static bool HideHitpointsGained
      {
        get => AdminPlugin.GetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_HIDE_HITPOINTS_GAINED).ToBool();
        set => AdminPlugin.SetPlayOption(AdminPlugin.NWNX_ADMINISTRATION_OPTION_HIDE_HITPOINTS_GAINED, value.ToInt());
      }
    }

    // TODO document
    public static class DebugOptions
    {
      public static bool Combat
      {
        get => AdminPlugin.GetDebugValue(AdminPlugin.NWNX_ADMINISTRATION_DEBUG_COMBAT).ToBool();
        set => AdminPlugin.SetDebugValue(AdminPlugin.NWNX_ADMINISTRATION_DEBUG_COMBAT, value.ToInt());
      }

      public static bool SavingThrow
      {
        get => AdminPlugin.GetDebugValue(AdminPlugin.NWNX_ADMINISTRATION_DEBUG_SAVING_THROW).ToBool();
        set => AdminPlugin.SetDebugValue(AdminPlugin.NWNX_ADMINISTRATION_DEBUG_SAVING_THROW, value.ToInt());
      }

      public static bool MovementSpeed
      {
        get => AdminPlugin.GetDebugValue(AdminPlugin.NWNX_ADMINISTRATION_DEBUG_MOVEMENT_SPEED).ToBool();
        set => AdminPlugin.SetDebugValue(AdminPlugin.NWNX_ADMINISTRATION_DEBUG_MOVEMENT_SPEED, value.ToInt());
      }

      public static bool HitDie
      {
        get => AdminPlugin.GetDebugValue(AdminPlugin.NWNX_ADMINISTRATION_DEBUG_HIT_DIE).ToBool();
        set => AdminPlugin.SetDebugValue(AdminPlugin.NWNX_ADMINISTRATION_DEBUG_HIT_DIE, value.ToInt());
      }
    }
  }
}
