using System;
using JetBrains.Annotations;

namespace NWN.API
{
  [AttributeUsage(AttributeTargets.Class)]
  [MeansImplicitUse]
  public sealed class CampaignVariableConverterAttribute : Attribute
  {
    public readonly Type[] Types;

    public CampaignVariableConverterAttribute(params Type[] types)
    {
      Types = types;
    }
  }
}
