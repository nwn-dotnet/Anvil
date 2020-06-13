using NWN.API;
using NWN.Core;
using NWN.Core.NWNX;

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
      set => AdminPlugin.SetPlayerPassword(value);
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
    /// <param name="player">The player to delete</param>
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
    /// <param name="playerName">The community (login name)</param>
    /// <param name="characterName">The character name</param>
    /// <returns>true if the TURD was successfully deleted.</returns>
    public static bool DeleteTURD(string playerName, string characterName) => AdminPlugin.DeleteTURD(playerName, characterName).ToBool();

    // / @brief Reload all rules (2da stuff etc).
    // / @warning DANGER, DRAGONS. Bad things may or may not happen.
    public static void ReloadRules() => AdminPlugin.ReloadRules();
  }
}