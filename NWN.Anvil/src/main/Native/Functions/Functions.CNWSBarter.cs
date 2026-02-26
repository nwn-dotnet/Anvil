using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSBarter
    {
      [NativeFunction("_ZN10CNWSBarter15SetListAcceptedEi", "?SetListAccepted@CNWSBarter@@QEAAHH@Z")]
      public delegate int SetListAccepted(void* pBarter, int bAccepted);

      [NativeFunction("_ZN11CNWSMessage34HandlePlayerToServerBarter_AddItemEP10CNWSPlayer", )]
      public delegate void AddItem(void* pInitiator, void* pTargetPlayer, uint oidItem);
    }
  }
}


