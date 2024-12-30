using Anvil.API;
using NWN.Native.API;

namespace Anvil.Services
{
  public sealed class PlayerNameOverride(string characterName, string playerName = "Someone")
  {
    /// <summary>
    /// Gets the character name used for this override.
    /// </summary>
    public string CharacterName { get; } = characterName;

    /// <summary>
    /// Gets the player/community name used for this override.
    /// </summary>
    public string PlayerName { get; } = playerName;

    internal CExoString CharacterNameInternal { get; } = characterName.ToExoString();

    internal CExoString PlayerNameInternal { get; } = playerName.ToExoString();
  }
}
