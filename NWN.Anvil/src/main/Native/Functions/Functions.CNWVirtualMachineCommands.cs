using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CVirtualMachineCmdImplementer
    {
      [NativeFunction("_ZN29CVirtualMachineCmdImplementer25ExecuteCommandPrintStringEii", "?ExecuteCommandPrintString@CVirtualMachineCmdImplementer@@QEAAHHH@Z")]
      public delegate int ExecuteCommandPrintString(void* pVirtualMachineCommands, int nCommandId, int nParameters);
    }
  }
}
