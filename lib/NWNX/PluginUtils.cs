using System;
using System.Reflection;
using NLog;

namespace NWNX
{
  internal class PluginUtils
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    internal static bool PluginExists<T>() where T : class
    {
      NWNXPluginAttribute plugin = GetPlugin<T>();
      return plugin != null && plugin.IsAvailable;
    }

    internal static void AssertPluginExists<T>() where T : class
    {
      NWNXPluginAttribute plugin = GetPlugin<T>();
      if (plugin != null && !plugin.IsAvailable)
      {
        string message = $"NWM Init failure! Missing plugin dependency \"{plugin.PluginName}\". Has it been enabled in the server config?";
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