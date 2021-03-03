using System;
using System.IO;

namespace NWN
{
  /// <summary>
  /// NWN.Managed settings that are configured through Environment Variables.
  /// </summary>
  public static class EnvironmentConfig
  {
    // NWN.Managed
    public static readonly string PluginsPath = Environment.GetEnvironmentVariable("NWM_PLUGIN_PATH") ?? Path.Combine(Assemblies.AssemblyDir, "Plugins");
    public static readonly string NLogConfigPath = Environment.GetEnvironmentVariable("NWM_NLOG_CONFIG");
    public static readonly bool ReloadEnabled = string.Equals(Environment.GetEnvironmentVariable("NWM_RELOAD_ENABLED"), "true", StringComparison.InvariantCultureIgnoreCase);
    public static readonly bool PreventStartNoPlugin = string.Equals(Environment.GetEnvironmentVariable("NWM_PREVENT_START_NO_PLUGIN"), "true", StringComparison.InvariantCultureIgnoreCase);

    // NWNX
    public static readonly string ModStartScript = Environment.GetEnvironmentVariable("NWNX_UTIL_PRE_MODULE_START_SCRIPT");
    public static readonly string CoreShutdownScript = Environment.GetEnvironmentVariable("NWNX_CORE_SHUTDOWN_SCRIPT");
  }
}
