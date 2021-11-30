using System.Collections.Generic;
using System.IO;
using Anvil.Internal;
using Anvil.Services;
using NLog;

namespace Anvil.Plugins
{
  internal sealed class LocalPluginSource : IPluginSource
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const string PluginResourceDir = "resources";

    private readonly PluginManager pluginManager;

    public LocalPluginSource(PluginManager pluginManager)
    {
      this.pluginManager = pluginManager;
    }

    public IEnumerable<Plugin> Bootstrap()
    {
      string[] pluginPaths = Directory.GetDirectories(HomeStorage.Plugins);

      Log.Info("Loading {PluginCount} DotNET plugin/s from: {PluginPath}", pluginPaths.Length, HomeStorage.Plugins);
      return CreatePluginsFromPaths(pluginPaths);
    }

    private IEnumerable<Plugin> CreatePluginsFromPaths(IEnumerable<string> pluginPaths)
    {
      List<Plugin> plugins = new List<Plugin>();
      foreach (string pluginRoot in pluginPaths)
      {
        string pluginName = new DirectoryInfo(pluginRoot).Name;

        if (Assemblies.IsReservedName(pluginName))
        {
          Log.Warn("Skipping plugin {Plugin} as it uses a reserved name", pluginName);
          continue;
        }

        string pluginPath = Path.Combine(pluginRoot, $"{pluginName}.dll");
        if (!File.Exists(pluginPath))
        {
          Log.Warn("Cannot find plugin assembly {Plugin}. Does your plugin assembly match the name of the directory?", pluginPath);
          continue;
        }

        Plugin plugin = new Plugin(pluginManager, pluginPath)
        {
          ResourcePath = Path.Combine(pluginRoot, Path.Combine(pluginRoot, PluginResourceDir)),
        };

        plugins.Add(plugin);
      }

      return plugins;
    }
  }
}
