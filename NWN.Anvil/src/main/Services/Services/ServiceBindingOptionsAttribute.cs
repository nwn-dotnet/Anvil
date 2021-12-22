using System;

namespace Anvil.Services
{
  /// <summary>
  /// Additional service binding options.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ServiceBindingOptionsAttribute : Attribute
  {
    public ServiceBindingOptionsAttribute()
    {
      BindingPriority = BindingPriority.Normal;
    }

    internal ServiceBindingOptionsAttribute(InternalBindingPriority priority)
    {
      Priority = (int)priority;
    }

    /// <summary>
    /// The priority of the service.<br/>
    /// Services with a higher priority are loaded first, and are chosen first to resolve dependency constraints when multiple candidates for a dependency are available.
    /// </summary>
    public BindingPriority BindingPriority
    {
      get => (BindingPriority)Priority;
      init => Priority = (int)value;
    }

    /// <summary>
    /// If true, this service will only be loaded if another service depends on it.
    /// </summary>
    public bool Lazy { get; init; }

    /// <summary>
    /// An optional list of plugin names that must exist for this service to be loaded.
    /// </summary>
    public string[] PluginDependencies { get; init; }

    internal int Priority { get; private init; }
  }
}
