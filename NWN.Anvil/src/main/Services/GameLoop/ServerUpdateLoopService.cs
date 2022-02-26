using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Anvil.API;
using NLog;
using NWN.Native.API;

namespace Anvil.Services
{
  internal sealed unsafe class ServerUpdateLoopService : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    [Inject]
    private static VirtualMachine VirtualMachine { get; set; }

    internal delegate int MainLoopHook(void* pServerExoAppInternal);

    private static FunctionHook<MainLoopHook> hook;
    private static IUpdateable[] updateables;

    public ServerUpdateLoopService(HookService hookService, IEnumerable<IUpdateable> updateables)
    {
      ServerUpdateLoopService.updateables = updateables.OrderBy(updateable => updateable.GetType().GetServicePriority()).ToArray();

      delegate* unmanaged<void*, int> pHook = &OnLoop;
      hook = hookService.RequestHook<MainLoopHook>(pHook, FunctionsLinux._ZN21CServerExoAppInternal8MainLoopEv, HookOrder.VeryEarly);
    }

    public void Dispose()
    {
      updateables = Array.Empty<IUpdateable>();
    }

    [UnmanagedCallersOnly]
    public static int OnLoop(void* pServerExoAppInternal)
    {
      VirtualMachine.ExecuteInScriptContext(() =>
      {
        foreach (IUpdateable updateable in updateables)
        {
          try
          {
            updateable.Update();
          }
          catch (Exception e)
          {
            Log.Error(e);
          }
        }
      });

      return hook.CallOriginal(pServerExoAppInternal);
    }
  }
}
