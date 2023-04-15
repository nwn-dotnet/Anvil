using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSUUID
    {
      [NativeFunction("_ZN8CNWSUUID11LoadFromGffEP7CResGFFP10CResStruct", "")]
      public delegate int LoadFromGff(void* pUUID, void* pRes, void* pStruct);

      [NativeFunction("_ZN8CNWSUUID9SaveToGffEP7CResGFFP10CResStruct", "")]
      public delegate void SaveToGff(void* pUUID, void* pRes, void* pStruct);
    }
  }
}
