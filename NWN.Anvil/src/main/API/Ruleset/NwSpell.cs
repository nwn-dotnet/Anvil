using System.Linq;
using Anvil.Services;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A spell definition.
  /// </summary>
  public sealed class NwSpell
  {
    [Inject]
    private static TlkTable TlkTable { get; set; }

    private readonly CNWSpell spellInfo;

    internal NwSpell(uint spellId, CNWSpell spellInfo)
    {
      Id = spellId;
      this.spellInfo = spellInfo;
    }

    /// <summary>
    /// Gets the description of this spell.
    /// </summary>
    public string Description
    {
      get => TlkTable.GetSimpleString(spellInfo.m_strrefDesc);
    }

    /// <summary>
    /// Gets the ID of this skill.
    /// </summary>
    public uint Id { get; }

    /// <summary>
    /// Gets the name of this spell.
    /// </summary>
    public string Name
    {
      get => TlkTable.GetSimpleString((uint)spellInfo.m_strrefName);
    }

    /// <summary>
    /// Gets the associated <see cref="Spell"/> type for this spell.
    /// </summary>
    public Spell SkillType
    {
      get => (Spell)Id;
    }

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

    public static implicit operator NwSpell(Spell spellType)
    {
      return NwRuleset.Spells.ElementAtOrDefault((int)spellType);
    }
  }
}
