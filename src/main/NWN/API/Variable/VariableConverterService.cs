using System;
using System.Collections.Generic;
using System.Reflection;
using NWN.Plugins;
using NWN.Services;

namespace NWN.API
{
  [ServiceBinding(typeof(VariableConverterService))]
  internal class VariableConverterService
  {
    private static readonly Dictionary<Type, ILocalVariableConverter> LocalVariableConverters = new Dictionary<Type, ILocalVariableConverter>();
    private static readonly Dictionary<Type, ICampaignVariableConverter> CampaignVariableConverters = new Dictionary<Type, ICampaignVariableConverter>();

    public VariableConverterService(ITypeLoader typeLoader)
    {
      foreach (Type type in typeLoader.LoadedTypes)
      {
        CheckInitLocalConverter(type);
        CheckInitCampaignConverter(type);
      }
    }

    internal static ILocalVariableConverter<T> GetLocalConverter<T>()
    {
      Type type = typeof(T);
      if (LocalVariableConverters.TryGetValue(type, out ILocalVariableConverter retVal))
      {
        return (ILocalVariableConverter<T>) retVal;
      }

      throw new Exception($"No valid variable converter found for type {type.FullName}!");
    }

    internal static ICampaignVariableConverter<T> GetCampaignConverter<T>()
    {
      Type type = typeof(T);
      if (CampaignVariableConverters.TryGetValue(type, out ICampaignVariableConverter retVal))
      {
        return (ICampaignVariableConverter<T>) retVal;
      }

      throw new Exception($"No valid variable converter found for type {type.FullName}!");
    }

    private void CheckInitLocalConverter(Type type)
    {
      LocalVariableConverterAttribute info = type.GetCustomAttribute<LocalVariableConverterAttribute>();
      if (info == null)
      {
        return;
      }

      ILocalVariableConverter converter = (ILocalVariableConverter) Activator.CreateInstance(type);
      foreach (Type infoType in info.Types)
      {
        LocalVariableConverters[infoType] = converter;
      }
    }

    private void CheckInitCampaignConverter(Type type)
    {
      CampaignVariableConverterAttribute info = type.GetCustomAttribute<CampaignVariableConverterAttribute>();
      if (info == null)
      {
        return;
      }

      ICampaignVariableConverter converter = (ICampaignVariableConverter) Activator.CreateInstance(type);
      foreach (Type infoType in info.Types)
      {
        CampaignVariableConverters[infoType] = converter;
      }
    }
  }
}
