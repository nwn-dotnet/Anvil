using System;
using System.Runtime.InteropServices;
using NWN.Native.API;

namespace NWN.Services
{
  public class FunctionHook<T> : IDisposable where T : Delegate
  {
    private readonly HookService hookService;
    private readonly FunctionHook nativeHook;

    public readonly T Original;

    internal FunctionHook(HookService hookService, FunctionHook nativeHook)
    {
      this.hookService = hookService;
      this.nativeHook = nativeHook;
      Original = Marshal.GetDelegateForFunctionPointer<T>(nativeHook.m_trampoline);
    }

    public void Dispose()
    {
      nativeHook?.Dispose();
      hookService.RemoveHook(this);
    }
  }
}
