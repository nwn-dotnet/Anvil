using System;
using System.IO;
using System.Reflection;
using Anvil.API;
using Anvil.Internal;
using Anvil.Plugins;

namespace Anvil.Services
{
  /// <summary>
  /// Manages content stored in the Anvil home directory.<br/>
  /// Use this service to get a path for reading and writing configs or data for your plugin.
  /// </summary>
  [ServiceBinding(typeof(HomeStorageService))]
  public sealed class HomeStorageService
  {
    private readonly Lazy<PluginManager> pluginManager;

    private readonly string anvilHome = Path.GetFullPath(EnvironmentConfig.AnvilHome, NwServer.Instance.UserDirectory);

    internal string PluginStorage
    {
      get => ResolvePath("Plugins");
    }

    internal string Paket
    {
      get => ResolvePath("Paket");
    }

    public HomeStorageService(Lazy<PluginManager> pluginManager)
    {
      this.pluginManager = pluginManager;
    }

    /// <summary>
    /// Gets the storage path for the specified plugin.
    /// </summary>
    /// <param name="pluginAssembly">The assembly of the plugin, e.g. typeof(MyService).Assembly</param>
    /// <returns>The storage directory for the specified plugin.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified assembly is not a plugin.</exception>
    public string GetPluginStoragePath(Assembly pluginAssembly)
    {
      if (pluginManager.Value.IsPluginAssembly(pluginAssembly))
      {
        return Path.Combine(PluginStorage, pluginAssembly.GetName().Name!);
      }

      throw new ArgumentException("Specified assembly is not a loaded plugin assembly.", nameof(pluginAssembly));
    }

    private string ResolvePath(string subPath)
    {
      string path = Path.Combine(anvilHome, subPath);
      Directory.CreateDirectory(path);
      return path;
    }
  }
}
