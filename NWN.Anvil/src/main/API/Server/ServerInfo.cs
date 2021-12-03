using NWN.Native.API;

namespace Anvil.API
{
  public sealed class ServerInfo
  {
    private readonly CNetLayer netLayer;
    private readonly CServerInfo serverInfo;

    internal ServerInfo(CServerInfo serverInfo, CNetLayer netLayer)
    {
      this.serverInfo = serverInfo;
      this.netLayer = netLayer;

      PlayOptions = new PlayOptions(serverInfo.m_PlayOptions);
      PersistentWorldOptions = new PersistentWorldOptions(serverInfo.m_PersistantWorldOptions);
      JoiningRestrictions = new JoiningRestrictions(serverInfo.m_JoiningRestrictions);
      DebugOptions = new DebugOptions();
    }

    public DebugOptions DebugOptions { get; }

    public JoiningRestrictions JoiningRestrictions { get; }

    /// <summary>
    /// Gets or sets the name of the module, as shown in the server browser.
    /// </summary>
    public string ModuleName
    {
      get => serverInfo.m_sModuleName.ToString();
      set => serverInfo.m_sModuleName = new CExoString(value);
    }

    public PersistentWorldOptions PersistentWorldOptions { get; }

    public PlayOptions PlayOptions { get; }

    /// <summary>
    /// Gets or sets the name of the server, as shown in the server browser.
    /// </summary>
    public string ServerName
    {
      get => netLayer.GetSessionName().ToString();
      set => netLayer.SetSessionName(new CExoString(value));
    }
  }
}
