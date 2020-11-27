using System;
using System.IO;

namespace NWN
{
  public sealed class EnvironmentConfig
  {
    // NWN.Managed
    public static readonly string PluginsPath = Environment.GetEnvironmentVariable("NWM_PLUGIN_PATH") ?? Path.Combine(AssemblyConstants.AssemblyDir, "Plugins");
    public static readonly string NLogConfigPath = Environment.GetEnvironmentVariable("NWM_NLOG_CONFIG") ?? Path.Combine(AssemblyConstants.AssemblyDir, "nlog.config");

    // NWNX
    public static readonly string ModStartScript = Environment.GetEnvironmentVariable("NWNX_UTIL_PRE_MODULE_START_SCRIPT");
    public static readonly string CoreShutdownScript = Environment.GetEnvironmentVariable("NWNX_CORE_SHUTDOWN_SCRIPT");
  }
}
