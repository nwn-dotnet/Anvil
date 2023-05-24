using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NLog;
using NWN.Core;

namespace Anvil.Services
{
  /// <summary>
  /// An advanced service for hooking native NWN functions.
  /// </summary>
  public sealed unsafe class HookService : ICoreService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly HashSet<IDisposable> hooks = new HashSet<IDisposable>();

    /// <summary>
    /// Requests a hook for a native function.
    /// </summary>
    /// <param name="handler">The handler to be invoked when this function is called. Once hooked, the original function will not be called, and must be invoked manually via the returned object.</param>
    /// <param name="order">The execution order for this hook. See the constants in <see cref="HookOrder"/>.</param>
    /// <typeparam name="T">The delegate type that identically matches the native function signature.</typeparam>
    /// <returns>A wrapper object containing a delegate to the original function. The wrapped object can be disposed to release the hook.</returns>
    public FunctionHook<T> RequestHook<T>(T handler, int order = HookOrder.Default) where T : Delegate
    {
      IntPtr managedFuncPtr = Marshal.GetFunctionPointerForDelegate(handler);
      IntPtr nativeFuncPtr = CreateHook<T>(managedFuncPtr, order);
      FunctionHook<T> hook = new FunctionHook<T>(this, nativeFuncPtr, handler);
      hooks.Add(hook);

      return hook;
    }

    /// <summary>
    /// Requests a hook for a native function.
    /// </summary>
    /// <param name="handler">A delegate pointer (delegate*) to be invoked when the original game function is called. Once hooked, the original function will not be called, and must be invoked manually via the returned object.</param>
    /// <param name="order">The execution order for this hook. See the constants in <see cref="HookOrder"/>.</param>
    /// <typeparam name="T">The delegate type that identically matches the native function signature. Must have the <see cref="NativeFunctionAttribute"/> applied.</typeparam>
    /// <returns>A wrapper object containing a delegate to the original function. The wrapped object can be disposed to release the hook.</returns>
    public FunctionHook<T> RequestHook<T>(void* handler, int order = HookOrder.Default) where T : Delegate
    {
      IntPtr nativeFuncPtr = CreateHook<T>((IntPtr)handler, order);
      FunctionHook<T> hook = new FunctionHook<T>(this, nativeFuncPtr);
      hooks.Add(hook);

      return hook;
    }

    private IntPtr CreateHook<T>(IntPtr handler, int order)
    {
      NativeFunctionAttribute? info = typeof(T).GetCustomAttribute<NativeFunctionAttribute>();
      if (info == null)
      {
        throw new ArgumentException($"Delegate {typeof(T).FullName} is missing the NativeFunctionAttribute", nameof(T));
      }

      Log.Debug("Requesting function hook for {HookType}, address {Address}", typeof(T).Name, $"0x{info.Address:X}");
      return VM.RequestHook(info.Address, handler, order);
    }

    void ICoreService.Init() {}

    void ICoreService.Load() {}

    void ICoreService.Shutdown() {}

    void ICoreService.Start() {}

    void ICoreService.Unload()
    {
      foreach (IDisposable hook in hooks.ToList())
      {
        hook.Dispose();
      }

      hooks.Clear();
    }

    internal void RemoveHook<T>(FunctionHook<T> hook) where T : Delegate
    {
      hooks.Remove(hook);
    }
  }
}
