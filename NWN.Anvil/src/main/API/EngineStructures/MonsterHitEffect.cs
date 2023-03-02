namespace Anvil.API
{
  /// <summary>
  /// Represents a monster "On Hit" item property effect, for use with <see cref="ItemProperty.OnMonsterHitProperties"/>.
  /// </summary>
  public readonly struct MonsterHitEffect
  {
    internal readonly int Property;
    internal readonly int Special;

    internal MonsterHitEffect(IPOnMonsterHit property, int special = 0)
    {
      Property = (int)property;
      Special = special;
    }

    /// <summary>
    /// Creates a <see cref="MonsterHitEffect"/> property that drains the specified ability score on a successful hit.
    /// </summary>
    /// <param name="ability">The ability score to drain.</param>
    /// <returns>The created HitEffect.</returns>
    public static MonsterHitEffect AbilityDrain(IPAbility ability)
    {
      return new MonsterHitEffect(IPOnMonsterHit.AbilityDrain, (int)ability);
    }

    /// <summary>
    /// Creates a <see cref="MonsterHitEffect"/> property that applies a confusion effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static MonsterHitEffect Confusion(IPOnHitDuration duration)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Confusion, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="MonsterHitEffect"/> property that applies a disease effect on a successful hit.
    /// </summary>
    /// <param name="diseaseType">The disease type to apply.</param>
    /// <returns>The created HitEffect.</returns>
    public static MonsterHitEffect Disease(DiseaseType diseaseType)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Disease, (int)diseaseType);
    }

    /// <summary>
    /// Creates a <see cref="MonsterHitEffect"/> property that applies a doom effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static MonsterHitEffect Doom(IPOnHitDuration duration)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Doom, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="MonsterHitEffect"/> property that applies a fear effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static MonsterHitEffect Fear(IPOnHitDuration duration)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Fear, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="MonsterHitEffect"/> property that applies a level drain effect on a successful hit.
    /// </summary>
    /// <param name="levelDrain">The amount of levels to drain per hit.</param>
    /// <returns>The created HitEffect.</returns>
    public static MonsterHitEffect LevelDrain(int levelDrain = 1)
    {
      return new MonsterHitEffect(IPOnMonsterHit.LevelDrain, levelDrain);
    }

    /// <summary>
    /// Creates a <see cref="MonsterHitEffect"/> property that applies a poison effect on a successful hit.
    /// </summary>
    /// <param name="poisonType">The poison type to apply.</param>
    /// <returns>The created HitEffect.</returns>
    public static MonsterHitEffect Poison(PoisonType poisonType)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Poison, (int)poisonType);
    }

    /// <summary>
    /// Creates a <see cref="MonsterHitEffect"/> property that applies a slow effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static MonsterHitEffect Slow(IPOnHitDuration duration)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Slow, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="MonsterHitEffect"/> property that applies a stun effect on a successful hit.
    /// </summary>
    /// <param name="duration">The duration of the effect.</param>
    /// <returns>The created HitEffect.</returns>
    public static MonsterHitEffect Stun(IPOnHitDuration duration)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Stun, (int)duration);
    }

    /// <summary>
    /// Creates a <see cref="MonsterHitEffect"/> property that applies a bleeding damage over time effect until the creature is healed.
    /// </summary>
    /// <param name="bleedDamage">The damage to apply per round.</param>
    /// <returns>The created HitEffect.</returns>
    public static MonsterHitEffect Wounding(int bleedDamage)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Wounding, bleedDamage);
    }
  }
}
