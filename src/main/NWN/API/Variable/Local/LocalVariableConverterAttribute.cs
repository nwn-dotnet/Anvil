using System;
using JetBrains.Annotations;

namespace NWN.API
{
  //! ## Examples
  //! @include DateTimeLocalVariableConverter.cs

  /// <summary>
  /// The attribute used in conjunction with <see cref="ILocalVariableConverter{T}"/> to implement a variable converter for the specified type.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class)]
  [MeansImplicitUse]
  public class LocalVariableConverterAttribute : Attribute
  {
    public readonly Type[] Types;

    /// <summary>
    /// Defines this class as a variable converter. MUST NOT BE USED WITH <see cref="NWN.Services.ServiceBindingAttribute"/>.
    /// </summary>
    /// <param name="types">The type/s handled by this converter. Each type must have a corresponding <see cref="ILocalVariableConverter{T}"/> interface that is implemented in the class.</param>
    public LocalVariableConverterAttribute(params Type[] types)
    {
      this.Types = types;
    }
  }
}
