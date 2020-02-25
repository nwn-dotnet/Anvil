using NWM.Core;

namespace NWM
{
  internal static class Main
  {
    private static ServiceManager serviceManager;
    private static ScriptHandlerDispatcher scriptHandlerDispatcher;

    private static bool initialized;

    public static void OnStart()
    {
      serviceManager = new ServiceManager();
    }

    public static void OnMainLoop(ulong frame)
    {
      // TODO
    }

    public static int OnRunScript(string script, uint oidSelf)
    {
      if (!initialized)
      {
        Init();
      }

      return scriptHandlerDispatcher.ExecuteScript(script, oidSelf);
    }

    private static void Init()
    {
      initialized = true;
      serviceManager.Verify();
      scriptHandlerDispatcher = serviceManager.GetService<ScriptHandlerDispatcher>();
      scriptHandlerDispatcher.Init(serviceManager.GetRegisteredServices());
    }
  }
}