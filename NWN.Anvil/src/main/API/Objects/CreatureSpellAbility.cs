using NWN.Core;

namespace Anvil.API
{
  /// <summary>
  /// Represents a spell-like ability on a creature.
  /// </summary>
  public sealed class CreatureSpellAbility
  {
    private readonly NwCreature creature;

    /// <summary>
    /// Get the internal index of this spell ability.
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Gets the associated spell cast when using this spell ability.
    /// </summary>
    public NwSpell Spell => NwSpell.FromSpellId(NWScript.GetSpellAbilitySpell(creature, Index))!;

    /// <summary>
    /// Gets the caster level of the spell ability.
    /// </summary>
    public int CasterLevel => NWScript.GetSpellAbilityCasterLevel(creature, Index);

    /// <summary>
    /// Gets or sets if this spell ability is ready and can be used.
    /// </summary>
    public bool Ready
    {
      get => NWScript.GetSpellAbilityReady(creature, Index).ToBool();
      set => NWScript.SetSpellAbilityReady(creature, Index, value.ToInt());
    }

    internal CreatureSpellAbility(NwCreature creature, int index)
    {
      this.creature = creature;
      Index = index;
    }
  }
}
