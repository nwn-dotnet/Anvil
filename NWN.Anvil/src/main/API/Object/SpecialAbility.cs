namespace Anvil.API
{
  public sealed class SpecialAbility
  {
    /// <summary>
    /// Gets the spell associated with this special ability.
    /// </summary>
    public Spell Spell { get; set; }

    /// <summary>
    /// Gets if this special ability is ready to use.
    /// </summary>
    public bool Ready { get; set; }

    /// <summary>
    /// Gets the caster level of this special ability.
    /// </summary>
    public byte CasterLevel { get; set; }

    public SpecialAbility(Spell spell, byte casterLevel, bool ready = true)
    {
      Spell = spell;
      Ready = ready;
      CasterLevel = casterLevel;
    }
  }
}
