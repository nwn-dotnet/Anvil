using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using NLog;
using NWN.Core;

namespace NWN.Services
{
  /// <summary>
  /// An advanced service for hooking native NWN functions.
  /// </summary>
  [ServiceBinding(typeof(HookService), BindingContext.API)]
  public class HookService : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly HashSet<IDisposable> hooks = new HashSet<IDisposable>();

    /// <summary>
    /// Requests a hook for a native function.
    /// </summary>
    /// <param name="address">The address of the native function. Use the constants available in the NWN.Native library:<see cref="NWN.Native.API.NWNXLib.Functions"/>.</param>
    /// <param name="handler">The handler to be invoked when this function is called. Once hooked, the original function will not be called, and must be invoked manually via the returned object.</param>
    /// <param name="priority">The execution order for this hook. See the constants in <see cref="HookPriority"/>.</param>
    /// <typeparam name="T">The delegate type that identically matches the native function signature.</typeparam>
    /// <returns>A wrapper object containing a delegate to the original function. The wrapped object can be disposed to release the hook.</returns>
    public FunctionHook<T> RequestHook<T>(uint address, T handler, int priority = HookPriority.Default) where T : Delegate
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
