using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NLog;
using NWN.Core;
using NWN.Native.API;

namespace NWN.Services
{
  [ServiceBinding(typeof(HookService), BindingContext.API)]
  public class HookService : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly HashSet<IDisposable> hooks = new HashSet<IDisposable>();

    public FunctionHook<T> RequestHook<T>(T handler, uint address, int priority = 0) where T : Delegate
    {
      Log.Debug($"Requesting function hook for {typeof(T).Name}, address 0x{address:X}");
      IntPtr handlerPtr = Marshal.GetFunctionPointerForDelegate(handler);
      IntPtr hook = VM.RequestHook(new IntPtr(address), handlerPtr, priority);

      FunctionHook nativeHook = new FunctionHook(hook, true);
      FunctionHook<T> managedHook = new FunctionHook<T>(this, nativeHook);

      hooks.Add(managedHook);
      return managedHook;
    }

    internal void RemoveHook<T>(FunctionHook<T> hook) where T : Delegate
    {
      hooks.Remove(hook);
    }

    public void Dispose()
    {
      foreach (IDisposable hook in hooks.ToList())
      {
        hook.Dispose();
      }
    }
  }
}
