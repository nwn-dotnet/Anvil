using System;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NWN.API;
using NWN.Core.NWNX;
using NWN.Native.API;

namespace NWN
{
  public class LoggerManager : IDisposable
  {
    private static readonly SimpleLayout DefaultLayout = new SimpleLayout("${level:format=FirstCharacter} [${date}] [${logger}] ${message} ${exception}");
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private static readonly string UserDirectory = NWNXLib.ExoBase().m_sUserDirectory.ToString();

    public LoggerManager()
    {
      LogManager.AutoShutdown = false;
      LogManager.Configuration = null;
      LogManager.ThrowConfigExceptions = true;
    }

    public void Init()
    {
      if (string.IsNullOrEmpty(EnvironmentConfig.NLogConfigPath))
      {
        LogManager.Configuration = GetDefaultConfig();
        Log.Info("Using default configuration.");
        return;
      }

      if (File.Exists(EnvironmentConfig.NLogConfigPath))
      {
        try
        {
          LogManager.Configuration = GetConfigFromFile(EnvironmentConfig.NLogConfigPath);
          Log.Info($"Using Logger config: \"{EnvironmentConfig.NLogConfigPath}\"");
        }
        catch (NLogConfigurationException e)
        {
          LogManager.Configuration = GetDefaultConfig();
          Log.Warn($"Using default configuration as the logger configuration at \"{EnvironmentConfig.NLogConfigPath}\" failed to load:\n{e}");
        }
      }
      else
      {
        LogManager.Configuration = GetDefaultConfig();
        Log.Warn($"Using default configuration as the logger configuration at \"{EnvironmentConfig.NLogConfigPath}\" could not be found.");
      }
    }

    private LoggingConfiguration GetConfigFromFile(string path)
    {
      LoggingConfiguration config = new XmlLoggingConfiguration(path);
      config.Variables["nwn_home"] = NwServer.Instance.UserDirectory;

      return config;
    }

    private LoggingConfiguration GetDefaultConfig()
    {
      LoggingConfiguration config = new LoggingConfiguration();
      config.Variables["nwn_home"] = NwServer.Instance.UserDirectory;

      ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget("console");
      consoleTarget.Layout = DefaultLayout;

      FileTarget fileTarget = new FileTarget("nwm.log");
      fileTarget.Layout = DefaultLayout;
      fileTarget.FileName = new SimpleLayout("${var:nwn_home}/logs.0/nwm.log");
      fileTarget.CreateDirs = false;
      fileTarget.KeepFileOpen = true;
      fileTarget.OpenFileCacheTimeout = 30;
      fileTarget.ConcurrentWrites = false;

      config.AddTarget(consoleTarget);
      config.AddTarget(fileTarget);
      config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);
      config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget);

      return config;
    }

    public void Dispose()
    {
      LogManager.Shutdown();
    }
  }
}
