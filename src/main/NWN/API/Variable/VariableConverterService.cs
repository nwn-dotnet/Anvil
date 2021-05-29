using System;
using System.Collections.Generic;
using System.Reflection;
using NWN.Plugins;
using NWN.Services;

namespace NWN.API
{
  [ServiceBinding(typeof(VariableConverterService))]
  [ServiceBindingOptions(BindingOrder.API)]
  internal class VariableConverterService
  {
    private readonly Dictionary<Type, ILocalVariableConverter> localVariableConverters = new Dictionary<Type, ILocalVariableConverter>();
    private readonly Dictionary<Type, ICampaignVariableConverter> campaignVariableConverters = new Dictionary<Type, ICampaignVariableConverter>();

    public VariableConverterService(ITypeLoader typeLoader)
    {
      foreach (Type type in typeLoader.LoadedTypes)
      {
        CheckInitLocalConverter(type);
        CheckInitCampaignConverter(type);
      }
    }

    internal ILocalVariableConverter<T> GetLocalConverter<T>()
    {
      Type type = typeof(T);
      if (localVariableConverters.TryGetValue(type, out ILocalVariableConverter retVal))
      {
        return (ILocalVariableConverter<T>)retVal;
      }

      throw new Exception($"No valid variable converter found for type {type.FullName}!");
    }

    internal ICampaignVariableConverter<T> GetCampaignConverter<T>()
    {
      Type type = typeof(T);
      if (campaignVariableConverters.TryGetValue(type, out ICampaignVariableConverter retVal))
      {
        return (ICampaignVariableConverter<T>)retVal;
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

      ILocalVariableConverter converter = (ILocalVariableConverter)Activator.CreateInstance(type);
      foreach (Type infoType in info.Types)
      {
        localVariableConverters[infoType] = converter;
      }
    }

    private void CheckInitCampaignConverter(Type type)
    {
      CampaignVariableConverterAttribute info = type.GetCustomAttribute<CampaignVariableConverterAttribute>();
      if (info == null)
      {
        return;
      }

      ICampaignVariableConverter converter = (ICampaignVariableConverter)Activator.CreateInstance(type);
      foreach (Type infoType in info.Types)
      {
        campaignVariableConverters[infoType] = converter;
      }
    }
  }
}
