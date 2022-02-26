using System;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  [ServiceBindingOptions(InternalBindingPriority.Highest)]
  public sealed unsafe class NwServer : ICoreService
  {
    public static NwServer Instance { get; private set; }

    private CExoBase exoBase;
    private CNetLayer netLayer;
    private CServerExoApp server;

    public NwServer()
    {
      Instance = this;
    }

    /// <summary>
    /// Gets a list of all banned IPs/Keys/names as a string.
    /// </summary>
    public string BannedList => server.GetBannedListString().ToString();

    /// <summary>
    /// Gets or sets the current DM password.
    /// </summary>
    public string DMPassword
    {
      get => netLayer.GetGameMasterPassword().ToString();
      set => netLayer.SetGameMasterPassword(new CExoString(value));
    }

    /// <summary>
    /// Gets or sets the current player password.
    /// </summary>
    public string PlayerPassword
    {
      get => netLayer.GetPlayerPassword().ToString();
      set => netLayer.SetPlayerPassword(new CExoString(value));
    }

    /// <summary>
    /// Gets server configuration and display info.
    /// </summary>
    public ServerInfo ServerInfo { get; private set; }

    /// <summary>
    /// Gets the version of this server.
    /// </summary>
    public Version ServerVersion { get; set; }

    /// <summary>
    /// Gets the absolute path of the server's home directory (-userDirectory).
    /// </summary>
    public string UserDirectory { get; set; }

    /// <summary>
    /// Gets the server world timer.
    /// </summary>
    public WorldTimer WorldTimer { get; private set; }

    /// <summary>
    /// Bans the provided public CD key.
    /// </summary>
    /// <param name="cdKey">The public CD key to ban.</param>
    public void AddBannedCDKey(string cdKey)
    {
      server.AddCDKeyToBannedList(new CExoString(cdKey));
    }

    /// <summary>
    /// Bans the provided IP.
    /// </summary>
    /// <param name="ip">The IP address to ban.</param>
    public void AddBannedIP(string ip)
    {
      server.AddIPToBannedList(new CExoString(ip));
    }

    /// <summary>
    /// Bans the provided player/community name.<br/>
    /// @warning Players can change their player name at will.
    /// </summary>
    /// <param name="playerName">The player name to ban.</param>
    public void AddBannedPlayerName(string playerName)
    {
      server.AddPlayerNameToBannedList(new CExoString(playerName));
    }

    /// <summary>
    /// Delete the TURD of playerName + characterName.
    /// <para>At times a PC may get stuck in a permanent crash loop when attempting to login. This function allows administrators to delete their Temporary User
    /// Resource Data where the PC's current location is stored allowing them to log into the starting area.</para>
    /// </summary>
    /// <param name="playerName">The community (login name).</param>
    /// <param name="characterName">The character name.</param>
    /// <returns>true if the TURD was successfully deleted.</returns>
    public bool DeletePlayerTURD(string playerName, string characterName)
    {
      if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(characterName))
      {
        return false;
      }

      CExoLinkedListInternal turds = NwModule.Instance.Module.m_lstTURDList.m_pcExoLinkedListInternal;
      CExoLinkedListNode node = FindTURD(turds, playerName, characterName);
      if (node == null)
      {
        return false;
      }

      turds.Remove(node);
      return true;
    }

    /// <summary>
    /// Gets the absolute path defined for the given directory alias (see nwn.ini).
    /// </summary>
    /// <param name="alias">The alias name.</param>
    /// <returns>The path defined for the specified alias, otherwise null if the alias could not be found.</returns>
    public string GetAliasPath(string alias)
    {
      return exoBase.m_pcExoAliasList.GetAliasPath(new CExoString(alias), 0).ToString();
    }

    /// <summary>
    /// Reloads all game rules (2da stuff, etc).<br/>
    /// @warning DANGER, DRAGONS. Bad things may or may not happen. Only use this if you know what you are doing.
    /// </summary>
    [Obsolete("Use NwRuleset.ReloadRules instead.")]
    public void ReloadRules()
    {
      NWNXLib.Rules().ReloadAll();
    }

    /// <summary>
    /// Removes the ban on the provided public CD key.
    /// </summary>
    /// <param name="cdKey">The public CD key to unban.</param>
    public void RemoveBannedCDKey(string cdKey)
    {
      server.RemoveCDKeyFromBannedList(new CExoString(cdKey));
    }

    /// <summary>
    /// Removes the ban on the provided IP.
    /// </summary>
    /// <param name="ip">The IP Address to unban.</param>
    public void RemoveBannedIP(string ip)
    {
      server.RemoveIPFromBannedList(new CExoString(ip));
    }

    /// <summary>
    /// Removes the ban on the provided player/community name.
    /// </summary>
    /// <param name="playerName">The player name to unban.</param>
    public void RemoveBannedPlayer(string playerName)
    {
      server.RemovePlayerNameFromBannedList(new CExoString(playerName));
    }

    /// <summary>
    /// Signals the server to immediately shutdown.
    /// </summary>
    public void ShutdownServer()
    {
      *NWNXLib.ExitProgram() = 1;
    }

    private static CExoLinkedListNode FindTURD(CExoLinkedListInternal turds, string playerName, string characterName)
    {
      for (CExoLinkedListNode node = turds.pHead; node != null; node = node.pNext)
      {
        CNWSPlayerTURD turd = CNWSPlayerTURD.FromPointer(node.pObject);
        if (turd != null)
        {
          string turdCharacterName = turd.m_lsFirstName.ExtractLocString();
          string turdLastName = turd.m_lsLastName.ExtractLocString();

          if (!string.IsNullOrEmpty(turdLastName))
          {
            turdCharacterName = string.IsNullOrEmpty(turdCharacterName) ? turdLastName : turdCharacterName + " " + turdLastName;
          }

          if (turd.m_sCommunityName.ToString() == playerName && characterName == turdCharacterName)
          {
            return node;
          }
        }
      }

      return null;
    }

    void ICoreService.Init()
    {
      exoBase = NWNXLib.ExoBase();
      UserDirectory = exoBase.m_sUserDirectory.ToString();
      ServerVersion = new Version($"{NWNXLib.BuildNumber()}.{NWNXLib.BuildRevision()}");
    }

    void ICoreService.Load()
    {
      server = NWNXLib.AppManager().m_pServerExoApp;
      netLayer = server.GetNetLayer();

      WorldTimer = new WorldTimer(server.GetWorldTimer());
      ServerInfo = new ServerInfo(server.GetServerInfo(), netLayer);
    }

    void ICoreService.Unload() {}

    void ICoreService.Shutdown() {}
  }
}
