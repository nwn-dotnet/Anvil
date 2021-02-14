using System;
using JetBrains.Annotations;

namespace NWN.Services
{
  //! ## Examples
  //! @include ServiceBindingBasics.cs
  //! @include BasicScriptHandler.cs
  //! @include TriggerHandlerService.cs
  //! @include ChatHandler.cs

  /// <summary>
  /// The main attribute used to define new services and run them.<br/>
  /// Each class flagged with a <see cref="ServiceBindingAttribute"/> will be automatically constructed on startup, and any services specified in the constructor will be injected as dependencies.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
  [MeansImplicitUse]
  public class ServiceBindingAttribute : Attribute
  {
    public readonly Type BindFrom;
    public readonly BindingType BindingType;
    internal readonly BindingContext BindingContext;

    /// <summary>
    /// Defines this class as a service.
    /// </summary>
    /// <param name="bindFrom">The type to bind to. This should usually be the class name, but may be a base class or interface implemented by the class too.</param>
    /// <param name="bindingType">Binds this service according to the specified <see cref="BindingType"/>.</param>
    public ServiceBindingAttribute(Type bindFrom, BindingType bindingType = BindingType.Singleton)
      : this(bindFrom, BindingContext.Service, bindingType) {}

    internal ServiceBindingAttribute(Type bindFrom, BindingContext bindingContext)
    {
      this.BindFrom = bindFrom;
      this.BindingContext = bindingContext;
      this.BindingType = BindingType.Singleton;
    }

    private ServiceBindingAttribute(Type bindFrom, BindingContext bindingContext, BindingType bindingType = BindingType.Singleton)
    {
      this.BindFrom = bindFrom;
      this.BindingContext = bindingContext;
      this.BindingType = bindingType;
    }
  }
}
