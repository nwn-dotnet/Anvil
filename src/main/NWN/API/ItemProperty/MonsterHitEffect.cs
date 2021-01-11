using NWN.API.Constants;

namespace NWN.API
{
  public readonly struct MonsterHitEffect
  {
    internal readonly int Property;
    internal readonly int Special;

    internal MonsterHitEffect(IPOnMonsterHit property, int special = 0)
    {
      Property = (int) property;
      Special = special;
    }

    public static MonsterHitEffect AbilityDrain(IPAbility ability) => new MonsterHitEffect(IPOnMonsterHit.AbilityDrain, (int) ability);

    public static MonsterHitEffect Confusion(IPOnHitDuration duration) => new MonsterHitEffect(IPOnMonsterHit.Confusion, (int) duration);

    public static MonsterHitEffect Disease(DiseaseType diseaseType) => new MonsterHitEffect(IPOnMonsterHit.Disease, (int) diseaseType);

    public static MonsterHitEffect Doom(IPOnHitDuration duration) => new MonsterHitEffect(IPOnMonsterHit.Doom, (int) duration);

    public static MonsterHitEffect Fear(IPOnHitDuration duration) => new MonsterHitEffect(IPOnMonsterHit.Fear, (int) duration);

    public static MonsterHitEffect Poison(PoisonType poisonType) => new MonsterHitEffect(IPOnMonsterHit.Poison, (int) poisonType);

    public static MonsterHitEffect Slow(IPOnHitDuration duration) => new MonsterHitEffect(IPOnMonsterHit.Slow, (int) duration);

    public static MonsterHitEffect Stun(IPOnHitDuration duration) => new MonsterHitEffect(IPOnMonsterHit.Stun, (int) duration);

    public static MonsterHitEffect LevelDrain(int levelDrain = 1) => new MonsterHitEffect(IPOnMonsterHit.LevelDrain, levelDrain);

    public static MonsterHitEffect Wounding(int bleedDamage) => new MonsterHitEffect(IPOnMonsterHit.Wounding, bleedDamage);
  }
}
