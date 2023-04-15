using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CExoDebugInternal
    {
      [NativeFunction("_ZN17CExoDebugInternal16WriteToErrorFileERK10CExoString", "")]
      public delegate void WriteToErrorFile(void* pExoDebugInternal, void* pMessage);

      [NativeFunction("_ZN17CExoDebugInternal14WriteToLogFileERK10CExoString", "")]
      public delegate void WriteToLogFile(void* pExoDebugInternal, void* pMessage);
    }
  }
}
