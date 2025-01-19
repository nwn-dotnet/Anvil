using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using NWNX.NET;
using NWNX.NET.Native;

namespace Anvil.Services
{
  public sealed unsafe class FunctionHook<T> : IDisposable where T : Delegate
  {
    /// <summary>
    /// The original function call - invoke this to run the standard game behaviour.
    /// </summary>
    public readonly T CallOriginal;

    // We hold a reference to the delegate to prevent clean up from the garbage collector.
    [UsedImplicitly]
    private readonly T? handler;

    private readonly HookService hookService;
    private readonly FunctionHook* functionHook;

    internal FunctionHook(HookService hookService, FunctionHook* functionHook, T? handler = null)
    {
      this.hookService = hookService;
      this.functionHook = functionHook;
      this.handler = handler;
      CallOriginal = Marshal.GetDelegateForFunctionPointer<T>((IntPtr)functionHook->m_trampoline);
    }

    /// <summary>
    /// Releases the FunctionHook, restoring the previous behaviour.
    /// </summary>
    public void Dispose()
    {
      NWNXAPI.ReturnFunctionHook(functionHook);
      hookService.RemoveHook(this);
    }
  }
}
