using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSDoor
    {
      [NativeFunction("_ZN8CNWSDoor12SetOpenStateEh", "")]
      public delegate void SetOpenState(void* pDoor, byte nOpenState);
    }
  }
}
