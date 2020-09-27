using System;
using JetBrains.Annotations;

namespace NWN.API
{
  [AttributeUsage(AttributeTargets.Class)]
  [MeansImplicitUse]
  public class LocalVariableConverterAttribute : Attribute
  {
    public readonly Type[] Types;

    public LocalVariableConverterAttribute(params Type[] types)
    {
      this.Types = types;
    }
  }
}
