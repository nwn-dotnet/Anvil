using System;
using JetBrains.Annotations;

namespace NWN.API
{
  [AttributeUsage(AttributeTargets.Class)]
  [MeansImplicitUse]
  public class CampaignVariableConverterAttribute : Attribute
  {
    public readonly Type[] Types;

    public CampaignVariableConverterAttribute(params Type[] types)
    {
      this.Types = types;
    }
  }
}