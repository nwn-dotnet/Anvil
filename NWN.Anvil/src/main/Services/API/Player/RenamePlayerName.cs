using NWN.Native.API;

namespace Anvil.Services
{
  internal class RenamePlayerName
  {
    public CExoString DisplayName { get; set; }
    public CExoString OverrideName { get; set; }
    public PlayerNameState PlayerNameState { get; set; }
  }
}
