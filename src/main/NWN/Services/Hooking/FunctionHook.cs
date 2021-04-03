using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using NWN.Core;

namespace NWN.Services
{
  public class FunctionHook<T> : IDisposable where T : Delegate
  {
    private readonly HookService hookService;
    private readonly IntPtr nativeFuncPtr;

    // We hold a reference to the delegate to prevent clean up from the garbage collector.
    [UsedImplicitly]
    private readonly T handler;

    /// <summary>
    /// The original function call - invoke this to run the standard game behaviour.
    /// </summary>
    public readonly T CallOriginal;

    internal FunctionHook(HookService hookService, T handler, IntPtr nativeFuncPtr)
    {
      this.hookService = hookService;
      this.handler = handler;
      this.nativeFuncPtr = nativeFuncPtr;
      CallOriginal = Marshal.GetDelegateForFunctionPointer<T>(nativeFuncPtr);
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
