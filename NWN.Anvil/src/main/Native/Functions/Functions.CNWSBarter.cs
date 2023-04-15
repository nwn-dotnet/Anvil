using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSBarter
    {
      [NativeFunction("_ZN10CNWSBarter15SetListAcceptedEi", "")]
      public delegate int SetListAccepted(void* pBarter, int bAccepted);
    }
  }
}
