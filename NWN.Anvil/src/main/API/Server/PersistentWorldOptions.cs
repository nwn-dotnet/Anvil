using NWN.Native.API;

namespace Anvil.API
{
  public sealed class PersistentWorldOptions
  {
    private readonly CPersistantWorldOptions persistantWorldOptions;

    internal PersistentWorldOptions(CPersistantWorldOptions persistantWorldOptions)
    {
      this.persistantWorldOptions = persistantWorldOptions;
    }

    public bool VaultCharactersOnly
    {
      get => persistantWorldOptions.bVaultCharsOnly.ToBool();
    }

    public bool SaveCharactersInSaveGame
    {
      get => persistantWorldOptions.bSaveCharsInSaveGame.ToBool();
    }

    public bool SuppressBaseServerVault
    {
      get => persistantWorldOptions.bSuppressBaseServerVault.ToBool();
    }

    public bool StickyPlayerNames
    {
      get => persistantWorldOptions.bStickyPlayerNames.ToBool();
    }

    public bool ServerVaultByPlayerName
    {
      get => persistantWorldOptions.bServerVaultByPlayerName.ToBool();
    }
  }
}
