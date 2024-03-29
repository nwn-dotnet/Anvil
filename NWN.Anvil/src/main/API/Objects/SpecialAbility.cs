namespace Anvil.API
{
  public sealed class SpecialAbility
  {
    public SpecialAbility(NwSpell spell, byte casterLevel, bool ready = true)
    {
      Spell = spell;
      Ready = ready;
      CasterLevel = casterLevel;
    }

    /// <summary>
    /// Gets the caster level of this special ability.
    /// </summary>
    public byte CasterLevel { get; set; }

    /// <summary>
    /// Gets if this special ability is ready to use.
    /// </summary>
    public bool Ready { get; set; }

    /// <summary>
    /// Gets the spell associated with this special ability.
    /// </summary>
    public NwSpell Spell { get; set; }
  }
}
