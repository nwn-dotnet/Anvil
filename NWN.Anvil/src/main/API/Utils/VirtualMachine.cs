using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Anvil.Internal;
using Anvil.Services;
using NLog;
using NWN.Core;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// Standard and Low Level methods and properties for querying/interacting with the NwScript virtual machine.
  /// </summary>
  public sealed unsafe class VirtualMachine : ICoreService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly int mainThreadId = Thread.CurrentThread.ManagedThreadId;

    private CVirtualMachine virtualMachine = null!;

    /// <summary>
    /// Gets or sets the current running script event.
    /// </summary>
    public EventScriptType CurrentRunningEvent
    {
      get => (EventScriptType)virtualMachine.m_pVirtualMachineScript[0].m_nScriptEventID;
      set => virtualMachine.m_pVirtualMachineScript[0].m_nScriptEventID = (int)value;
    }

    /// <summary>
    /// Gets or sets the instruction limit for the NWScript VM.
    /// </summary>
    public uint InstructionLimit
    {
      get => virtualMachine.m_nInstructionLimit;
      set => virtualMachine.m_nInstructionLimit = value;
    }

    public uint InstructionsExecuted
    {
      get => virtualMachine.m_nInstructionsExecuted;
      set => virtualMachine.m_nInstructionsExecuted = value;
    }

    /// <summary>
    /// Returns true if the current executing code is being executed on the main thread, and in a Virtual Machine script context.
    /// </summary>
    public bool IsInScriptContext => Thread.CurrentThread.ManagedThreadId == mainThreadId && RecursionLevel >= 0;

    public int RecursionLevel => virtualMachine.m_nRecursionLevel;

    public bool ScriptReturnValue
    {
      get
      {
        int parameterType;
        void* pParameter;

        if (virtualMachine.GetRunScriptReturnValue(&parameterType, &pParameter).ToBool() && parameterType == 3)
        {
          return (*(int*)pParameter).ToBool();
        }

        return false;
      }
    }

    /// <summary>
    /// Executes the specified NWN script.
    /// If scriptName does not specify a compiled script, nothing happens.
    /// </summary>
    public void Execute(string scriptName, NwObject? target, params (string ParamName, string ParamValue)[] scriptParams)
    {
      foreach ((string paramName, string paramValue) in scriptParams)
      {
        NWScript.SetScriptParam(paramName, paramValue);
      }

      virtualMachine.RunScript(scriptName.ToExoString(), target);
    }

    /// <summary>
    /// Executes the specified NWN script.
    /// If scriptName does not specify a compiled script, nothing happens.
    /// </summary>
    public void Execute(string scriptName, params (string ParamName, string ParamValue)[] scriptParams)
    {
      Execute(scriptName, null, scriptParams);
    }

    public void ExecuteInScriptContext(System.Action action, uint objectId = NwObject.Invalid, int scriptEventId = 0)
    {
      int spBefore = PushScriptContext(objectId, scriptEventId);
      try
      {
        action();
      }
      finally
      {
        int spAfter = PopScriptContext();
        if (spAfter != spBefore)
        {
          Log.Error("VM stack is invalid ({SpBefore} != {SpAfter}) after script context invocation: {Method}", spBefore, spAfter, action.Method.GetFullName());
        }
      }
    }

    public T ExecuteInScriptContext<T>(System.Func<T> action, uint objectId = NwObject.Invalid, int scriptEventId = 0)
    {
      int spBefore = PushScriptContext(objectId, scriptEventId);

      try
      {
        return action();
      }
      finally
      {
        int spAfter = PopScriptContext();
        if (spAfter != spBefore)
        {
          Log.Error("VM stack is invalid ({SpBefore} != {SpAfter}) after script context invocation: {Method}", spBefore, spAfter, action.Method.GetFullName());
        }
      }
    }

    /// <summary>
    /// Gets the name of the currently executing script.<br/>
    /// If depth is > 0, it will return the name of the script that called this one via ExecuteScript().
    /// </summary>
    /// <param name="depth">depth to seek the executing script.</param>
    /// <returns>The name of the currently executing script.</returns>
    public string? GetCurrentScriptName(int depth = 0)
    {
      if (virtualMachine.m_nRecursionLevel >= 0 && virtualMachine.m_nRecursionLevel >= depth)
      {
        CVirtualMachineScript script = virtualMachine.m_pVirtualMachineScript[virtualMachine.m_nRecursionLevel - depth];

        if (!script.m_sScriptName.IsEmpty().ToBool())
        {
          return script.m_sScriptName.ToString();
        }
      }

      return null;
    }

    void ICoreService.Init() {}

    void ICoreService.Load()
    {
      virtualMachine = NWNXLib.VirtualMachine();
    }

    void ICoreService.Shutdown() {}

    void ICoreService.Start() {}

    void ICoreService.Unload() {}

    internal IEnumerable<ScriptParam> GetCurrentContextScriptParams()
    {
      if (IsInScriptContext)
      {
        return virtualMachine.m_lScriptParams.GetItem(RecursionLevel);
      }

      return Enumerable.Empty<ScriptParam>();
    }

    private int PopScriptContext()
    {
      CNWVirtualMachineCommands cmd = CNWVirtualMachineCommands.FromPointer(virtualMachine.m_pCmdImplementer.Pointer);

      if (--virtualMachine.m_nRecursionLevel != -1)
      {
        cmd.m_oidObjectRunScript = virtualMachine.m_oidObjectRunScript[virtualMachine.m_nRecursionLevel];
        cmd.m_bValidObjectRunScript = virtualMachine.m_bValidObjectRunScript[virtualMachine.m_nRecursionLevel];
      }

      return virtualMachine.m_cRunTimeStack.GetStackPointer();
    }

    private int PushScriptContext(uint oid, int scriptEventId)
    {
      CNWVirtualMachineCommands cmd = CNWVirtualMachineCommands.FromPointer(virtualMachine.m_pCmdImplementer.Pointer);
      bool valid = LowLevel.ServerExoApp.GetGameObject(oid) != null;

      if (virtualMachine.m_nRecursionLevel++ == -1)
      {
        virtualMachine.m_cRunTimeStack.InitializeStack();
        virtualMachine.m_cRunTimeStack.m_pVMachine = virtualMachine;
        virtualMachine.m_nInstructPtrLevel = 0;
        virtualMachine.m_nInstructionsExecuted = 0;
      }

      virtualMachine.m_oidObjectRunScript[virtualMachine.m_nRecursionLevel] = oid;
      virtualMachine.m_bValidObjectRunScript[virtualMachine.m_nRecursionLevel] = valid.ToInt();

      virtualMachine.m_pVirtualMachineScript[virtualMachine.m_nRecursionLevel].m_nScriptEventID = scriptEventId;
      cmd.m_oidObjectRunScript = virtualMachine.m_oidObjectRunScript[virtualMachine.m_nRecursionLevel];
      cmd.m_bValidObjectRunScript = virtualMachine.m_bValidObjectRunScript[virtualMachine.m_nRecursionLevel];

      return virtualMachine.m_cRunTimeStack.GetStackPointer();
    }
  }
}
