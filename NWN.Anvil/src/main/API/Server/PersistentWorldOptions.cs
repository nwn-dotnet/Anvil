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

    public bool SaveCharactersInSaveGame => persistantWorldOptions.bSaveCharsInSaveGame.ToBool();

    public bool ServerVaultByPlayerName => persistantWorldOptions.bServerVaultByPlayerName.ToBool();

    public bool StickyPlayerNames => persistantWorldOptions.bStickyPlayerNames.ToBool();

    public bool SuppressBaseServerVault => persistantWorldOptions.bSuppressBaseServerVault.ToBool();

    public bool VaultCharactersOnly => persistantWorldOptions.bVaultCharsOnly.ToBool();
  }
}
