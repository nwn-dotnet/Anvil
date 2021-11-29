using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using NWN.Core;

namespace Anvil.Services
{
  public sealed class FunctionHook<T> : IDisposable where T : Delegate
  {
    /// <summary>
    /// The original function call - invoke this to run the standard game behaviour.
    /// </summary>
    public readonly T CallOriginal;

    // We hold a reference to the delegate to prevent clean up from the garbage collector.
    [UsedImplicitly]
    private readonly T handler;

    private readonly HookService hookService;
    private readonly IntPtr nativeFuncPtr;

    internal FunctionHook(HookService hookService, IntPtr nativeFuncPtr, T handler = null)
    {
      this.hookService = hookService;
      this.nativeFuncPtr = nativeFuncPtr;
      this.handler = handler;
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
