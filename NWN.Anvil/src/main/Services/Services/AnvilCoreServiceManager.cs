using System;
using Anvil.API;
using Anvil.Plugins;
using LightInject;
using NLog;

namespace Anvil.Services
{
  internal sealed class AnvilCoreServiceManager
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly NwServer nwServer;
    private readonly LoggerManager loggerManager;
    private readonly UnhandledExceptionLogger unhandledExceptionLogger;
    private readonly UnobservedTaskExceptionLogger unobservedTaskExceptionLogger;
    private readonly InjectionService injectionService;
    private readonly ModuleLoadTracker moduleLoadTracker;
    private readonly HookService hookService;
    private readonly VirtualMachine virtualMachine;
    private readonly PluginManager pluginManager;
    private readonly EncodingService encodingService;
    private readonly ResourceManager resourceManager;
    private readonly AnvilMessageService anvilMessageService;
    private readonly ObjectStorageService objectStorageService;

    public AnvilCoreServiceManager(IServiceContainer container)
    {
      container.RegisterCoreService<NwServer>();
      container.RegisterCoreService<LoggerManager>();
      container.RegisterCoreService<UnhandledExceptionLogger>();
      container.RegisterCoreService<UnobservedTaskExceptionLogger>();
      container.RegisterCoreService<InjectionService>();
      container.RegisterCoreService<ModuleLoadTracker>();
      container.RegisterCoreService<HookService>();
      container.RegisterCoreService<VirtualMachine>();
      container.RegisterCoreService<PluginManager>();
      container.RegisterCoreService<EncodingService>();
      container.RegisterCoreService<ResourceManager>();
      container.RegisterCoreService<AnvilMessageService>();
      container.RegisterCoreService<ObjectStorageService>();

      container.Compile();

      nwServer = container.GetInstance<NwServer>();
      loggerManager = container.GetInstance<LoggerManager>();
      unhandledExceptionLogger = container.GetInstance<UnhandledExceptionLogger>();
      unobservedTaskExceptionLogger = container.GetInstance<UnobservedTaskExceptionLogger>();
      anvilMessageService = container.GetInstance<AnvilMessageService>();
      resourceManager = container.GetInstance<ResourceManager>();
      pluginManager = container.GetInstance<PluginManager>();
      injectionService = container.GetInstance<InjectionService>();
      virtualMachine = container.GetInstance<VirtualMachine>();
      encodingService = container.GetInstance<EncodingService>();
      hookService = container.GetInstance<HookService>();
      moduleLoadTracker = container.GetInstance<ModuleLoadTracker>();
      objectStorageService = container.GetInstance<ObjectStorageService>();
    }

    public void Init()
    {
      Console.WriteLine("Initialising core services...");
      InitService(nwServer, false);
      InitService(loggerManager, false);
      InitService(unhandledExceptionLogger);
      InitService(unobservedTaskExceptionLogger);
      InitService(resourceManager);
      InitService(pluginManager);
      InitService(injectionService);
      InitService(anvilMessageService);
      InitService(virtualMachine);
      InitService(encodingService);
      InitService(hookService);
      InitService(moduleLoadTracker);
      InitService(objectStorageService);
    }

    public void Load()
    {
      Log.Info("Loading core services...");
      LoadService(nwServer);
      LoadService(loggerManager);
      LoadService(unhandledExceptionLogger);
      LoadService(unobservedTaskExceptionLogger);
      LoadService(resourceManager);
      LoadService(pluginManager);
      LoadService(injectionService);
      LoadService(anvilMessageService);
      LoadService(virtualMachine);
      LoadService(encodingService);
      LoadService(hookService);
      LoadService(moduleLoadTracker);
      LoadService(objectStorageService);
    }

    public void Start()
    {
      Log.Debug("Starting core services...");
      StartService(nwServer);
      StartService(loggerManager);
      StartService(unhandledExceptionLogger);
      StartService(unobservedTaskExceptionLogger);
      StartService(resourceManager);
      StartService(pluginManager);
      StartService(injectionService);
      StartService(anvilMessageService);
      StartService(virtualMachine);
      StartService(encodingService);
      StartService(hookService);
      StartService(moduleLoadTracker);
      StartService(objectStorageService);
    }

    public void Unload()
    {
      Log.Info("Unloading core services...");
      UnloadService(objectStorageService);
      UnloadService(moduleLoadTracker);
      UnloadService(hookService);
      UnloadService(encodingService);
      UnloadService(virtualMachine);
      UnloadService(anvilMessageService);
      UnloadService(injectionService);
      UnloadService(pluginManager);
      UnloadService(resourceManager);
      UnloadService(unobservedTaskExceptionLogger);
      UnloadService(unhandledExceptionLogger);
      UnloadService(loggerManager);
      UnloadService(nwServer);
    }

    public void Shutdown()
    {
      ShutdownService(objectStorageService);
      ShutdownService(moduleLoadTracker);
      ShutdownService(hookService);
      ShutdownService(encodingService);
      ShutdownService(virtualMachine);
      ShutdownService(anvilMessageService);
      ShutdownService(injectionService);
      ShutdownService(pluginManager);
      ShutdownService(resourceManager);
      ShutdownService(unobservedTaskExceptionLogger);
      ShutdownService(unhandledExceptionLogger);
      ShutdownService(loggerManager);
      ShutdownService(nwServer);
    }

    private static void InitService(ICoreService service, bool loggerReady = true)
    {
      if (loggerReady)
      {
        Log.Info("Initialising core service {ServiceName}", service.GetType().FullName);
      }
      else
      {
        Console.WriteLine($"Initialising core service \"{service.GetType().FullName}\"");
      }

      service.Init();
    }

    private static void LoadService(ICoreService service)
    {
      Log.Info("Loading core service {ServiceName}", service.GetType().FullName);
      service.Load();
    }

    private static void StartService(ICoreService service)
    {
      Log.Debug("Starting core service {ServiceName}", service.GetType().FullName);
      service.Start();
    }

    private static void UnloadService(ICoreService service)
    {
      Log.Info("Unloading core service {ServiceName}", service.GetType().FullName);
      service.Unload();
    }

    private static void ShutdownService(ICoreService service)
    {
      service.Shutdown();
    }
  }
}
