namespace Anvil.API
{
  public sealed class SurfaceMaterialTableEntry : ITwoDimArrayEntry
  {
    private const int MaxActions = 8;

    /// <summary>
    /// Gets the human readable name of the surface material. Otherwise unused.
    /// </summary>
    public string? Label { get; private set; }

    /// <summary>
    /// Gets if the player may walk on this part of the walkmesh.
    /// </summary>
    public bool? Walk { get; private set; }

    public bool? WalkCheck { get; private set; }

    /// <summary>
    /// Gets if the material blocks Line of Sight. For instance Grass will but Transparent will not.
    /// </summary>
    public bool? LineOfSight { get; private set; }

    public string? Sound { get; private set; }

    /// <summary>
    /// Gets the column name prefix for the footsteps table which defines the noise made when walking on the surface.
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// Gets if this material is considered water.
    /// </summary>
    public bool? IsWater { get; private set; }

    /// <summary>
    /// Gets the effect to place at a creature's feet when walking through this material.
    /// </summary>
    public string? Visual { get; private set; }

    /// <summary>
    /// Gets the list of string references used to show actions in the radial menu when right clicking this material.
    /// </summary>
    public string?[] ActionStrRefs { get; } = new string?[MaxActions];

    /// <summary>
    /// Gets the list of icons used to show actions in the radial menu when right clicking this material.
    /// </summary>
    public string?[] ActionIcons { get; } = new string?[MaxActions];

    public int RowIndex { get; init; }

    public void InterpretEntry(TwoDimArrayEntry entry)
    {
      Label = entry.GetString("Label");
      Walk = entry.GetBool("Walk");
      WalkCheck = entry.GetBool("WalkCheck");
      LineOfSight = entry.GetBool("LineOfSight");
      Sound = entry.GetString("Sound");
      Name = entry.GetString("Name");
      IsWater = entry.GetBool("IsWater");
      Visual = entry.GetString("Visual");

      for (int i = 0; i < MaxActions; i++)
      {
        ActionStrRefs[i] = entry.GetString($"Act{i}_Strref");
        ActionIcons[i] = entry.GetString($"Act{i}_Icon");
      }
    }
  }
}
