namespace Anvil.API
{
  /// <summary>
  /// Represents an "On Hit" item property effect, for use with <see cref="ItemProperty.OnHitEffect"/>.
  /// </summary>
  public readonly struct HitEffect
  {
    internal readonly int Property;
    internal readonly int Special;

    internal HitEffect(IPOnHit property, int special = 0)
    {
      Property = (int)property;
      Special = special;
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that drains the specified ability score on a successful hit.
    /// </summary>
    /// <param name="ability">The ability score to drain.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect AbilityDrain(IPAbility ability)
    {
      return new HitEffect(IPOnHit.AbilityDrain, (int)ability);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a blind effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Blindness(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Blindness, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a confusion effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Confusion(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Confusion, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a daze effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Daze(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Daze, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a deafness effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Deafness(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Deafness, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a disease effect on a successful hit.
    /// </summary>
    /// <param name="diseaseType">The disease type to apply.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Disease(DiseaseType diseaseType)
    {
      return new HitEffect(IPOnHit.Disease, (int)diseaseType);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that dispels magical effects on a successful hit.
    /// </summary>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect DispelMagic()
    {
      return new HitEffect(IPOnHit.DispelMagic);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a doom effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Doom(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Doom, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a fear effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Fear(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Fear, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that dispels magical effects on a successful hit.
    /// </summary>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect GreaterDispel()
    {
      return new HitEffect(IPOnHit.GreaterDispel);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a hold effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Hold(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Hold, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a poison effect on a successful hit.
    /// </summary>
    /// <param name="poisonType">The poison type to apply.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect ItemPoison(IPPoisonDamage poisonType)
    {
      return new HitEffect(IPOnHit.ItemPoison, (int)poisonType);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a knock effect on a successful hit.
    /// </summary>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Knock()
    {
      return new HitEffect(IPOnHit.Knock);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that dispels magical effects on a successful hit.
    /// </summary>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect LesserDispel()
    {
      return new HitEffect(IPOnHit.LesserDispel);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a level drain effect on a successful hit.
    /// </summary>
    /// <param name="levelDrain">The amount of levels to drain per hit.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect LevelDrain(int levelDrain = 1)
    {
      return new HitEffect(IPOnHit.LevelDrain, levelDrain);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that dispels magical effects on a successful hit.
    /// </summary>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect MordsDisjunction()
    {
      return new HitEffect(IPOnHit.MordsDisjunction);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a silence effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Silence(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Silence, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that instantly kills a creature of the specified alignment on a successful hit.
    /// </summary>
    /// <param name="alignment">The alignment that will be slain.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect SlayAlignment(IPAlignment alignment)
    {
      return new HitEffect(IPOnHit.SlayAlignment, (int)alignment);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that instantly kills a creature of the specified alignment group on a successful hit.
    /// </summary>
    /// <param name="alignmentGroup">The alignment group that will be slain.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect SlayAlignmentGroup(IPAlignmentGroup alignmentGroup)
    {
      return new HitEffect(IPOnHit.SlayAlignmentGroup, (int)alignmentGroup);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that instantly kills a creature of the specified race on a successful hit.
    /// </summary>
    /// <param name="racialType">The racial type that will be slain.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect SlayRace(IPRacialType racialType)
    {
      return new HitEffect(IPOnHit.SlayRace, (int)racialType);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a sleep effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Sleep(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Sleep, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a slow effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Slow(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Slow, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a stun effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Stun(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Stun, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that instantly kills a creature on a critical hit.
    /// </summary>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Vorpal()
    {
      return new HitEffect(IPOnHit.Vorpal);
    }

    /// <summary>
    /// Creates a <see cref="HitEffect"/> property that applies a bleeding damage over time effect until the creature is healed.
    /// </summary>
    /// <param name="bleedDamage">The damage to apply per round.</param>
    /// <returns>The created HitEffect.</returns>
    public static HitEffect Wounding(int bleedDamage)
    {
      return new HitEffect(IPOnHit.Wounding, bleedDamage);
    }
  }
}
