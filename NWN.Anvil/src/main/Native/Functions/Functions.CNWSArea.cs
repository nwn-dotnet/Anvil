using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSArea
    {
      [NativeFunction("_ZN8CNWSAreaD1Ev", "??1CNWSArea@@UEAA@XZ")]
      public delegate void Destructor(void* pArea);
    }
  }
}
