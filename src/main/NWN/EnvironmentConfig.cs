using System;

namespace NWN
{
  internal class EnvironmentConfig
  {
    // NWN.Managed
    public static readonly string PluginsPath = Environment.GetEnvironmentVariable("NWM_PLUGIN_PATH");

    // NWNX
    public static readonly string ModStartScript = Environment.GetEnvironmentVariable("NWNX_UTIL_PRE_MODULE_START_SCRIPT");
    public static readonly string CoreShutdownScript = Environment.GetEnvironmentVariable("NWNX_CORE_SHUTDOWN_SCRIPT");
  }
}