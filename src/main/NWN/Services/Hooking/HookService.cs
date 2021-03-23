using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NLog;
using NWN.Core;

namespace NWN.Services
{
  /// <summary>
  /// An advanced service for hooking native NWN functions.
  /// </summary>
  [ServiceBinding(typeof(HookService))]
  [BindingOrder(BindingOrder.API)]
  public class HookService : IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly HashSet<IDisposable> hooks = new HashSet<IDisposable>();

    /// <summary>
    /// Requests a hook for a native function.
    /// </summary>
    /// <param name="handler">The handler to be invoked when this function is called. Once hooked, the original function will not be called, and must be invoked manually via the returned object.</param>
    /// <param name="order">The execution order for this hook. See the constants in <see cref="HookOrder"/>.</param>
    /// <typeparam name="T">The delegate type that identically matches the native function signature. Must have a <see cref="NativeFunctionAttribute"/> attribute.</typeparam>
    /// <returns>A wrapper object containing a delegate to the original function. The wrapped object can be disposed to release the hook.</returns>
    public FunctionHook<T> RequestHook<T>(T handler, int order = HookOrder.Default) where T : Delegate
    {
      NativeFunctionAttribute nativeFunctionInfo = typeof(T).GetCustomAttribute<NativeFunctionAttribute>();
      if (nativeFunctionInfo == null)
      {
        throw new ArgumentOutOfRangeException(nameof(handler), $"Delegate type is missing {nameof(NativeFunctionAttribute)}.");
      }

      return RequestHook(nativeFunctionInfo.Address, handler, order);
    }

    private FunctionHook<T> RequestHook<T>(uint address, T handler, int order = HookOrder.Default) where T : Delegate
    {
      Log.Debug($"Requesting function hook for {typeof(T).Name}, address 0x{address:X}");
      IntPtr managedFuncPtr = Marshal.GetFunctionPointerForDelegate(handler);
      IntPtr nativeFuncPtr = VM.RequestHook(new IntPtr(address), managedFuncPtr, order);

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
