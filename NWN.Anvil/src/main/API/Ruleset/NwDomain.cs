using System.Collections.Generic;
using System.Linq;
using NWN.Native.API;

namespace Anvil.API
{
  /// <summary>
  /// A selectable domain type used by classes with the <see cref="NwClass.HasDomains"/> property (e.g. clerics)
  /// </summary>
  public sealed class NwDomain
  {
    private readonly CNWDomain domainInfo;

    internal NwDomain(byte domainId, CNWDomain domainInfo)
    {
      Id = domainId;
      this.domainInfo = domainInfo;
    }

    /// <summary>
    /// Gets if the feat provides an active property, e.g. the Death domain's "Negative Plane Avatar" ability.
    /// </summary>
    public bool CastableFeat => domainInfo.m_bCastableFeat.ToBool();

    /// <summary>
    /// Gets the description for this domain, as shown in the character generation and level up screens.
    /// </summary>
    public StrRef Description => new StrRef(domainInfo.m_nDescriptionStrref);

    /// <summary>
    /// Gets the associated <see cref="Domain"/> type for this domain.
    /// </summary>
    public Domain DomainType => (Domain)Id;

    /// <summary>
    /// Gets the feat that is granted when selecting this domain.
    /// </summary>
    public NwFeat? GrantedFeat => NwFeat.FromFeatId(domainInfo.m_nGrantedFeat);

    /// <summary>
    /// Gets the icon resref to use for this domain.
    /// </summary>
    public string? Icon => domainInfo.m_cIcon.ToString();

    /// <summary>
    /// Gets the ID of this domain.
    /// </summary>
    public byte Id { get; }

    /// <summary>
    /// Gets if this is a correctly configured domain.
    /// </summary>
    public bool IsValidDomain => domainInfo.m_bValidDomain.ToBool();

    /// <summary>
    /// Gets the name of this domain.
    /// </summary>
    public StrRef Name => new StrRef(domainInfo.m_nNameStrref);

    /// <summary>
    /// Gets a list of spells specific to this domain.
    /// </summary>
    /// <remarks>
    /// This property returns a fixed size list of 10, matching the number of spell levels (cantrips + 9 levels)<br/>
    /// Levels that do not grant a spell for a domain return a null <see cref="NwSpell"/> in that slot.
    /// </remarks>
    public IReadOnlyList<NwSpell?> Spells => domainInfo.m_lstSpells.Select(spellId => NwSpell.FromSpellId((int)spellId)).ToArray();

    /// <summary>
    /// Resolves a <see cref="NwDomain"/> from a <see cref="Domain"/>.
    /// </summary>
    /// <param name="domainType">The domain type to resolve.</param>
    /// <returns>The associated <see cref="NwDomain"/> instance. Null if the domain type is invalid.</returns>
    public static NwDomain? FromDomainType(Domain domainType)
    {
      return NwRuleset.Domains.ElementAtOrDefault((int)domainType);
    }

    public static implicit operator NwDomain?(Domain domainType)
    {
      return NwRuleset.Domains.ElementAtOrDefault((int)domainType);
    }

    /// <summary>
    /// Resolves a <see cref="NwDomain"/> from a domain id.
    /// </summary>
    /// <param name="domainId">The id of the domain to resolve.</param>
    /// <returns>The associated <see cref="NwDomain"/> instance. Null if the domain id is invalid.</returns>
    public static NwDomain? FromDomainId(int domainId)
    {
      return NwRuleset.Domains.ElementAtOrDefault(domainId);
    }
  }
}
