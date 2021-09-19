using Newtonsoft.Json;
using NWN.Core;

namespace Anvil.API
{
  public sealed class NuiBind<T>
  {
    [JsonProperty("bind")]
    public string Key { get; set; }

    public T GetValue(NwPlayer player, int nUiToken)
    {
      string value = NWScript.JsonDump(NWScript.NuiGetBind(player.ControlledCreature, nUiToken, Key));
      return default;
    }
  }
}
