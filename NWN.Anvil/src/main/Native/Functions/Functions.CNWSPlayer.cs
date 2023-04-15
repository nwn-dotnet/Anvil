using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CNWSPlayer
    {
      [NativeFunction("_ZN10CNWSPlayer8DropTURDEv", "?DropTURD@CNWSPlayer@@QEAAXXZ")]
      public delegate void DropTURD(void* pPlayer);

      [NativeFunction("_ZN10CNWSPlayer7EatTURDEP14CNWSPlayerTURD", "?EatTURD@CNWSPlayer@@QEAAXPEAVCNWSPlayerTURD@@@Z")]
      public delegate void EatTURD(void* pPlayer, void* pTURD);

      [NativeFunction("_ZN10CNWSPlayer19SaveServerCharacterEi", "?SaveServerCharacter@CNWSPlayer@@QEAAHH@Z")]
      public delegate int SaveServerCharacter(void* pPlayer, int bBackupPlayer);

      [NativeFunction("_ZN10CNWSPlayer17ValidateCharacterEPi", "?ValidateCharacter@CNWSPlayer@@QEAAIPEAH@Z")]
      public delegate int ValidateCharacter(void* pPlayer, int* bFailedServerRestriction);
    }
  }
}
