using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// Area tile information
  /// </summary>
  public sealed class TileInfo
  {
    private readonly CNWTile tile;

    internal TileInfo(CNWTile tile)
    {
      this.tile = tile;
    }

    /// <summary>
    /// The tile's grid x position.
    /// </summary>
    public int GridX => tile.m_nGridX;

    /// <summary>
    /// The tile's grid y position.
    /// </summary>
    public int GridY => tile.m_nGridY;

    /// <summary>
    /// The height of the tile.
    /// </summary>
    public int Height => tile.m_nHeight;

    /// <summary>
    /// The tile's ID
    /// </summary>
    public int Id => tile.m_nID;

    /// <summary>
    /// The orientation/rotation of the tile.
    /// </summary>
    public int Orientation => tile.m_nOrientation;
  }
}
