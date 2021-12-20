using System;
using System.Collections.Generic;
using Anvil.Services;
using NLog;
using NWN.Core;

namespace Anvil.Internal
{
  internal sealed class CoreInteropHandler : ICoreFunctionHandler, ICoreEventHandler, IDisposable
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();
    private readonly Dictionary<ulong, Action> closures = new Dictionary<ulong, Action>();

    private readonly Stack<uint> scriptContexts = new Stack<uint>();

    private readonly IServerLifeCycleEventHandler signalHandler;
    private ICoreLoopHandler loopHandler;
    private ulong nextEventId;

    private uint objectSelf;
    private ICoreRunScriptHandler scriptHandler;

    public CoreInteropHandler(IServerLifeCycleEventHandler signalHandler)
    {
      this.signalHandler = signalHandler;
    }

    uint ICoreFunctionHandler.ObjectSelf => objectSelf;

    public void Dispose()
    {
      scriptHandler = null;
      loopHandler = null;
    }

    public void Init(ICoreRunScriptHandler scriptHandler, ICoreLoopHandler loopHandler)
    {
      this.scriptHandler = scriptHandler;
      this.loopHandler = loopHandler;
    }

    void ICoreEventHandler.OnClosure(ulong eid, uint oidSelf)
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

    void ICoreEventHandler.OnMainLoop(ulong frame)
    {
      try
      {
        loopHandler?.OnLoop();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    int ICoreEventHandler.OnRunScript(string script, uint oidSelf)
    {
      int retVal = 0;
      objectSelf = oidSelf;
      scriptContexts.Push(oidSelf);

      try
      {
        // Ignored Scripts
        if (script == EnvironmentConfig.ModStartScript || script == EnvironmentConfig.CoreShutdownScript)
        {
          return retVal;
        }

        retVal = scriptHandler.OnRunScript(script, oidSelf);
      }
      catch (Exception e)
      {
        Log.Error(e);
      }

      scriptContexts.Pop();
      objectSelf = scriptContexts.Count == 0 ? NWScript.OBJECT_INVALID : scriptContexts.Peek();
      return retVal;
    }

    void ICoreEventHandler.OnSignal(string signal)
    {
      LifeCycleEvent eventType = signal switch
      {
        "ON_MODULE_LOAD_FINISH" => LifeCycleEvent.ModuleLoad,
        "ON_DESTROY_SERVER" => LifeCycleEvent.DestroyServer,
        "ON_DESTROY_SERVER_AFTER" => LifeCycleEvent.DestroyServerAfter,
        _ => LifeCycleEvent.Unhandled,
      };

      if (eventType == LifeCycleEvent.Unhandled)
      {
        Log.Debug("Unhandled Signal: {Signal}", signal);
      }

      signalHandler.HandleLifeCycleEvent(eventType);
    }

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
  }
}
