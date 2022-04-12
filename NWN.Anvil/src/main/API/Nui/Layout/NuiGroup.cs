using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// A group, usually with a border and some padding, holding a single element. Can scroll.<br/>
  /// Will not advise parent of size, so you need to let it fill a span (col/row) as if it was a element.
  /// </summary>
  public sealed class NuiGroup : NuiLayout
  {
    [JsonProperty("border")]
    public bool Border { get; set; } = true;

    [JsonIgnore]
    public NuiLayout Layout { get; set; }

    [JsonProperty("scrollbars")]
    public NuiScrollbars Scrollbars { get; set; } = NuiScrollbars.Auto;

    public override string Type { get; } = "group";

    protected override IEnumerable<NuiElement> SerializedChildren => Layout.SafeYield();

    /// <summary>
    /// Sets the group layout for a specific player + window token (override/partial update).<br/>
    /// </summary>
    /// <param name="player">The player with the window containing this group.</param>
    /// <param name="token">The token of the window to update.</param>
    /// <param name="newLayout">The new layout to apply to this group.</param>
    /// <exception cref="InvalidOperationException">Thrown if this group does not have an Id assigned.</exception>
    public void SetLayout(NwPlayer player, int token, NuiLayout newLayout)
    {
      if (string.IsNullOrEmpty(Id))
      {
        throw new InvalidOperationException("Layout cannot be updated as the NuiGroup does not have an ID.");
      }

      NWScript.NuiSetGroupLayout(player.ControlledCreature, token, Id, JsonUtility.ToJsonStructure(newLayout));
    }
  }
}
