using System.Linq;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A spell definition.
  /// </summary>
  public sealed class NwSpell
  {
    private readonly CNWSpell spell;

    public NwSpell(CNWSpell spell, Spell spellType)
    {
      this.spell = spell;
      SpellType = spellType;
    }

    /// <summary>
    /// Gets the associated <see cref="Spell"/> type for this spell.
    /// </summary>
    public Spell SpellType { get; }

    /// <summary>
    /// Resolves a <see cref="NwSpell"/> from a spell id.
    /// </summary>
    /// <param name="spellId">The id of the spell to resolve.</param>
    /// <returns>The associated <see cref="NwSpell"/> instance. Null if the spell id is invalid.</returns>
    public static NwSpell FromSpellId(int spellId)
    {
      return NwRuleset.Spells.ElementAtOrDefault(spellId);
    }

    /// <summary>
    /// Resolves a <see cref="NwSpell"/> from a <see cref="Anvil.API.Spell"/>.
    /// </summary>
    /// <param name="spellType">The spell type to resolve.</param>
    /// <returns>The associated <see cref="NwSpell"/> instance. Null if the spell type is invalid.</returns>
    public static NwSpell FromSpellType(Spell spellType)
    {
      return NwRuleset.Spells.ElementAtOrDefault((int)spellType);
    }
  }
}
