using System;
using System.IO;
using Anvil.API;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace Anvil.Internal
{
  internal sealed class LoggerManager : IDisposable
  {
    private static readonly SimpleLayout DefaultLayout = new SimpleLayout("${level:format=FirstCharacter} [${date}] [${logger}] ${message} ${exception:format=toString,Data}");
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

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
        Log.Info("Using default configuration");
        return;
      }

      if (File.Exists(EnvironmentConfig.NLogConfigPath))
      {
        try
        {
          LogManager.Configuration = GetConfigFromFile(EnvironmentConfig.NLogConfigPath);
          Log.Info("Using Logger config: {Path}", EnvironmentConfig.NLogConfigPath);
        }
        catch (NLogConfigurationException e)
        {
          LogManager.Configuration = GetDefaultConfig();
          Log.Warn(e, "Using default configuration as the logger configuration at {Path} failed to load", EnvironmentConfig.NLogConfigPath);
        }
      }
      else
      {
        LogManager.Configuration = GetDefaultConfig();
        Log.Warn("Using default configuration as the logger configuration at {Path} could not be found", EnvironmentConfig.NLogConfigPath);
      }
    }

    public void InitVariables()
    {
      LogManager.Configuration.Variables["nwn_home"] = NwServer.Instance.UserDirectory;
    }

    private LoggingConfiguration GetConfigFromFile(string path)
    {
      LoggingConfiguration config = new XmlLoggingConfiguration(path);
      config.Variables["nwn_home"] = NwServer.Instance.UserDirectory;

      return config;
    }

    private static LoggingConfiguration GetDefaultConfig()
    {
      LoggingConfiguration config = new LoggingConfiguration();
      ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget("console")
      {
        Layout = DefaultLayout,
      };

      FileTarget fileTarget = new FileTarget("anvil.log")
      {
        Layout = DefaultLayout,
        FileName = new SimpleLayout("${var:nwn_home}/logs.0/anvil.log"),
        CreateDirs = false,
        KeepFileOpen = true,
        OpenFileCacheTimeout = 30,
        ConcurrentWrites = false,
      };

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
