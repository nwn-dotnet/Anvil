using NWM.API;
using NWN;

namespace NWNX
{
  [NWNXPlugin(PLUGIN_NAME)]
  internal class UtilPlugin
  {
    public const string PLUGIN_NAME = "NWNX_Util";

    // This plugin is required.
    static UtilPlugin() => PluginUtils.AssertPluginExists<UtilPlugin>();

    public static string GetUserDirectory()
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "GetUserDirectory");
      Internal.NativeFunctions.nwnxCallFunction();
      return Internal.NativeFunctions.nwnxPopString();
    }

    public static bool PluginExists(string pluginName)
    {
      Internal.NativeFunctions.nwnxSetFunction(PLUGIN_NAME, "PluginExists");
      Internal.NativeFunctions.nwnxPushString(pluginName);
      Internal.NativeFunctions.nwnxCallFunction();
      return Internal.NativeFunctions.nwnxPopInt().ToBool();
    }
  }
}