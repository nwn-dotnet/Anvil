using System;
using NLog;
using NWN.Native.API;

namespace NWN.API
{
  internal static class VirtualMachine
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static readonly CVirtualMachine vm = NWNXLib.VirtualMachine();

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
      CNWVirtualMachineCommands cmd = new CNWVirtualMachineCommands(vm.m_pCmdImplementer.Pointer, false);

      if (vm.m_nRecursionLevel++ == -1)
      {
        vm.m_cRunTimeStack.InitializeStack();
        vm.m_cRunTimeStack.m_pVMachine = vm;
        vm.m_nInstructPtrLevel = 0;
        vm.m_nInstructionsExecuted = 0;
      }

      uint[] m_oidObjectRunScript = vm.m_oidObjectRunScript;
      int[] m_bValidObjectRunScript = vm.m_bValidObjectRunScript;
      m_oidObjectRunScript[vm.m_nRecursionLevel] = oid;
      m_bValidObjectRunScript[vm.m_nRecursionLevel] = valid.ToInt();

      vm.m_oidObjectRunScript = m_oidObjectRunScript;
      vm.m_bValidObjectRunScript = m_bValidObjectRunScript;
      vm.m_pVirtualMachineScript[vm.m_nRecursionLevel].m_nScriptEventID = scriptEventId;
      cmd.m_oidObjectRunScript = vm.m_oidObjectRunScript[vm.m_nRecursionLevel];
      cmd.m_bValidObjectRunScript = vm.m_bValidObjectRunScript[vm.m_nRecursionLevel];

      return vm.m_cRunTimeStack.GetStackPointer();
    }

    public static int PopScriptContext()
    {
      CNWVirtualMachineCommands cmd = new CNWVirtualMachineCommands(vm.m_pCmdImplementer.Pointer, false);

      if (--vm.m_nRecursionLevel != -1)
      {
        cmd.m_oidObjectRunScript    = vm.m_oidObjectRunScript[vm.m_nRecursionLevel];
        cmd.m_bValidObjectRunScript = vm.m_bValidObjectRunScript[vm.m_nRecursionLevel];
      }

      return vm.m_cRunTimeStack.GetStackPointer();
    }
  }
}
