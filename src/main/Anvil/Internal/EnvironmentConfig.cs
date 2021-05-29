using System;
using System.IO;

namespace Anvil.Internal
{
  /// <summary>
  /// Anvil settings that are configured through Environment Variables.
  /// </summary>
  public static class EnvironmentConfig
  {
    private static readonly string[] VariablePrefixes = { "ANVIL_", "NWM_" };

    // Anvil
    public static readonly string PluginsPath = GetAnvilVariableString("PLUGIN_PATH", Path.Combine(Assemblies.AssemblyDir, "Plugins"));
    public static readonly string ResourcePath = GetAnvilVariableString("RESOURCE_TEMP_DIR", Path.Combine(Assemblies.AssemblyDir, "res_temp"));
    public static readonly string NLogConfigPath = GetAnvilVariableString("NLOG_CONFIG");
    public static readonly bool ReloadEnabled = GetAnvilVariableBool("RELOAD_ENABLED");
    public static readonly bool PreventStartNoPlugin = GetAnvilVariableBool("PREVENT_START_NO_PLUGIN");

    // NWNX
    public static readonly string ModStartScript = Environment.GetEnvironmentVariable("NWNX_UTIL_PRE_MODULE_START_SCRIPT");
    public static readonly string CoreShutdownScript = Environment.GetEnvironmentVariable("NWNX_CORE_SHUTDOWN_SCRIPT");

    private static bool GetAnvilVariableBool(string key, bool defaultValue = false)
    {
      string value = GetAnvilVariableString(key, defaultValue.ToString());
      return value.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    private static string GetAnvilVariableString(string key, string defaultValue = null)
    {
      foreach (string prefix in VariablePrefixes)
      {
        string value = Environment.GetEnvironmentVariable(prefix + key);
        if (value != null)
        {
          return value;
        }
      }

      return defaultValue;
    }
  }
}
