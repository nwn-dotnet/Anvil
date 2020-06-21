using System;
using System.Reflection;
using NLog;
using NWN.API;
using NWN.Core.NWNX;

namespace NWNX
{
  public class PluginUtils
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static bool PluginAvailable(NWNXPluginAttribute plugin)
    {
      return UtilPlugin.PluginExists(plugin.PluginName).ToBool();
    }

    internal static bool PluginExists<T>() where T : class
    {
      NWNXPluginAttribute plugin = GetPlugin<T>();
      return plugin != null && PluginAvailable(plugin);
    }

    internal static void AssertPluginExists<T>() where T : class
    {
      NWNXPluginAttribute plugin = GetPlugin<T>();
      if (plugin != null && !PluginAvailable(plugin))
      {
        string message = $"Missing plugin dependency \"{plugin.PluginName}\". Has it been enabled in the server config?";
        Log.Fatal(message);
        throw new InvalidOperationException(message);
      }
    }

    private static NWNXPluginAttribute GetPlugin<T>() where T : class
    {
      Type pluginType = typeof(T);
      NWNXPluginAttribute plugin = pluginType.GetCustomAttribute<NWNXPluginAttribute>();

      // Invalid class specified
      if (plugin == null)
      {
        Log.Warn($"Class \"{pluginType.FullName}\" does not have a {nameof(NWNXPluginAttribute)}. It may be missing, or an invalid class specified.");
      }

      return plugin;
    }
  }
}