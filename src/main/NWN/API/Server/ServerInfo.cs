using NWN.Native.API;

namespace NWN.API
{
  public class ServerInfo
  {
    private readonly CServerInfo serverInfo;
    private readonly CNetLayer netLayer;

    public PlayOptions PlayOptions { get; }

    public PersistentWorldOptions PersistentWorldOptions { get; }

    public ServerInfo(CServerInfo serverInfo, CNetLayer netLayer)
    {
      this.serverInfo = serverInfo;
      this.netLayer = netLayer;

      PlayOptions = new PlayOptions(serverInfo.m_PlayOptions);
      PersistentWorldOptions = new PersistentWorldOptions(this.serverInfo.m_PersistantWorldOptions);
    }

    /// <summary>
    /// Gets or sets the name of the server, as shown in the server browser.
    /// </summary>
    public string ServerName
    {
      get => netLayer.GetSessionName().ToString();
      set => netLayer.SetSessionName(new CExoString(value));
    }

    /// <summary>
    /// Gets or sets the name of the module, as shown in the server browser.
    /// </summary>
    public string ModuleName
    {
      get => serverInfo.m_sModuleName.ToString();
      set => serverInfo.m_sModuleName = new CExoString(value);
    }
  }
}
