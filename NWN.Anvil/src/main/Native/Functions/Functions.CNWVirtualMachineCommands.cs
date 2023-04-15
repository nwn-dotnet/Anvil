using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWVirtualMachineCommands
    {
      [NativeFunction("_ZN25CNWVirtualMachineCommands25ExecuteCommandPrintStringEii", "")]
      public delegate int ExecuteCommandPrintString(void* pVirtualMachineCommands, int nCommandId, int nParameters);
    }
  }
}
