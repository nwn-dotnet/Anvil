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

    [Inject]
    private InjectionService InjectionService { get; init; } = null!;

    private readonly string rootPath;

    public LocalPluginSource(string rootPath)
    {
      this.rootPath = rootPath;
    }

    public IEnumerable<Plugin> Bootstrap()
    {
      string[] pluginPaths = Directory.GetDirectories(rootPath);

      Log.Info("Loading {PluginCount} DotNET plugin/s from: {PluginPath}", pluginPaths.Length, rootPath);
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

        Plugin plugin = InjectionService.Inject(new Plugin(pluginPath)
        {
          ResourcePath = Path.Combine(pluginRoot, Path.Combine(pluginRoot, PluginResourceDir)),
        });

        plugins.Add(plugin);
      }

      return plugins;
    }
  }
}
