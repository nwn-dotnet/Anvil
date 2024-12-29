using System;
using System.IO;
using System.Reflection;
using Anvil.Plugins;

namespace Anvil.Services
{
  /// <summary>
  /// Manages plugin data stored in the Anvil home directory.<br/>
  /// Use this service to get a path for reading and writing configs or data for your plugin.
  /// </summary>
  [ServiceBinding(typeof(PluginStorageService))]
  public sealed class PluginStorageService(PluginManager pluginManager)
  {
    /// <summary>
    /// Gets the storage path for the specified plugin.
    /// </summary>
    /// <param name="pluginAssembly">The assembly of the plugin, e.g. typeof(MyService).Assembly</param>
    /// <returns>The storage directory for the specified plugin.</returns>
    /// <exception cref="ArgumentException">Thrown if the specified assembly is not a plugin.</exception>
    public string GetPluginStoragePath(Assembly pluginAssembly)
    {
      if (pluginManager.IsPluginAssembly(pluginAssembly))
      {
        string path = Path.Combine(HomeStorage.PluginData, pluginAssembly.GetName().Name!);
        Directory.CreateDirectory(path);
        return path;
      }

      throw new ArgumentException("Specified assembly is not a loaded plugin assembly", nameof(pluginAssembly));
    }
  }
}
