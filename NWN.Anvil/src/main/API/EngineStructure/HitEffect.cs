namespace Anvil.API
{
  public readonly struct HitEffect
  {
    internal readonly int Property;
    internal readonly int Special;

    internal HitEffect(IPOnHit property, int special = 0)
    {
      Property = (int)property;
      Special = special;
    }

    public static HitEffect AbilityDrain(IPAbility ability)
    {
      return new HitEffect(IPOnHit.AbilityDrain, (int)ability);
    }

    public static HitEffect Blindness(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Blindness, (int)duration);
    }

    public static HitEffect Confusion(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Confusion, (int)duration);
    }

    public static HitEffect Daze(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Daze, (int)duration);
    }

    public static HitEffect Deafness(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Deafness, (int)duration);
    }

    public static HitEffect Disease(DiseaseType diseaseType)
    {
      return new HitEffect(IPOnHit.Disease, (int)diseaseType);
    }

    public static HitEffect Doom(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Doom, (int)duration);
    }

    public static HitEffect Fear(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Fear, (int)duration);
    }

    public static HitEffect Hold(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Hold, (int)duration);
    }

    public static HitEffect ItemPoison(IPPoisonDamage poisonType)
    {
      return new HitEffect(IPOnHit.ItemPoison, (int)poisonType);
    }

    public static HitEffect Silence(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Silence, (int)duration);
    }

    public static HitEffect SlayRace(IPRacialType racialType)
    {
      return new HitEffect(IPOnHit.SlayRace, (int)racialType);
    }

    public static HitEffect SlayAlignmentGroup(IPAlignmentGroup alignmentGroup)
    {
      return new HitEffect(IPOnHit.SlayAlignmentGroup, (int)alignmentGroup);
    }

    public static HitEffect SlayAlignment(IPAlignment alignment)
    {
      return new HitEffect(IPOnHit.SlayAlignment, (int)alignment);
    }

    public static HitEffect Sleep(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Sleep, (int)duration);
    }

    public static HitEffect Slow(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Slow, (int)duration);
    }

    public static HitEffect Stun(IPOnHitDuration duration)
    {
      return new HitEffect(IPOnHit.Stun, (int)duration);
    }

    public static HitEffect LevelDrain(int levelDrain = 1)
    {
      return new HitEffect(IPOnHit.LevelDrain, levelDrain);
    }

    public static HitEffect Wounding(int bleedDamage)
    {
      return new HitEffect(IPOnHit.Wounding, bleedDamage);
    }

    public static HitEffect Knock()
    {
      return new HitEffect(IPOnHit.Knock);
    }

    public static HitEffect LesserDispel()
    {
      return new HitEffect(IPOnHit.LesserDispel);
    }

    public static HitEffect DispelMagic()
    {
      return new HitEffect(IPOnHit.DispelMagic);
    }

    public static HitEffect GreaterDispel()
    {
      return new HitEffect(IPOnHit.GreaterDispel);
    }

    public static HitEffect MordsDisjunction()
    {
      return new HitEffect(IPOnHit.MordsDisjunction);
    }

    public static HitEffect Vorpal()
    {
      return new HitEffect(IPOnHit.Vorpal);
    }
  }
}
