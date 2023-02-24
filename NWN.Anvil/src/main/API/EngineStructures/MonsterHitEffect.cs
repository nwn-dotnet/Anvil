namespace Anvil.API
{
  public readonly struct MonsterHitEffect
  {
    internal readonly int Property;
    internal readonly int Special;

    internal MonsterHitEffect(IPOnMonsterHit property, int special = 0)
    {
      Property = (int)property;
      Special = special;
    }

    public static MonsterHitEffect AbilityDrain(IPAbility ability)
    {
      return new MonsterHitEffect(IPOnMonsterHit.AbilityDrain, (int)ability);
    }

    public static MonsterHitEffect Confusion(IPOnHitDuration duration)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Confusion, (int)duration);
    }

    public static MonsterHitEffect Disease(DiseaseType diseaseType)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Disease, (int)diseaseType);
    }

    public static MonsterHitEffect Doom(IPOnHitDuration duration)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Doom, (int)duration);
    }

    public static MonsterHitEffect Fear(IPOnHitDuration duration)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Fear, (int)duration);
    }

    public static MonsterHitEffect LevelDrain(int levelDrain = 1)
    {
      return new MonsterHitEffect(IPOnMonsterHit.LevelDrain, levelDrain);
    }

    public static MonsterHitEffect Poison(PoisonType poisonType)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Poison, (int)poisonType);
    }

    public static MonsterHitEffect Slow(IPOnHitDuration duration)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Slow, (int)duration);
    }

    public static MonsterHitEffect Stun(IPOnHitDuration duration)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Stun, (int)duration);
    }

    public static MonsterHitEffect Wounding(int bleedDamage)
    {
      return new MonsterHitEffect(IPOnMonsterHit.Wounding, bleedDamage);
    }
  }
}
