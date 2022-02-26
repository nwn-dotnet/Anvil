using System.IO;
using Anvil.API;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;

namespace Anvil.Services
{
  [ServiceBindingOptions(InternalBindingPriority.VeryHigh)]
  internal sealed class LoggerManager : ICoreService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private static readonly SimpleLayout DefaultLayout = new SimpleLayout("${level:format=FirstCharacter} [${date}] [${logger}] ${message}${onexception:${newline}${exception:format=ToString}}");

    private readonly NwServer nwServer;

    public LoggerManager(NwServer nwServer)
    {
      this.nwServer = nwServer;

      LogManager.AutoShutdown = false;
      LogManager.Configuration = null;
      LogManager.ThrowConfigExceptions = true;
    }

    void ICoreService.Init()
    {
      if (File.Exists(HomeStorage.NLogConfig))
      {
        try
        {
          LogManager.Configuration = GetConfigFromFile(HomeStorage.NLogConfig);
          Log.Info("Using Logger config: {Path}", HomeStorage.NLogConfig);
        }
        catch (NLogConfigurationException e)
        {
          LogManager.Configuration = GetDefaultConfig();
          Log.Warn(e, "Using default configuration as the logger configuration at {Path} failed to load", HomeStorage.NLogConfig);
        }
      }
      else
      {
        LogManager.Configuration = GetDefaultConfig();
        Log.Info("Using default configuration");
      }

      LogManager.Configuration.Variables["nwn_home"] = nwServer.UserDirectory;
    }

    void ICoreService.Load() {}

    void ICoreService.Unload() {}

    void ICoreService.Shutdown()
    {
      LogManager.Shutdown();
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

    private LoggingConfiguration GetConfigFromFile(string path)
    {
      LoggingConfiguration config = new XmlLoggingConfiguration(path);
      config.Variables["nwn_home"] = nwServer.UserDirectory;

      return config;
    }
  }
}
