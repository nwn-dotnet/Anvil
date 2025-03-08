using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NLog;
using NWNX.NET;
using NWNX.NET.Native;

namespace Anvil.Services
{
  /// <summary>
  /// An advanced service for hooking native NWN functions.
  /// </summary>
  public sealed unsafe class HookService : ICoreService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly HashSet<IDisposable> hooks = [];
    private readonly HashSet<IDisposable> persistentHooks = [];

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
      return CreateHook(managedFuncPtr, false, order, handler);
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
      return CreateHook<T>((IntPtr)handler, false, order);
    }

    internal FunctionHook<T> RequestCoreHook<T>(T handler, int order = HookOrder.Default) where T : Delegate
    {
      IntPtr managedFuncPtr = Marshal.GetFunctionPointerForDelegate(handler);
      return CreateHook(managedFuncPtr, true, order, handler);
    }

    internal FunctionHook<T> RequestCoreHook<T>(void* handler, int order = HookOrder.Default) where T : Delegate
    {
      return CreateHook<T>((IntPtr)handler, true, order);
    }

    private FunctionHook<T> CreateHook<T>(IntPtr managedFuncPtr, bool persist, int order = HookOrder.Default, T? managedFunc = null) where T : Delegate
    {
      NativeFunctionAttribute? info = typeof(T).GetCustomAttribute<NativeFunctionAttribute>();
      if (info == null)
      {
        throw new ArgumentException($"Delegate {typeof(T).FullName} is missing the NativeFunctionAttribute", nameof(T));
      }

      Log.Debug("Requesting function hook for {HookType}, address {Address}", typeof(T).Name, $"0x{info.Address:X}");
      FunctionHook* nativeHook = NWNXAPI.RequestFunctionHook(info.Address, managedFuncPtr, order);
      FunctionHook<T> hook = new FunctionHook<T>(this, nativeHook, managedFunc);

      if (persist)
      {
        persistentHooks.Add(hook);
      }
      else
      {
        hooks.Add(hook);
      }

      return hook;
    }

    void ICoreService.Init() {}

    void ICoreService.Load() {}

    void ICoreService.Shutdown()
    {
      foreach (IDisposable hook in persistentHooks.ToList())
      {
        hook.Dispose();
      }

      persistentHooks.Clear();
    }

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
      if (!hooks.Remove(hook))
      {
        persistentHooks.Remove(hook);
      }
    }
  }
}
