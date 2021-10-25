using System;

namespace Anvil.Services
{
  /// <summary>
  /// Additional service binding options.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ServiceBindingOptionsAttribute : Attribute
  {
    /// <summary>
    /// The order that this service should be loaded in, after dependency constraints have been resolved. Values less than 0 are reserved by anvil.
    /// </summary>
    [Obsolete("Use the BindingPriority property instead. This property will be removed in a future release.")]
    public short Order
    {
      init => Priority = value;
    }

    internal int Priority { get; private init; }

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

    /// <summary>
    /// An optional list of plugin names that must be missing for this service to be loaded.
    /// </summary>
    [Obsolete("This property will be removed in a future release. BindingPriority now determines which service gets injected for multiple dependency candidates, making this property obsolete.")]
    public string[] MissingPluginDependencies { get; init; }

    internal ServiceBindingOptionsAttribute(InternalBindingPriority priority)
    {
      Priority = (int)priority;
    }

    public ServiceBindingOptionsAttribute()
    {
      BindingPriority = BindingPriority.Normal;
    }
  }
}
