using NWN.Core.NWNX;

namespace NWNX.API
{
  public static class Chat
  {
    static Chat()
    {
      PluginUtils.AssertPluginExists<ChatPlugin>();
    }
  }
}