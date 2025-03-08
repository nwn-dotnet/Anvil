using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Anvil.Services;
using NWN.Core;
using NWNX.NET;
using NWNX.NET.Native;
using Action = System.Action;

namespace Anvil
{
  public sealed partial class AnvilCore : ICoreFunctionHandler
  {
    [Inject]
    private static ScriptDispatchService? ScriptDispatchService { get; set; }

    [Inject]
    private static AnvilMessageService? AnvilMessageService { get; set; }

    private static readonly Dictionary<ulong, Action> Closures = new Dictionary<ulong, Action>();
    private static readonly Stack<uint> ScriptContexts = new Stack<uint>();

    private static ulong nextEventId;
    private static uint objectSelf;

    uint ICoreFunctionHandler.ObjectSelf => objectSelf;

    void ICoreFunctionHandler.ClosureActionDoCommand(uint obj, Action func)
    {
      if (NWNXAPI.ClosureActionDoCommand(obj, nextEventId) != 0)
      {
        Closures.Add(nextEventId++, func);
      }
    }

    void ICoreFunctionHandler.ClosureAssignCommand(uint obj, Action func)
    {
      if (NWNXAPI.ClosureAssignCommand(obj, nextEventId) != 0)
      {
        Closures.Add(nextEventId++, func);
      }
    }

    void ICoreFunctionHandler.ClosureDelayCommand(uint obj, float duration, Action func)
    {
      if (NWNXAPI.ClosureDelayCommand(obj, duration, nextEventId) != 0)
      {
        Closures.Add(nextEventId++, func);
      }
    }

    [UnmanagedCallersOnly]
    private static void OnNWNXSignal(IntPtr signalPtr)
    {
      string? signal = signalPtr.ReadNullTerminatedString();

      switch (signal)
      {
        case "ON_NWNX_LOADED":
          instance.Init();
          break;
        case "ON_MODULE_LOAD_FINISH":
          instance.LoadAndStart();
          break;
        case "ON_DESTROY_SERVER":
          Log.Info("Server is shutting down...");
          instance.Unload();
          break;
        case "ON_DESTROY_SERVER_AFTER":
          instance.Shutdown();
          break;
      }
    }

    [UnmanagedCallersOnly]
    private static int OnRunScript(IntPtr scriptPtr, uint oidSelf)
    {
      string? script = scriptPtr.ReadNullTerminatedString();
      if (script == null)
      {
        return 0;
      }

      int retVal;
      objectSelf = oidSelf;
      ScriptContexts.Push(oidSelf);

      try
      {
        retVal = (int)(ScriptDispatchService?.TryExecuteScript(script, oidSelf) ?? ScriptHandleResult.Handled);
      }
      catch (Exception e)
      {
        retVal = 0;
        Log.Error(e, "An exception occured while executing script {Script}", script);
      }

      ScriptContexts.Pop();
      objectSelf = ScriptContexts.Count == 0 ? NWScript.OBJECT_INVALID : ScriptContexts.Peek();
      return retVal;
    }

    [UnmanagedCallersOnly]
    private static void OnClosure(ulong eid, uint oidSelf)
    {
      uint old = objectSelf;
      objectSelf = oidSelf;

      try
      {
        Closures[eid].Invoke();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }

      Closures.Remove(eid);
      objectSelf = old;
    }

    [UnmanagedCallersOnly]
    private static void OnLoop(ulong _)
    {
      AnvilMessageService?.RunServerLoop();
    }

    [UnmanagedCallersOnly]
    private static void OnAssertFail(IntPtr messagePtr, IntPtr nativeStackTracePtr)
    {
      string? message = messagePtr.ReadNullTerminatedString();
      string? nativeStackTrace = nativeStackTracePtr.ReadNullTerminatedString();

      StackTrace stackTrace = new StackTrace(true);
      Log.Error("An assertion failure occurred in native code.\n" +
        $"{message}{nativeStackTrace}\n" +
        $"{stackTrace}");
    }
  }
}
