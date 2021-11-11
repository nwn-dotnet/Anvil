using System;
using System.IO;
using System.Reflection;
using Anvil.API;
using Anvil.Internal;
using Anvil.Services;

namespace Anvil.Plugins
{
  /// <summary>
  /// Manages user generated/configured content from plugins.<br/>
  /// Use this service to get a path for reading and writing configs or data for your plugin.
  /// </summary>
  [ServiceBinding(typeof(PluginStorageService))]
  public sealed class PluginStorageService
  {
    private readonly string storageBasePath;
    private readonly PluginManager pluginManager;

    public PluginStorageService(PluginManager pluginManager)
    {
      this.pluginManager = pluginManager;
      storageBasePath = Path.GetFullPath(EnvironmentConfig.PluginStoragePath, NwServer.Instance.UserDirectory);
    }

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
        return Path.Combine(storageBasePath, pluginAssembly.GetName().Name!);
      }

      throw new ArgumentException("Specified assembly is not a loaded plugin assembly.", nameof(pluginAssembly));
    }
  }
}
