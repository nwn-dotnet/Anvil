using System;
using NLog;
using NWN.Native.API;

namespace NWN.API
{
  internal static class VirtualMachine
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static readonly CVirtualMachine VM = NWNXLib.VirtualMachine();

    public static void ExecuteInScriptContext(Action action, uint objectId = NwObject.INVALID)
    {
      int spBefore = PushScriptContext(NwObject.INVALID, 0, false);
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

    public static int PushScriptContext(uint oid, int scriptEventId, bool valid)
    {
      CNWVirtualMachineCommands cmd = new CNWVirtualMachineCommands(VM.m_pCmdImplementer.Pointer, false);

      if (VM.m_nRecursionLevel++ == -1)
      {
        VM.m_cRunTimeStack.InitializeStack();
        VM.m_cRunTimeStack.m_pVMachine = VM;
        VM.m_nInstructPtrLevel = 0;
        VM.m_nInstructionsExecuted = 0;
      }

      uint[] m_oidObjectRunScript = VM.m_oidObjectRunScript;
      int[] m_bValidObjectRunScript = VM.m_bValidObjectRunScript;
      m_oidObjectRunScript[VM.m_nRecursionLevel] = oid;
      m_bValidObjectRunScript[VM.m_nRecursionLevel] = valid.ToInt();

      VM.m_oidObjectRunScript = m_oidObjectRunScript;
      VM.m_bValidObjectRunScript = m_bValidObjectRunScript;
      VM.m_pVirtualMachineScript[VM.m_nRecursionLevel].m_nScriptEventID = scriptEventId;
      cmd.m_oidObjectRunScript = VM.m_oidObjectRunScript[VM.m_nRecursionLevel];
      cmd.m_bValidObjectRunScript = VM.m_bValidObjectRunScript[VM.m_nRecursionLevel];

      return VM.m_cRunTimeStack.GetStackPointer();
    }

    public static int PopScriptContext()
    {
      CNWVirtualMachineCommands cmd = new CNWVirtualMachineCommands(VM.m_pCmdImplementer.Pointer, false);

      if (--VM.m_nRecursionLevel != -1)
      {
        cmd.m_oidObjectRunScript = VM.m_oidObjectRunScript[VM.m_nRecursionLevel];
        cmd.m_bValidObjectRunScript = VM.m_bValidObjectRunScript[VM.m_nRecursionLevel];
      }

      return VM.m_cRunTimeStack.GetStackPointer();
    }
  }
}
