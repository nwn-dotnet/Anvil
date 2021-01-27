using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NLog;
using NWN.Core;

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
      IntPtr managedFuncPtr = Marshal.GetFunctionPointerForDelegate(handler);
      IntPtr nativeFuncPtr = VM.RequestHook(new IntPtr(address), managedFuncPtr, priority);

      FunctionHook<T> hook = new FunctionHook<T>(this, nativeFuncPtr);
      hooks.Add(hook);

      return hook;
    }

    internal void RemoveHook<T>(FunctionHook<T> hook) where T : Delegate
    {
      hooks.Remove(hook);
    }

    void IDisposable.Dispose()
    {
      foreach (IDisposable hook in hooks.ToList())
      {
        hook.Dispose();
      }
    }
  }
}
