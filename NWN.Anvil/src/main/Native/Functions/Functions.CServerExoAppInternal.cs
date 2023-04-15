using Anvil.Services;

// ReSharper disable InconsistentNaming
namespace Anvil.Native
{
  internal static unsafe partial class Functions
  {
    public static class CServerExoAppInternal
    {
      [NativeFunction("_ZN21CServerExoAppInternal19LoadCharacterFinishEP10CNWSPlayerii", "")]
      public delegate int LoadCharacterFinish(void* pServerExoAppInternal, void* pPlayer, int bUseSaveGameCharacter, int bUseStateDataInSaveGame);

      [NativeFunction("_ZN21CServerExoAppInternal17RemovePCFromWorldEP10CNWSPlayer", "")]
      public delegate void RemovePCFromWorld(void* pServerExoAppInternal, void* pPlayer);
    }
  }
}
