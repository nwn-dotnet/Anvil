namespace Anvil.API
{
  public sealed class SpecialAbility(NwSpell spell, byte casterLevel, bool ready = true)
  {
    /// <summary>
    /// Gets the caster level of this special ability.
    /// </summary>
    public byte CasterLevel { get; set; } = casterLevel;

    /// <summary>
    /// Gets if this special ability is ready to use.
    /// </summary>
    public bool Ready { get; set; } = ready;

    /// <summary>
    /// Gets the spell associated with this special ability.
    /// </summary>
    public NwSpell Spell { get; set; } = spell;
  }
}
