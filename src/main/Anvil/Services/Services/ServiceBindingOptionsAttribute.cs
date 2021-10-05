using System;

namespace Anvil.Services
{
  /// <summary>
  /// Additional service binding options.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  public sealed class ServiceBindingOptionsAttribute : Attribute
  {
    private readonly short order;

    /// <summary>
    /// The order that this service should be loaded in, after dependency constraints have been resolved. Values less than 0 are reserved by anvil.
    /// </summary>
    public short Order
    {
      get => order;
      init
      {
        if (value < 0)
        {
          throw new ArgumentOutOfRangeException(nameof(Order), "Order must not be less than 0.");
        }

        order = value;
      }
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
    public string[] MissingPluginDependencies { get; init; }

    internal ServiceBindingOptionsAttribute(BindingOrder order)
    {
      this.order = (short)order;
    }

    public ServiceBindingOptionsAttribute()
    {
      order = (short)BindingOrder.Default;
    }
  }
}
