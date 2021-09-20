using Newtonsoft.Json;
using NWN.Core;

namespace Anvil.API
{
  public abstract class NuiBind
  {
    [JsonProperty("bind")]
    public string Key { get; set; }
  }

  public sealed class NuiBind<T> : NuiBind
  {
    public NuiBind() {}

    public NuiBind(string key)
    {
      Key = key;
    }

    public T GetValue(NwPlayer player, int nUiToken)
    {
      string value = NWScript.JsonDump(NWScript.NuiGetBind(player.ControlledCreature, nUiToken, Key));
      return default;
    }
  }
}
