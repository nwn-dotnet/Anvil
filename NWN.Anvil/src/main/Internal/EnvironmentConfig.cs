using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Anvil.Services;

namespace Anvil.Internal
{
  /// <summary>
  /// Anvil settings that are configured through Environment Variables.
  /// </summary>
  public static class EnvironmentConfig
  {
    private static readonly string[] VariablePrefixes = { "ANVIL_", "NWM_" };

    public static readonly string AnvilHome = GetAnvilVariableString("HOME", "./anvil");
    public static readonly string Encoding = GetAnvilVariableString("ENCODING", "windows-1252");
    public static readonly LogMode LogMode = GetAnvilVariableEnum("LOG_MODE", LogMode.Default);
    public static readonly bool NativePrelinkEnabled = GetAnvilVariableBool("PRELINK_ENABLED", true);
    public static readonly bool PreventStartNoPlugin = GetAnvilVariableBool("PREVENT_START_NO_PLUGIN");
    public static readonly bool ReloadEnabled = GetAnvilVariableBool("RELOAD_ENABLED");
    public static readonly string[] AdditionalPluginPaths = GetAnvilVariableArrayString("ADD_PLUGIN_PATHS");

    static EnvironmentConfig()
    {
      ValidateUnset("NLOG_CONFIG");
      ValidateUnset("PLUGIN_PATH");
    }

    public static bool GetIsPluginDisabled(string pluginName)
    {
      return GetAnvilVariableBool($"{pluginName.ToUpper()}_SKIP");
    }

    private static bool GetAnvilVariableBool(string key, bool defaultValue = false)
    {
      string value = GetAnvilVariableString(key, defaultValue.ToString());
      return value.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    private static T GetAnvilVariableEnum<T>(string key, T defaultValue = default) where T : struct, Enum
    {
      string value = GetAnvilVariableString(key, defaultValue.ToString());
      return Enum.TryParse(value, out T result) ? result : defaultValue;
    }

    [return: NotNullIfNotNull("defaultValue")]
    private static string? GetAnvilVariableString(string key, string? defaultValue = null)
    {
      foreach (string prefix in VariablePrefixes)
      {
        string? value = Environment.GetEnvironmentVariable(prefix + key);
        if (value != null)
        {
          return value;
        }
      }

      return defaultValue;
    }

    private static string[] GetAnvilVariableArrayString(string key, string[]? defaultValue = null)
    {
      defaultValue ??= Array.Empty<string>();
      string? value = GetAnvilVariableString(key);

      return value != null ? value.Split(Path.PathSeparator) : defaultValue;
    }

    private static void ValidateUnset(string key)
    {
      if (Environment.GetEnvironmentVariable(key) != null)
      {
        throw new Exception($"Unsupported environment variable {key}. Please see the changelog for more information.");
      }
    }
  }
}
