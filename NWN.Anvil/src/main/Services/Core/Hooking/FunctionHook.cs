using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using NWNX.NET;
using NWNX.NET.Native;

namespace Anvil.Services
{
  public sealed unsafe class FunctionHook<T> : IDisposable where T : Delegate
  {
    // We hold a reference to the delegate to prevent clean up from the garbage collector.
    [UsedImplicitly]
    private readonly T? managedHandle;

    private readonly HookService hookService;
    private readonly FunctionHook* functionHook;

    private T functionDelegate;
    private void* functionDelegatePointer;

    /// <summary>
    /// The original function call - invoke this to run the standard game behaviour.
    /// </summary>
    public T CallOriginal
    {
      get
      {
        void* functionPointer = functionHook->m_trampoline;
        if (functionPointer != functionDelegatePointer)
        {
          UpdateFunctionDelegate(functionPointer);
        }

        return functionDelegate;
      }
    }

    internal FunctionHook(HookService hookService, FunctionHook* functionHook, T? managedHandle = null)
    {
      this.hookService = hookService;
      this.functionHook = functionHook;
      this.managedHandle = managedHandle;

      UpdateFunctionDelegate(functionHook->m_trampoline);
    }

    ~FunctionHook()
    {
      ReleaseUnmanagedResources();
    }

    private void ReleaseUnmanagedResources()
    {
      NWNXAPI.ReturnFunctionHook(functionHook);
    }

    [MemberNotNull(nameof(functionDelegate), nameof(functionDelegatePointer))]
    private void UpdateFunctionDelegate(void* functionPointer)
    {
      functionDelegate = Marshal.GetDelegateForFunctionPointer<T>((IntPtr)functionPointer);
      functionDelegatePointer = functionPointer;
    }

    /// <summary>
    /// Releases the FunctionHook, restoring the previous behaviour.
    /// </summary>
    public void Dispose()
    {
      ReleaseUnmanagedResources();
      GC.SuppressFinalize(this);
      hookService.RemoveHook(this);
    }
  }
}
