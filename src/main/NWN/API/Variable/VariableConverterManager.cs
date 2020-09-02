using System;
using System.Collections.Generic;
using System.Reflection;

namespace NWN.API
{
  internal static class VariableConverterManager
  {
    private static readonly Dictionary<Type, ILocalVariableConverter> localVariableConverters = new Dictionary<Type, ILocalVariableConverter>();
    private static readonly Dictionary<Type, ICampaignVariableConverter> campaignVariableConverters = new Dictionary<Type, ICampaignVariableConverter>();

    internal static ILocalVariableConverter<T> GetLocalConverter<T>()
    {
      Type type = typeof(T);
      if (localVariableConverters.TryGetValue(type, out ILocalVariableConverter retVal))
      {
        return (ILocalVariableConverter<T>) retVal;
      }

      throw new Exception($"No valid variable converter found for type {type.FullName}!");
    }

    internal static ICampaignVariableConverter<T> GetCampaignConverter<T>()
    {
      Type type = typeof(T);
      if (campaignVariableConverters.TryGetValue(type, out ICampaignVariableConverter retVal))
      {
        return (ICampaignVariableConverter<T>) retVal;
      }

      throw new Exception($"No valid variable converter found for type {type.FullName}!");
    }

    static VariableConverterManager()
    {
      foreach (Type type in Types.AllLinkedTypes)
      {
        CheckInitLocalConverter(type);
        CheckInitCampaignConverter(type);
      }
    }

    private static void CheckInitLocalConverter(Type type)
    {
      LocalVariableConverterAttribute info = type.GetCustomAttribute<LocalVariableConverterAttribute>();
      if (info == null)
      {
        return;
      }

      ILocalVariableConverter converter = (ILocalVariableConverter) Activator.CreateInstance(type);
      foreach (Type infoType in info.Types)
      {
        localVariableConverters[infoType] = converter;
      }
    }

    private static void CheckInitCampaignConverter(Type type)
    {
      CampaignVariableConverterAttribute info = type.GetCustomAttribute<CampaignVariableConverterAttribute>();
      if (info == null)
      {
        return;
      }

      ICampaignVariableConverter converter = (ICampaignVariableConverter) Activator.CreateInstance(type);
      foreach (Type infoType in info.Types)
      {
        campaignVariableConverters[infoType] = converter;
      }
    }
  }
}