using System;
using JetBrains.Annotations;

namespace Anvil.Services
{
  /// <summary>
  /// Indicates a property as a service dependency to be injected.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property)]
  [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
  public sealed class InjectAttribute : Attribute
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="InjectAttribute"/> class.
    /// </summary>
    public InjectAttribute() : this(string.Empty) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="InjectAttribute"/> class.
    /// </summary>
    /// <param name="serviceName">The name of the service to be injected.</param>
    public InjectAttribute(string serviceName)
    {
      ServiceName = serviceName;
    }

    /// <summary>
    /// Marks this dependency as optional. Optional dependencies do not throw exceptions if the service is not available.
    /// </summary>
    public bool Optional { get; init; }

    /// <summary>
    /// Gets the name of the service to be injected.
    /// </summary>
    public string ServiceName { get; }
  }
}
