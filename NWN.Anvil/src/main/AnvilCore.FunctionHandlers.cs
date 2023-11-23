using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Anvil.API;
using Anvil.Internal;
using Anvil.Services;
using NWN.Core;
using NWN.Native.API;
using Action = System.Action;

namespace Anvil
{
  public sealed partial class AnvilCore : ICoreFunctionHandler
  {
    [Inject]
    private static ScriptDispatchService? ScriptDispatchService { get; set; }

    [Inject]
    private static ServerUpdateLoopService? ServerUpdateLoopService { get; set; }

    private static readonly Dictionary<ulong, Action> Closures = new Dictionary<ulong, Action>();
    private static readonly Stack<uint> ScriptContexts = new Stack<uint>();

    private static ulong nextEventId;
    private static uint objectSelf;

    uint ICoreFunctionHandler.ObjectSelf => objectSelf;

    void ICoreFunctionHandler.ClosureActionDoCommand(uint obj, Action func)
    {
      if (VM.ClosureActionDoCommand(obj, nextEventId) != 0)
      {
        Closures.Add(nextEventId++, func);
      }
    }

    void ICoreFunctionHandler.ClosureAssignCommand(uint obj, Action func)
    {
      if (VM.ClosureAssignCommand(obj, nextEventId) != 0)
      {
        Closures.Add(nextEventId++, func);
      }
    }

    void ICoreFunctionHandler.ClosureDelayCommand(uint obj, float duration, Action func)
    {
      if (VM.ClosureDelayCommand(obj, duration, nextEventId) != 0)
      {
        Closures.Add(nextEventId++, func);
      }
    }

    [UnmanagedCallersOnly]
    private static void OnNWNXSignal(IntPtr signalPtr)
    {
      string signal = signalPtr.ReadNullTerminatedString();

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
      string script = scriptPtr.ReadNullTerminatedString();
      int retVal = 0;
      objectSelf = oidSelf;
      ScriptContexts.Push(oidSelf);

      try
      {
        if (ScriptDispatchService != null)
        {
          retVal = (int)ScriptDispatchService.TryExecuteScript(script, oidSelf);
        }
      }
      catch (Exception e)
      {
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
      ServerUpdateLoopService?.Update();
    }

    [UnmanagedCallersOnly]
    private static void OnAssertFail(IntPtr messagePtr, IntPtr nativeStackTracePtr)
    {
      string message = messagePtr.ReadNullTerminatedString();
      string nativeStackTrace = nativeStackTracePtr.ReadNullTerminatedString();

      StackTrace stackTrace = new StackTrace(true);
      Log.Error("An assertion failure occurred in native code.\n" +
        $"{message}{nativeStackTrace}\n" +
        $"{stackTrace}");
    }

    [UnmanagedCallersOnly]
    private static void OnServerCrash(int signal, IntPtr nativeStackTracePtr)
    {
      string stackTrace = nativeStackTracePtr.ReadNullTerminatedString();
      Version serverVersion = NwServer.Instance.ServerVersion;

      string error = signal switch
      {
        4 => "Illegal instruction",
        6 => "Program aborted",
        8 => "Floating point exception",
        11 => "Segmentation fault",
        _ => "Unknown error",
      };

      Log.Fatal("\n==============================================================\n" +
        " Please file a bug at https://github.com/nwn-dotnet/Anvil/issues\n" +
        $" {Assemblies.Anvil.GetName().Name} {AssemblyInfo.VersionInfo.InformationalVersion} has crashed. Fatal error: {error} ({signal})\n" +
        $" Using: NWN {serverVersion}, NWN.Core {Assemblies.Core.GetName().Version}, NWN.Native {Assemblies.Native.GetName().Version}\n" +
        "==============================================================\n" +
        "  Managed Backtrace:\n" +
        $"{new StackTrace(true)}" +
        $"{stackTrace}");
    }
  }
}
