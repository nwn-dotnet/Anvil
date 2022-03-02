using System;
using System.Collections.Generic;
using NLog;
using NWN.Core;
using Action = System.Action;

namespace Anvil.Services
{
  internal sealed class VirtualMachineFunctionHandler : ICoreService, ICoreFunctionHandler
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    [Inject]
    private static ScriptDispatchService ScriptDispatchService { get; set; }

    [Inject]
    private static ServerUpdateLoopService ServerUpdateLoopService { get; set; }

    private readonly Dictionary<ulong, Action> closures = new Dictionary<ulong, Action>();
    private readonly Stack<uint> scriptContexts = new Stack<uint>();

    private ulong nextEventId;
    private uint objectSelf;

    uint ICoreFunctionHandler.ObjectSelf => objectSelf;

    void ICoreFunctionHandler.ClosureActionDoCommand(uint obj, Action func)
    {
      if (VM.ClosureActionDoCommand(obj, nextEventId) != 0)
      {
        closures.Add(nextEventId++, func);
      }
    }

    void ICoreFunctionHandler.ClosureAssignCommand(uint obj, Action func)
    {
      if (VM.ClosureAssignCommand(obj, nextEventId) != 0)
      {
        closures.Add(nextEventId++, func);
      }
    }

    void ICoreFunctionHandler.ClosureDelayCommand(uint obj, float duration, Action func)
    {
      if (VM.ClosureDelayCommand(obj, duration, nextEventId) != 0)
      {
        closures.Add(nextEventId++, func);
      }
    }

    void ICoreService.Init() {}

    void ICoreService.Load() {}

    void ICoreService.Start() {}

    void ICoreService.Shutdown() {}

    void ICoreService.Unload() {}

    internal void OnClosure(ulong eid, uint oidSelf)
    {
      uint old = objectSelf;
      objectSelf = oidSelf;

      try
      {
        closures[eid].Invoke();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }

      closures.Remove(eid);
      objectSelf = old;
    }

    internal int OnRunScript(string script, uint oidSelf)
    {
      int retVal = 0;
      objectSelf = oidSelf;
      scriptContexts.Push(oidSelf);

      try
      {
        retVal = (int)ScriptDispatchService.TryExecuteScript(script, oidSelf);
      }
      catch (Exception e)
      {
        Log.Error(e);
      }

      scriptContexts.Pop();
      objectSelf = scriptContexts.Count == 0 ? NWScript.OBJECT_INVALID : scriptContexts.Peek();
      return retVal;
    }

    public void OnLoop(ulong _)
    {
      ServerUpdateLoopService?.Update();
    }
  }
}
