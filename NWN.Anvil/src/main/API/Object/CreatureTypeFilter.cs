namespace Anvil.API
{
  public readonly struct CreatureTypeFilter
  {
    public static readonly CreatureTypeFilter None = new CreatureTypeFilter(CreatureType.None, -1);
    internal readonly int Key;
    internal readonly int Value;

    internal CreatureTypeFilter(CreatureType key, int value)
    {
      Key = (int)key;
      Value = value;
    }

    public static CreatureTypeFilter Alive(bool alive)
    {
      return new CreatureTypeFilter(CreatureType.IsAlive, alive.ToInt());
    }

    public static CreatureTypeFilter Class(ClassType classType)
    {
      return new CreatureTypeFilter(CreatureType.Class, (int)classType);
    }

    public static CreatureTypeFilter DoesNotHaveSpellEffect(Spell spellEffect)
    {
      return new CreatureTypeFilter(CreatureType.DoesNotHaveSpellEffect, (int)spellEffect);
    }

    public static CreatureTypeFilter HasSpellEffect(Spell spellEffect)
    {
      return new CreatureTypeFilter(CreatureType.HasSpellEffect, (int)spellEffect);
    }

    public static CreatureTypeFilter Perception(PerceptionType perceptionType)
    {
      return new CreatureTypeFilter(CreatureType.Perception, (int)perceptionType);
    }

    public static CreatureTypeFilter PlayerChar(bool isPc)
    {
      return new CreatureTypeFilter(CreatureType.PlayerChar, isPc.ToInt());
    }

    public static CreatureTypeFilter RacialType(RacialType racialType)
    {
      return new CreatureTypeFilter(CreatureType.RacialType, (int)racialType);
    }

    public static CreatureTypeFilter Reputation(ReputationType reputationType)
    {
      return new CreatureTypeFilter(CreatureType.Reputation, (int)reputationType);
    }
  }
}
