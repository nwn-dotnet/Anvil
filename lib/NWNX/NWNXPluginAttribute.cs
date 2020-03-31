using System;

namespace NWNX
{
  [AttributeUsage(AttributeTargets.Class)]
  public class NWNXPluginAttribute : Attribute
  {
    public readonly string PluginName;
    public bool IsAvailable => UtilPlugin.PluginExists(PluginName);

    public NWNXPluginAttribute(string pluginName)
    {
      PluginName = pluginName;
    }
  }
}