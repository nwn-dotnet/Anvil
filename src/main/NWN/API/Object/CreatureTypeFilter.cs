using NWN.API.Constants;

namespace NWN.API
{
  public struct CreatureTypeFilter
  {
    internal readonly int Key;
    internal readonly int Value;

    internal CreatureTypeFilter(CreatureType key, int value)
    {
      this.Key = (int) key;
      this.Value = value;
    }


    public static readonly CreatureTypeFilter None = new CreatureTypeFilter(CreatureType.None, -1);

    public static CreatureTypeFilter RacialType(RacialType racialType) => new CreatureTypeFilter(CreatureType.RacialType, (int) racialType);
    public static CreatureTypeFilter PlayerChar(bool isPc) => new CreatureTypeFilter(CreatureType.PlayerChar, isPc.ToInt());
    public static CreatureTypeFilter Class(ClassType classType) => new CreatureTypeFilter(CreatureType.Class, (int) classType);
    public static CreatureTypeFilter Reputation(ReputationType reputationType) => new CreatureTypeFilter(CreatureType.Reputation, (int) reputationType);
    public static CreatureTypeFilter Alive(bool alive) => new CreatureTypeFilter(CreatureType.IsAlive, alive.ToInt());
    public static CreatureTypeFilter HasSpellEffect(Spell spellEffect) => new CreatureTypeFilter(CreatureType.HasSpellEffect, (int) spellEffect);
    public static CreatureTypeFilter DoesNotHaveSpellEffect(Spell spellEffect) => new CreatureTypeFilter(CreatureType.DoesNotHaveSpellEffect, (int) spellEffect);
    public static CreatureTypeFilter Perception(PerceptionType perceptionType) => new CreatureTypeFilter(CreatureType.Perception, (int) perceptionType);
  }
}