using System;

namespace NWNX
{
  [AttributeUsage(AttributeTargets.Class)]
  public class NWNXPluginAttribute : Attribute
  {
    public readonly string PluginName;
    public bool IsAvailable => NWMInteropPlugin.PluginExists(PluginName);

    public NWNXPluginAttribute(string pluginName)
    {
      this.PluginName = pluginName;
    }
  }
}