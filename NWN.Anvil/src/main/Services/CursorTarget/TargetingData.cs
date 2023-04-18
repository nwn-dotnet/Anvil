using System.Numerics;
using Anvil.API;

namespace Anvil.Services
{
  /// <summary>
  /// Configuration options for the target helper. See <see cref="TargetModeSettings.TargetingData"/>, <see cref="NwPlayer.SetSpellTargetingData"/>.
  /// </summary>
  public sealed class TargetingData
  {
    /// <summary>
    /// The shape of the target helper.
    /// </summary>
    public SpellTargetingShape Shape { get; set; }

    /// <summary>
    /// Flags and options for the target helper.
    /// </summary>
    public SpellTargetingFlags Flags { get; set; }

    /// <summary>
    /// The spell associated with the target helper.
    /// </summary>
    public NwSpell? Spell { get; set; }

    /// <summary>
    /// The feat associated with the target helper.
    /// </summary>
    public NwFeat? Feat { get; set; }

    /// <summary>
    /// The size of the target helper.
    /// </summary>
    public Vector2 Size { get; set; }

    /// <summary>
    /// The range of the target helper.
    /// </summary>
    public float Range { get; set; }
  }
}
