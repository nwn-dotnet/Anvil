using System;
using Newtonsoft.Json;

namespace Anvil.API
{
  /// <summary>
  /// Represents a tile configuration at a location.
  /// </summary>
  [Serializable]
  public sealed class TileData
  {
    /// <summary>
    /// The index of the tile to be changed.
    /// </summary>
    /// <remarks>
    /// For example, a 3x3 area has the following tile indexes:<br/>
    /// 6 7 8<br/>
    /// 3 4 5<br/>
    /// 0 1 2<br/>
    /// </remarks>
    [JsonProperty("index")]
    public int Index { get; set; }

    /// <summary>
    /// The new tile ID to assign.
    /// </summary>
    [JsonProperty("tileid")]
    public int TileId { get; set; }

    /// <summary>
    /// The rotation of the new tile.
    /// </summary>
    [JsonProperty("orientation")]
    public TileRotation Orientation { get; set; }

    /// <summary>
    /// The height of the new tile.
    /// </summary>
    [JsonProperty("height")]
    public int Height { get; set; }

    /// <summary>
    /// The animation state of the new tile (1/0).
    /// </summary>
    [JsonProperty("animloop1")]
    public int AnimationLoop1 { get; set; }

    /// <summary>
    /// The animation state of the new tile (1/0).
    /// </summary>
    [JsonProperty("animloop2")]
    public int AnimationLoop2 { get; set; }

    /// <summary>
    /// The animation state of the new tile (1/0).
    /// </summary>
    [JsonProperty("animloop3")]
    public int AnimationLoop3 { get; set; }
  }
}
