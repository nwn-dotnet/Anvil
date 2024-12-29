using System.Collections.Generic;
using Newtonsoft.Json;
using NWN.Core;

namespace Anvil.API
{
  [method: JsonConstructor]
  public sealed class NuiBindStrRef(string key) : NuiProperty<string>
  {
    [JsonProperty("bind")]
    public string Key { get; init; } = key;

    /// <summary>
    /// Queries the specified player for the value of this binding.
    /// </summary>
    /// <param name="player">The player to query.</param>
    /// <param name="uiToken">The associated UI token.</param>
    /// <returns>The current value of the binding.</returns>
    public StrRef? GetBindValue(NwPlayer player, int uiToken)
    {
      return JsonUtility.FromJson<StrRef?>(NWScript.NuiGetBind(player.ControlledCreature, uiToken, Key));
    }

    /// <summary>
    /// Queries the specified player for the array of values assigned to this binding.
    /// </summary>
    /// <param name="player">The player to query.</param>
    /// <param name="uiToken">The associated UI token.</param>
    /// <returns>The current values of the binding.</returns>
    public List<StrRef?>? GetBindValues(NwPlayer player, int uiToken)
    {
      return JsonUtility.FromJson<List<StrRef?>>(NWScript.NuiGetBind(player.ControlledCreature, uiToken, Key));
    }

    /// <summary>
    /// Assigns a value to the binding for the specified player.
    /// </summary>
    /// <param name="player">The player whose binding will be updated.</param>
    /// <param name="uiToken">The unique UI token to be updated.</param>
    /// <param name="value">The new value to assign.</param>
    public void SetBindValue(NwPlayer player, int uiToken, StrRef? value)
    {
      NWScript.NuiSetBind(player.ControlledCreature, uiToken, Key, JsonUtility.ToJsonStructure(value));
    }

    /// <summary>
    /// Assigns an array of values to the binding for the specified player.
    /// </summary>
    /// <param name="player">The player whose binding will be updated.</param>
    /// <param name="uiToken">The unique UI token to be updated.</param>
    /// <param name="values">The new value to assign.</param>
    public void SetBindValues(NwPlayer player, int uiToken, IEnumerable<StrRef?> values)
    {
      NWScript.NuiSetBind(player.ControlledCreature, uiToken, Key, JsonUtility.ToJsonStructure(values));
    }

    /// <summary>
    /// Marks this property as watched/un-watched.<br/>
    /// A watched bind will invoke the NUI script event every time its value changes.
    /// </summary>
    /// <param name="player">The player whose binding will be updated.</param>
    /// <param name="uiToken">The unique UI token to be updated.</param>
    /// <param name="watch">True if the value should be watched, false if it should not be watched.</param>
    public void SetBindWatch(NwPlayer player, int uiToken, bool watch)
    {
      NWScript.NuiSetBindWatch(player.ControlledCreature, uiToken, Key, watch.ToInt());
    }
  }
}
