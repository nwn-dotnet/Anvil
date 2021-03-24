using System;
using System.Runtime.InteropServices;
using NWN.Core;

namespace NWN.Services
{
  public class FunctionHook<T> : IDisposable where T : Delegate
  {
    private readonly HookService hookService;
    private readonly T handler; // We hold a reference to the delegate to prevent clean up from the garbage collector.
    private readonly IntPtr nativeFuncPtr;

    /// <summary>
    /// The original function call - invoke this to run the standard game behaviour.
    /// </summary>
    public readonly T Original;

    internal FunctionHook(HookService hookService, T handler, IntPtr nativeFuncPtr)
    {
      this.hookService = hookService;
      this.handler = handler;
      this.nativeFuncPtr = nativeFuncPtr;
      Original = Marshal.GetDelegateForFunctionPointer<T>(nativeFuncPtr);
    }

    /// <summary>
    /// Releases the FunctionHook, restoring the previous behaviour.
    /// </summary>
    public void Dispose()
    {
      VM.ReturnHook(nativeFuncPtr);
      hookService.RemoveHook(this);
    }
  }
}
