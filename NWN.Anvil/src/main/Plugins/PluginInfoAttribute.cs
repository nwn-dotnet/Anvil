using System;

namespace Anvil.Plugins
{
  /// <summary>
  /// Additional Plugin Metadata.
  /// </summary>
  [AttributeUsage(AttributeTargets.Assembly)]
  public sealed class PluginInfoAttribute : Attribute
  {
    /// <summary>
    /// A list of optional plugin dependencies.
    /// </summary>
    /// <remarks>
    /// By default, an exception will be thrown at startup if a plugin references a type from another plugin, and the plugin is not loaded.<br/>
    /// Adding a plugin name to this list will instead cause these types to be skipped if the plugin is not loaded.<br/>
    /// </remarks>
    public string[] OptionalDependencies { get; init; } = [];

    /// <summary>
    /// Gets or sets if this is an isolated plugin.
    /// </summary>
    /// <remarks>
    /// An isolated plugin is given a separate container for services, and its services are hidden from other plugins.<br/>
    /// It may still access services from anvil, and other non-isolated plugins.<br/>
    /// Isolated plugins may be loaded and unloaded on demand, independent of startup.
    /// </remarks>
    public bool Isolated { get; init; }
  }
}
