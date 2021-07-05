using System.Diagnostics.Contracts;
using LightInject;

namespace NWN.Services
{
  [ServiceBinding(typeof(InjectionService))]
  [ServiceBindingOptions(BindingOrder.API)]
  public sealed class InjectionService
  {
    [Inject]
    private IServiceContainer Container { get; init; }

    /// <summary>
    /// Injects all properties with <see cref="InjectAttribute"/> in the specified object.
    /// </summary>
    /// <param name="instance">The instance to inject.</param>
    /// <typeparam name="T">The instance type.</typeparam>
    /// <returns>The instance with injected dependencies.</returns>
    [Pure]
    public T Inject<T>(T instance)
    {
      Container.InjectProperties(instance);
      return instance;
    }
  }
}
