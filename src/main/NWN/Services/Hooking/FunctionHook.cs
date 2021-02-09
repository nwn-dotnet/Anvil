using System;
using System.Runtime.InteropServices;
using NWN.Core;

namespace NWN.Services
{
  public class FunctionHook<T> : IDisposable where T : Delegate
  {
    private readonly HookService hookService;
    private readonly IntPtr nativeFuncPtr;

    /// <summary>
    /// The original function call - invoke this to run the standard game behaviour.
    /// </summary>
    public readonly T Original;

    internal FunctionHook(HookService hookService, IntPtr nativeFuncPtr)
    {
      this.hookService = hookService;
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
