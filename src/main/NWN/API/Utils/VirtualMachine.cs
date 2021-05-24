using System;
using NLog;
using NWN.Core;
using NWN.Native.API;

namespace NWN.API
{
  public sealed class VirtualMachine
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static readonly VirtualMachine Instance = new VirtualMachine(NWNXLib.VirtualMachine());

    private readonly CVirtualMachine virtualMachine;

    internal VirtualMachine(CVirtualMachine virtualMachine)
    {
      this.virtualMachine = virtualMachine;
    }

    public uint InstructionsExecuted
    {
      get => virtualMachine.m_nInstructionsExecuted;
      set => virtualMachine.m_nInstructionsExecuted = value;
    }

    /// <summary>
    /// Gets or sets the instruction limit for the NWScript VM.
    /// </summary>
    public uint InstructionLimit
    {
      get => virtualMachine.m_nInstructionLimit;
      set => virtualMachine.m_nInstructionLimit = value;
    }

    public bool IsInScriptContext
    {
      get => virtualMachine.m_nRecursionLevel >= 0;
    }

    public unsafe bool ScriptReturnValue
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
    /// Gets the name of the currently executing script.<br/>
    /// If depth is > 0, it will return the name of the script that called this one via ExecuteScript().
    /// </summary>
    /// <param name="depth">depth to seek the executing script.</param>
    /// <returns>The name of the currently executing script.</returns>
    public string GetCurrentScriptName(int depth = 0)
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

    /// <summary>
    /// Executes the specified NWN script.
    /// If scriptName does not specify a compiled script, nothing happens.
    /// </summary>
    public void Execute(string scriptName, NwObject target, params (string ParamName, string ParamValue)[] scriptParams)
    {
      foreach ((string ParamName, string ParamValue) scriptParam in scriptParams)
      {
        NWScript.SetScriptParam(scriptParam.ParamName, scriptParam.ParamValue);
      }

      NWScript.ExecuteScript(scriptName, target);
    }

    /// <summary>
    /// Executes the specified NWN script.
    /// If scriptName does not specify a compiled script, nothing happens.
    /// </summary>
    public void Execute(string scriptName, params (string ParamName, string ParamValue)[] scriptParams)
      => Execute(scriptName, null, scriptParams);

    public void ExecuteInScriptContext(Action action, uint objectId = NwObject.INVALID)
    {
      int spBefore = PushScriptContext(objectId, 0, false);
      try
      {
        action();
      }
      finally
      {
        int spAfter = PopScriptContext();
        if (spAfter != spBefore)
        {
          Log.Error($"VM stack is invalid ({spBefore} != {spAfter}) after script context invocation: {action.Method.GetFullName()}");
        }
      }
    }

    private int PushScriptContext(uint oid, int scriptEventId, bool valid)
    {
      CNWVirtualMachineCommands cmd = CNWVirtualMachineCommands.FromPointer(virtualMachine.m_pCmdImplementer.Pointer);

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
  }
}
