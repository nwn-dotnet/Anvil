using NWN.API.Constants;

namespace NWN.API
{
  public readonly struct HitEffect
  {
    internal readonly int Property;
    internal readonly int Special;

    internal HitEffect(IPOnHit property, int special = 0)
    {
      Property = (int) property;
      Special = special;
    }

    public static HitEffect AbilityDrain(IPAbility ability) => new HitEffect(IPOnHit.AbilityDrain, (int) ability);

    public static HitEffect Blindness(IPOnHitDuration duration) => new HitEffect(IPOnHit.Blindness, (int) duration);

    public static HitEffect Confusion(IPOnHitDuration duration) => new HitEffect(IPOnHit.Confusion, (int) duration);

    public static HitEffect Daze(IPOnHitDuration duration) => new HitEffect(IPOnHit.Daze, (int) duration);

    public static HitEffect Deafness(IPOnHitDuration duration) => new HitEffect(IPOnHit.Deafness, (int) duration);

    public static HitEffect Disease(DiseaseType diseaseType) => new HitEffect(IPOnHit.Disease, (int) diseaseType);

    public static HitEffect Doom(IPOnHitDuration duration) => new HitEffect(IPOnHit.Doom, (int) duration);

    public static HitEffect Fear(IPOnHitDuration duration) => new HitEffect(IPOnHit.Fear, (int) duration);

    public static HitEffect Hold(IPOnHitDuration duration) => new HitEffect(IPOnHit.Hold, (int) duration);

    public static HitEffect ItemPoison(IPPoisonDamage poisonType) => new HitEffect(IPOnHit.ItemPoison, (int) poisonType);

    public static HitEffect Silence(IPOnHitDuration duration) => new HitEffect(IPOnHit.Silence, (int) duration);

    public static HitEffect SlayRace(IPRacialType racialType) => new HitEffect(IPOnHit.SlayRace, (int) racialType);

    public static HitEffect SlayAlignmentGroup(IPAlignmentGroup alignmentGroup) => new HitEffect(IPOnHit.SlayAlignmentGroup, (int) alignmentGroup);

    public static HitEffect SlayAlignment(IPAlignment alignment) => new HitEffect(IPOnHit.SlayAlignment, (int) alignment);

    public static HitEffect Sleep(IPOnHitDuration duration) => new HitEffect(IPOnHit.Sleep, (int) duration);

    public static HitEffect Slow(IPOnHitDuration duration) => new HitEffect(IPOnHit.Slow, (int) duration);

    public static HitEffect Stun(IPOnHitDuration duration) => new HitEffect(IPOnHit.Stun, (int) duration);

    public static HitEffect LevelDrain(int levelDrain = 1) => new HitEffect(IPOnHit.LevelDrain, levelDrain);

    public static HitEffect Wounding(int bleedDamage) => new HitEffect(IPOnHit.Wounding, bleedDamage);

    public static HitEffect Knock() => new HitEffect(IPOnHit.Knock);

    public static HitEffect LesserDispel() => new HitEffect(IPOnHit.LesserDispel);

    public static HitEffect DispelMagic() => new HitEffect(IPOnHit.DispelMagic);

    public static HitEffect GreaterDispel() => new HitEffect(IPOnHit.GreaterDispel);

    public static HitEffect MordsDisjunction() => new HitEffect(IPOnHit.MordsDisjunction);

    public static HitEffect Vorpal() => new HitEffect(IPOnHit.Vorpal);
  }
}