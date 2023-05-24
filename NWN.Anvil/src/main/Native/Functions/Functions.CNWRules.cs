using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWRules
    {
      [NativeFunction("_ZN8CNWRules9ReloadAllEv", "?ReloadAll@CNWRules@@QEAAXXZ")]
      public delegate void ReloadAll(void* pRules);
    }
  }
}
