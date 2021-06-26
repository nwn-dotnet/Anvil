using NWN.Native.API;

namespace Anvil.Internal
{
  internal static class LowLevel
  {
    public static readonly CAppManager AppManager = NWNXLib.AppManager();
    public static readonly CServerExoApp ServerExoApp = AppManager.m_pServerExoApp;
  }
}
