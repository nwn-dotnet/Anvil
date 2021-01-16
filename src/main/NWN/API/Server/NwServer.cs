using NWN.Native.API;

namespace NWN.API
{
  public class NwServer
  {
    private readonly CExoBase exoBase;
    private readonly CServerExoApp server;
    private readonly CNetLayer netLayer;

    public static readonly NwServer Instance = new NwServer(NWNXLib.ExoBase(), NWNXLib.AppManager().m_pServerExoApp);

    internal NwServer(CExoBase exoBase, CServerExoApp server)
    {
      this.exoBase = exoBase;
      this.server = server;
      this.netLayer = server.GetNetLayer();

      UserDirectory = exoBase.m_sUserDirectory.ToString();
      ServerInfo = new ServerInfo(server.GetServerInfo(), netLayer);
    }

    /// <summary>
    /// Gets the absolute path of the server's home directory (-userDirectory).
    /// </summary>
    public string UserDirectory { get; }

    /// <summary>
    /// Gets server configuration and display info.
    /// </summary>
    public ServerInfo ServerInfo { get; }

    /// <summary>
    /// Gets or sets the current player password.
    /// </summary>
    public string PlayerPassword
    {
      get => netLayer.GetPlayerPassword().ToString();
      set => netLayer.SetPlayerPassword(new CExoString(value));
    }

    /// <summary>
    /// Gets or sets the current DM password.
    /// </summary>
    public string DMPassword
    {
      get => netLayer.GetGameMasterPassword().ToString();
      set => netLayer.SetPlayerPassword(new CExoString(value));
    }

    /// <summary>
    /// Gets a list of all banned IPs/Keys/names as a string.
    /// </summary>
    public string BannedList
    {
      get => server.GetBannedListString().ToString();
    }

    /// <summary>
    /// Bans the provided IP.
    /// </summary>
    /// <param name="ip">The IP address to ban.</param>
    public void AddBannedIP(string ip)
      => server.AddIPToBannedList(new CExoString(ip));

    /// <summary>
    /// Removes the ban on the provided IP.
    /// </summary>
    /// <param name="ip">The IP Address to unban.</param>
    public void RemoveBannedIP(string ip)
      => server.RemoveIPFromBannedList(new CExoString(ip));

    /// <summary>
    /// Bans the provided public CD key.
    /// </summary>
    /// <param name="cdKey">The public CD key to ban.</param>
    public void AddBannedCDKey(string cdKey)
      => server.AddCDKeyToBannedList(new CExoString(cdKey));

    /// <summary>
    /// Removes the ban on the provided public CD key.
    /// </summary>
    /// <param name="cdKey">The public CD key to unban.</param>
    public void RemoveBannedCDKey(string cdKey)
      => server.RemoveCDKeyFromBannedList(new CExoString(cdKey));

    /// <summary>
    /// Bans the provided player/community name.<br/>
    /// @warning Players can change their player name at will.
    /// </summary>
    /// <param name="playerName">The player name to ban.</param>
    public void AddBannedPlayerName(string playerName)
      => server.AddPlayerNameToBannedList(new CExoString(playerName));

    /// <summary>
    /// Removes the ban on the provided player/community name.
    /// </summary>
    /// <param name="playerName">The player name to unban.</param>
    public void RemoveBannedPlayer(string playerName)
      => server.RemovePlayerNameFromBannedList(new CExoString(playerName));

    /// <summary>
    /// Reloads all game rules (2da stuff, etc).<br/>
    /// @warning DANGER, DRAGONS. Bad things may or may not happen. Only use this if you know what you are doing.
    /// </summary>
    public void ReloadRules()
      => NWNXLib.Rules().ReloadAll();

    /// <summary>
    /// Gets the absolute path defined for the given directory alias (see nwn.ini).
    /// </summary>
    /// <param name="alias">The alias name.</param>
    /// <returns>The path defined for the specified alias, otherwise null if the alias could not be found.</returns>
    public string GetAliasPath(string alias)
    {
      return exoBase.m_pcExoAliasList.GetAliasPath(new CExoString(alias), 0).ToString();
    }
  }
}
