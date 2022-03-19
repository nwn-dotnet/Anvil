using Anvil.API;
using NWN.Native.API;

namespace Anvil.Services
{
  public sealed class PlayerNameOverride
  {
    public PlayerNameOverride(string characterName, string playerName = "Someone")
    {
      CharacterName = characterName;
      PlayerName = playerName;

      CharacterNameInternal = characterName.ToExoString();
      PlayerNameInternal = playerName.ToExoString();
    }

    /// <summary>
    /// Gets the character name used for this override.
    /// </summary>
    public string CharacterName { get; }

    /// <summary>
    /// Gets the player/community name used for this override.
    /// </summary>
    public string PlayerName { get; }

    internal CExoString CharacterNameInternal { get; }

    internal CExoString PlayerNameInternal { get; }
  }
}
