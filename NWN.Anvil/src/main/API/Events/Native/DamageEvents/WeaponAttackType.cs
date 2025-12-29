namespace Anvil.API.Events
{
  /// <summary>
  /// Categorizes the weapon or natural attack slot used for a strike.
  /// </summary>
  public enum WeaponAttackType
  {
    /// <summary>
    /// Unknown attack type.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Main-hand weapon attack.
    /// </summary>
    MainHand = 1,

    /// <summary>
    /// Off-hand weapon attack.
    /// </summary>
    Offhand = 2,

    /// <summary>
    /// Natural attack (left).
    /// </summary>
    CreatureLeft = 3,

    /// <summary>
    /// Natural attack (right).
    /// </summary>
    CreatureRight = 4,

    /// <summary>
    /// Natural attack (bite).
    /// </summary>
    CreatureBite = 5,

    /// <summary>
    /// Extra attack granted by the haste effect.
    /// </summary>
    HastedAttack = 6,

    /// <summary>
    /// Unarmed attack.
    /// </summary>
    Unarmed = 7,

    /// <summary>
    /// Additional unarmed attack.
    /// </summary>
    UnarmedExtra = 8,
  }
}
