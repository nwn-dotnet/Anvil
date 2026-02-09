namespace Anvil.API
{
  /// <summary>
  /// Represents a composable filter used to match creatures by a specific criterion (alive state, class, race, spell effects, perception, player control, or reputation).
  /// </summary>
  /// <remarks>
  /// These filters encode a filter type and its parameter and are typically combined with object query APIs to narrow results.
  /// </remarks>
  public readonly struct CreatureTypeFilter
  {
    /// <summary>
    /// Represents no filtering, useful as a placeholder when a filter is optional.
    /// </summary>
    public static readonly CreatureTypeFilter None = new CreatureTypeFilter(CreatureType.None, -1);
    internal readonly int Key;
    internal readonly int Value;

    internal CreatureTypeFilter(CreatureType key, int value)
    {
      Key = (int)key;
      Value = value;
    }

    /// <summary>
    /// Creates a filter that matches creatures by alive or dead state.
    /// </summary>
    /// <param name="alive">True to match living creatures; false to match dead creatures.</param>
    /// <returns>A filter for the alive/dead state.</returns>
    public static CreatureTypeFilter Alive(bool alive)
    {
      return new CreatureTypeFilter(CreatureType.IsAlive, alive.ToInt());
    }

    /// <summary>
    /// Creates a filter that matches creatures that have levels in the specified class.
    /// </summary>
    /// <param name="nwClass">The class to match.</param>
    /// <returns>A filter for the specified class.</returns>
    public static CreatureTypeFilter Class(NwClass nwClass)
    {
      return new CreatureTypeFilter(CreatureType.Class, nwClass.Id);
    }

    /// <summary>
    /// Creates a filter that matches creatures that do not have the specified active spell effect.
    /// </summary>
    /// <param name="spellEffect">The spell effect to check.</param>
    /// <returns>A filter for the absence of the specified spell effect.</returns>
    public static CreatureTypeFilter DoesNotHaveSpellEffect(NwSpell spellEffect)
    {
      return new CreatureTypeFilter(CreatureType.DoesNotHaveSpellEffect, spellEffect.Id);
    }

    /// <summary>
    /// Creates a filter that matches creatures that have the specified active spell effect.
    /// </summary>
    /// <param name="spellEffect">The spell effect to check.</param>
    /// <returns>A filter for the presence of the specified spell effect.</returns>
    public static CreatureTypeFilter HasSpellEffect(NwSpell spellEffect)
    {
      return new CreatureTypeFilter(CreatureType.HasSpellEffect, spellEffect.Id);
    }

    /// <summary>
    /// Creates a filter that matches creatures based on perception state.
    /// </summary>
    /// <param name="perceptionType">The perception type to match.</param>
    /// <returns>A filter for the specified perception state.</returns>
    public static CreatureTypeFilter Perception(PerceptionType perceptionType)
    {
      return new CreatureTypeFilter(CreatureType.Perception, (int)perceptionType);
    }

    /// <summary>
    /// Creates a filter that matches player-controlled characters or non-player creatures.
    /// </summary>
    /// <param name="isPc">True to match player characters; false to match non-player creatures.</param>
    /// <returns>A filter for player control.</returns>
    public static CreatureTypeFilter PlayerChar(bool isPc)
    {
      return new CreatureTypeFilter(CreatureType.PlayerChar, isPc.ToInt());
    }

    /// <summary>
    /// Creates a filter that matches creatures of the specified racial type.
    /// </summary>
    /// <param name="race">The race to match.</param>
    /// <returns>A filter for the specified race.</returns>
    public static CreatureTypeFilter Race(NwRace race)
    {
      return new CreatureTypeFilter(CreatureType.RacialType, race.Id);
    }

    /// <summary>
    /// Creates a filter that matches creatures by reputation relationship.
    /// </summary>
    /// <param name="reputationType">The reputation relationship to match.</param>
    /// <returns>A filter for the specified reputation relationship.</returns>
    public static CreatureTypeFilter Reputation(ReputationType reputationType)
    {
      return new CreatureTypeFilter(CreatureType.Reputation, (int)reputationType);
    }
  }
}
