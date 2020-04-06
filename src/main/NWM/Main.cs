using System;
using System.IO;
using System.Reflection;
using NWM.Core;
using NWNX;

namespace NWM
{
  internal static class Main
  {
    private static ServiceManager serviceManager;
    private static DispatchServiceManager handlerDispatcher;
    private static LoopService loopService;

    private static bool initialized;

    public static void OnStart()
    {
      serviceManager = new ServiceManager();
      AppendAssemblyToPath();
    }

    public static void OnMainLoop(ulong frame)
    {
      loopService?.Update();
    }

    public static int OnRunScript(string script, uint oidSelf)
    {
      if (!initialized)
      {
        Init();
      }

      return handlerDispatcher.OnRunScript(script, oidSelf);
    }

    // Needed to allow native libs to be loaded.
    private static void AppendAssemblyToPath()
    {
      string envPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
      string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

      Environment.SetEnvironmentVariable("PATH", $"{envPath}; {assemblyDir}");
    }

    private static void Init()
    {
      initialized = true;
      serviceManager.Verify();
      handlerDispatcher = serviceManager.GetService<DispatchServiceManager>();
      loopService = serviceManager.GetService<LoopService>();
      serviceManager.GetService<AttributeDispatchService>().Init(serviceManager.GetRegisteredServices());
    }

    private static void CheckPluginDependencies()
    {
      PluginUtils.AssertPluginExists<UtilPlugin>();
      PluginUtils.AssertPluginExists<ObjectPlugin>();
    }
  }
}