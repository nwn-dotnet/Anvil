using System;
using JetBrains.Annotations;

namespace Anvil.Services
{
  /// <summary>
  /// The main attribute used to define new services and run them.<br/>
  /// Each class flagged with a <see cref="ServiceBindingAttribute"/> will be automatically constructed on startup, and any services specified in the constructor will be injected as dependencies.
  /// </summary>
  /// <example>[!code-csharp[](~/../NWN.Anvil.Samples/src/main/Services/ServiceBindingBasics.cs)]</example>
  /// <example>[!code-csharp[](~/../NWN.Anvil.Samples/src/main/Services/BasicScriptHandler.cs)]</example>
  /// <example>[!code-csharp[](~/../NWN.Anvil.Samples/src/main/Services/TriggerHandlerService.cs)]</example>
  /// <example>[!code-csharp[](~/../NWN.Anvil.Samples/src/main/Services/ChatHandler.cs)]</example>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  [MeansImplicitUse(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, ImplicitUseTargetFlags.Itself)]
  public sealed class ServiceBindingAttribute : Attribute
  {
    public readonly Type BindFrom;

    /// <summary>
    /// Defines this class as a service.
    /// </summary>
    /// <param name="bindFrom">The type to bind to. This should usually be the class name, but may be a base class or interface implemented by the class too.</param>
    public ServiceBindingAttribute(Type bindFrom)
    {
      BindFrom = bindFrom;
    }
  }
}
