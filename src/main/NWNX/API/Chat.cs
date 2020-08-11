using NWN.Core.NWNX;

namespace NWNX.API
{
  // TODO implement plugin functions
  public static class Chat
  {
    static Chat()
    {
      PluginUtils.AssertPluginExists<ChatPlugin>();
    }
  }
}